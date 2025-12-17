"use client";

import { useEffect, useState } from "react";
import { useSearchParams, useRouter } from "next/navigation";
import { DressListDTO } from "@/models/DTOs/DressListDTO";
import {
  getFilteredDresses,
  mapFiltersToParams,
} from "@/services/dressService";
import FilterPanel from "./Filter/FilterPanel";
import DressGrid from "./DressGrid";
import {
  DressFilters,
  parseUrlToFilters,
} from "@/models/types/dress/DressFilter.types";
import { buildFilterParams } from "@/utils/filterUtils";
import MobileFilterDrawer from "./Mobile/MobileFilterDrawer";
import DressesHeader from "./Header/DressesHeader";
import PageAnchorObserver from "@/components/ui/layout/PageAnchorObserver";

const dressesCache: Record<string, DressListDTO[]> = {};
const totalResultsCache: Record<string, number> = {};
const maxPriceCache: Record<string, number> = {};

export default function DressListPage() {
  const router = useRouter();
  const searchParams = useSearchParams();

  const [mobileOpen, setMobileOpen] = useState(false);
  const [initialLoad, setInitialLoad] = useState(true);
  const [maxPrice, setMaxPrice] = useState(9999999);

  const [pageNumber, setPageNumber] = useState(1);
  const pageSize = 25;

  const defaultFilters: DressFilters = {
    cityIds: [],
    sizeIds: [],
    colorIds: [],
    ageGroupIds: [],
    eventTypeIds: [],
    saleTypeIds: [],
    stateGroupIds: [],
    statusGroupIds: [],
    priceMin: 0,
    priceMax: 9999999,
    orderBy: "",
  };

  //parseUrlToFilters יודע לקרוא את ה-URL ולפרק אותו למבנה מסודר של filters
  const initialFilters = parseUrlToFilters(
    searchParams, //מאפשר לקורא את המחרוזת מURL
    defaultFilters, //מבנה בסיסי של הפילטרים
    maxPrice //מגיע מהשרת מה המחיר הגבוה ביותר
  );

  //הפילטרים הפעילים
  const [filters, setFilters] = useState<DressFilters>(initialFilters);
  //פילטרים שהמשתמש סימן אבל עוד לא אישר
  const [draftFilters, setDraftFilters] = useState<DressFilters>({
    ...initialFilters,
  });

  const [dresses, setDresses] = useState<DressListDTO[]>([]);
  const [loading, setLoading] = useState(false);
  const [totalResults, setTotalResults] = useState(0);

  const updateUrl = (f: DressFilters, page?: number) => {
    //בונה את הפילטרים בצורה אחידה 2_3
    const raw = buildFilterParams(f);
    //URLSearchParams מאפשר לנו להוסיף פרמטרי ל- URL
    //ה- URLSearchParams עושה & אוטמטי
    const params = new URLSearchParams();

    const currentPage = page ?? pageNumber;
    if (currentPage > 1) params.set("pageNumber", String(currentPage));

    for (const key in raw) {
      const value = raw[key];
      if (
        key === "priceGroup" &&
        f.priceMin === 0 &&
        (f.priceMax === 9999999 || f.priceMax === maxPrice)
      )
        continue;
      //כולם נכנסים לפה
      if (value !== undefined) params.set(key, value);
    }

    router.replace(`/dresses?${params.toString()}`, { scroll: false });
  };

  const getCacheKey = (f: DressFilters, page: number) => {
    return JSON.stringify({ ...f, pageNumber: page });
  };

  useEffect(() => {
  const loadFiltered = async () => {
    setLoading(true);
    try {
      const key = getCacheKey(filters, pageNumber);

      if (dressesCache[key]) {
        const cachedItems = dressesCache[key];
        if (pageNumber === 1) setDresses(cachedItems);
        else setDresses((prev) => [...prev, ...cachedItems]);
        setTotalResults(totalResultsCache[key]);
        setMaxPrice(maxPriceCache[key]);
      } else {
        const params = { ...mapFiltersToParams(filters), pageNumber, pageSize };
        const res = await getFilteredDresses(params);

        dressesCache[key] = res.items;
        totalResultsCache[key] = res.totalCount;
        maxPriceCache[key] = res.maxPrice;

        if (pageNumber === 1) setDresses(res.items);
        else setDresses((prev) => [...prev, ...res.items]);

        setTotalResults(res.totalCount);
        setMaxPrice(res.maxPrice);
      }

      setInitialLoad(false);
    } finally {
      setLoading(false);
    }
  };

  loadFiltered();
}, [filters, pageNumber]);


  useEffect(() => {
    if (initialLoad) return;

    const currentPage = pageNumber;
    const isFilterChange = currentPage === 1;
    updateUrl(filters, isFilterChange ? 1 : currentPage);
  }, [filters, pageNumber]);

  const applyFilters = (quick?: DressFilters) => {
    const next = quick ?? draftFilters;

    setPageNumber(1);
    setFilters(next);
    updateUrl(next);
  };

  const hasMore = dresses.length < totalResults;
  const loadMore = () => {
    if (!hasMore || loading) return;
    if (hasMore) setPageNumber((prev) => prev + 1);
  };

  return (
    <div className="min-h-screen bg-gray-50" dir="rtl">
      <div className="w-full px-4 py-6">
        <MobileFilterDrawer
          mobileOpen={mobileOpen}
          setMobileOpen={setMobileOpen}
          draftFilters={draftFilters}
          setDraftFilters={setDraftFilters}
          applyFilters={applyFilters}
          maxPrice={maxPrice}
        />

        <div className="grid grid-cols-1 lg:grid-cols-5 gap-6">
          <div className="hidden lg:block lg:col-span-1">
            <FilterPanel
              draftFilters={draftFilters}
              setDraftFilters={setDraftFilters}
              applyFilters={applyFilters}
              maxPrice={maxPrice}
            />
          </div>

          <div className="lg:col-span-4">
            <DressesHeader
              loading={loading}
              totalResults={totalResults}
              draftFilters={draftFilters}
              setDraftFilters={setDraftFilters}
              applyFilters={applyFilters}
            />

            <PageAnchorObserver list={dresses} basePath="/dresses" />

            <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-6">
              <DressGrid
                dresses={dresses}
                pageSize={pageSize}
                pageNumber={pageNumber}
                hasMore={hasMore}
                loadMore={loadMore}
                loading={loading}
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
