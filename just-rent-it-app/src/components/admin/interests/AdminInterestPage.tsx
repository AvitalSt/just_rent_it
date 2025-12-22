"use client";

import { useEffect, useState } from "react";
import {
  getFilteredInterests,
  updateStatus,
  updateNotes,
  updateOwnerComment,
  updateUserComment,
  updatePaymentAmount,
  messageOwner,
  messageUser,
  messageOwnerPayment,
  mapFiltersToInterestParams,
  exportInterests,
} from "@/services/interestService";

import { InterestListDTO } from "@/models/DTOs/InterestListDTO";
import { InterestDraftFilters } from "@/models/types/interest/InterestFilterParams";

import InterestFilterDrawer from "./InterestFilterDrawer";
import InterestTable from "./InterestTable";
import { ErrorMessage } from "@/components/ui/ErrorMessage";
import AdminOnly from "../AdminOnly";
import ListHeaderContainer from "@/components/ui/layout/ListHeaderContainer";
import InfiniteScrollContainer from "@/components/ui/layout/InfiniteScrollContainer";
import { useRouter } from "next/navigation";
import PageAnchorObserver from "@/components/ui/layout/PageAnchorObserver";
import { useAppSelector } from "@/store/hooks";
import {
  interestsCache,
  totalCountCache,
  clearInterestsCache,
} from "@/services/interestCache";

export default function AdminInterestPage() {
  const router = useRouter();

  const user = useAppSelector((state) => state.user.user);

  const [list, setList] = useState<InterestListDTO[]>([]);
  const [pageNumber, setPageNumber] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const pageSize = 50;

  const [loadError, setLoadError] = useState("");

  const [sendingOwner, setSendingOwner] = useState<Record<number, boolean>>({});
  const [sendingUser, setSendingUser] = useState<Record<number, boolean>>({});
  const [sendingPayment, setSendingPayment] = useState<Record<number, boolean>>(
    {}
  );
  const [exportLoading, setExportLoading] = useState(false);

  const [filterOpen, setFilterOpen] = useState(false);

  const [draftFilters, setDraftFilters] = useState<InterestDraftFilters>({
    status: [],
    ownerName: "",
    userName: "",
    dressName: "",
    from: "",
    to: "",
  });

  const [activeFilters, setActiveFilters] = useState<InterestDraftFilters>({
    status: [],
    ownerName: "",
    userName: "",
    dressName: "",
    from: "",
    to: "",
  });

  const getCacheKey = (f: InterestDraftFilters, page: number) => {
    return JSON.stringify({ ...f, pageNumber: page });
  };

  const load = async () => {
    try {
      setLoadError("");
      const key = getCacheKey(activeFilters, pageNumber);

      if (interestsCache[key]) {
        const cachedItems = interestsCache[key];
        if (pageNumber === 1) setList(cachedItems);
        else setList((prev) => [...prev, ...cachedItems]);
        setTotalCount(totalCountCache[key]);
        return;
      }

      const params = {
        ...mapFiltersToInterestParams(activeFilters),
        pageNumber,
        pageSize,
      };

      const result = await getFilteredInterests(params);

      interestsCache[key] = result.items;
      totalCountCache[key] = result.totalCount;
      if (pageNumber === 1) setList(result.items);
      else setList((prev) => [...prev, ...result.items]);
      setTotalCount(result.totalCount);
    } catch {
      setLoadError("נכשלה טעינת הנתונים");
    }
  };

  useEffect(() => {
    if (user?.role !== 2) return;
    load();
  }, [pageNumber, activeFilters, user]);

  const updateUrl = (filters: InterestDraftFilters) => {
    const raw = mapFiltersToInterestParams(filters);
    const params = new URLSearchParams();

    if (pageNumber > 1) {
      params.set("pageNumber", String(pageNumber));
    }
    for (const key in raw) {
      params.set(key, raw[key]!);
    }

    router.replace(`/interest?${params.toString()}`, {
      scroll: false,
    });
  };

  const applyFilters = () => {
    setPageNumber(1);
    setActiveFilters({ ...draftFilters });
    updateUrl(draftFilters);
  };

  const refresh = () => {
    const empty = {
      status: [],
      ownerName: "",
      userName: "",
      dressName: "",
      from: "",
      to: "",
    };

    setDraftFilters(empty);
    setActiveFilters(empty);
    setPageNumber(1);

    router.replace(`/interest`, { scroll: false });
  };

  const hasMore = pageNumber * pageSize < totalCount;

  const loadMore = () => {
    if (hasMore) setPageNumber((p) => p + 1);
  };

  const updateField = async (id: number, field: string, value: any) => {
    try {
      if (field === "status") await updateStatus(id, value);
      if (field === "notes") await updateNotes(id, value);
      if (field === "ownerComment") await updateOwnerComment(id, value);
      if (field === "userComment") await updateUserComment(id, value);
      if (field === "paymentAmount") await updatePaymentAmount(id, value);
      clearInterestsCache();
      setPageNumber(1);
      await load();
    } catch {
      alert("שמירה נכשלה");
    }
  };

  const sendOwnerMail = async (row: InterestListDTO) => {
    const id = row.interestID;

    try {
      setSendingOwner((p) => ({ ...p, [id]: true }));
      await messageOwner(id);

      setList((prev) =>
        prev.map((i) =>
          i.interestID === id
            ? { ...i, ownerMailCount: i.ownerMailCount + 1 }
            : i
        )
      );
    } finally {
      setTimeout(() => {
        setSendingOwner((p) => ({ ...p, [id]: false }));
      }, 60000);
    }
  };

  const sendUserMail = async (row: InterestListDTO) => {
    const id = row.interestID;

    try {
      setSendingUser((p) => ({ ...p, [id]: true }));
      await messageUser(id);

      setList((prev) =>
        prev.map((i) =>
          i.interestID === id ? { ...i, userMailCount: i.userMailCount + 1 } : i
        )
      );
    } finally {
      setTimeout(() => {
        setSendingUser((p) => ({ ...p, [id]: false }));
      }, 60000);
    }
  };

  const sendOwnerPaymentMail = async (row: InterestListDTO) => {
    const id = row.interestID;

    try {
      setSendingPayment((p) => ({ ...p, [id]: true }));
      await messageOwnerPayment(id);
    } catch {
      alert("שליחת המייל נכשלה");
      setSendingPayment((p) => ({ ...p, [id]: false }));
    }
  };

  const handleExport = async () => {
    try {
      setExportLoading(true);
      const params = mapFiltersToInterestParams(activeFilters);
      const res = await exportInterests(params);

      //Binary Large Object
      //יוצר URL זמני בזכרון הדפדפן שמצביע על קובץ Blob
      const url = window.URL.createObjectURL(new Blob([res.data]));
      const link = document.createElement("a");
      //הקישור מצביע על ה-URL של הקובץ
      link.href = url;
      //מגדיר כשאר לוחצים על הקישור במקום לנווט לכתובת URL צריך להוריד את התכון בשם ..
      link.download = "Interests.xlsx";
      //מבצע את הלחיצה מאוחרי הקלעים
      link.click();
    } catch (err) {
      console.error("Export failed:", err);
    } finally {
      setTimeout(() => {
        setExportLoading(false);
      }, 60000);
    }
  };

  return (
    <AdminOnly>
      <div className="p-6" dir="rtl">
        <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-4 mb-6">
          <div className="flex items-center justify-between gap-4">
            <ListHeaderContainer
              title="ניהול התעניינויות"
              total={totalCount}
              loading={false}
            />

            <div className="flex items-center gap-3">
              <button
                onClick={handleExport}
                disabled={exportLoading}
                className="
                            flex items-center gap-2 px-4 py-2 rounded-lg shadow
                            bg-black text-white
                            hover:bg-gray-800
                            disabled:bg-gray-400 disabled:cursor-not-allowed disabled:opacity-60
                            transition-all
                          "
              >
                ייצוא לאקסל
              </button>

              <InterestFilterDrawer
                open={filterOpen}
                setOpen={setFilterOpen}
                draftFilters={draftFilters}
                setDraftFilters={setDraftFilters}
                applyFilters={applyFilters}
                refresh={refresh}
              />
            </div>
          </div>
        </div>

        {loadError && <ErrorMessage message={loadError} />}

        <PageAnchorObserver list={list} basePath="/interest" />

        <InfiniteScrollContainer hasMore={hasMore} loadMore={loadMore}>
          <InterestTable
            list={list}
            pageSize={pageSize}
            sendingOwner={sendingOwner}
            sendingUser={sendingUser}
            sendingPayment={sendingPayment}
            updateField={updateField}
            sendOwnerMail={sendOwnerMail}
            sendUserMail={sendUserMail}
            sendOwnerPaymentMail={sendOwnerPaymentMail}
          />
        </InfiniteScrollContainer>
      </div>
    </AdminOnly>
  );
}
