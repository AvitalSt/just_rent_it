"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";

import { useAppSelector } from "@/store/hooks";

import Gallery from "./Gallery";
import FavoriteButton from "./FavoriteButton";
import Details from "./Details";
import ConfirmModal from "@/components/ui/ConfirmModal";
import Interest from "./Interest";
import DressEditForm from "./DressEditForm";
import { DressOwnerDTO } from "@/models/DTOs/DressOwnerDTO";

import {
  activateDress,
  deleteDress,
  getDressOwner,
} from "@/services/dressService";
import { sendInterest } from "@/services/interestService";
import { Button } from "@/components/ui/Button";
import { DressViewProps } from "@/models/types/dress/DressView.types";
import axios from "axios";
import { clearDressesCache } from "@/services/dressCache";

export default function DressView({ dress, reload }: DressViewProps) {
  const API_BASE_ORIGIN = process.env.NEXT_PUBLIC_API_BASE_ORIGIN;

  const router = useRouter();
  const user = useAppSelector((s) => s.user.user);
  const isLoggedIn = !!user;
  const isAdmin = user?.role === 2;

  const [showDetails, setShowDetails] = useState(false);
  const [showOwner, setShowOwner] = useState(false);

  const [showInterest, setShowInterest] = useState(false);
  const [interestMessage, setInterestMessage] = useState("");

  const [isEditing, setIsEditing] = useState(false);

  const [modal, setModal] = useState<null | {
    text: string;
    confirm?: () => void;
  }>(null);
  const [loadingInterest, setLoadingInterest] = useState(false);

  const [owner, setOwner] = useState<DressOwnerDTO | null>(null);

  const [loadingActivate, setLoadingActivate] = useState(false);
  const [loadingDelete, setLoadingDelete] = useState(false);

  const toImgUrl = (path?: string) => {
    if (!path) return "";
    return path.startsWith("http")
      ? path
      : `${API_BASE_ORIGIN}${path.startsWith("/") ? "" : "/"}${path}`;
  };

  const galleryImages = [
    dress.mainImage,
    ...dress.images
      .filter((img) => img.imagePath !== dress.mainImage)
      .map((img) => img.imagePath),
  ].map(toImgUrl);

  const toggleButtonClasses =
    "w-full py-3 rounded-lg border bg-gray-100 hover:bg-gray-200 font-medium transition";

  const handleInterestClick = () => {
    if (!isLoggedIn) {
      setModal({
        text: "כדי להתעניין בשמלה יש להתחבר למערכת",
        confirm: () => router.push("/login"),
      });
      return;
    }
    setShowInterest((p) => !p);
  };

  const submitInterest = async () => {
    setLoadingInterest(true);
    try {
      await sendInterest(dress.dressID, interestMessage);
      setShowInterest(false);
      setInterestMessage("");
      setModal({ text: "ההתעניינות נשלחה בהצלחה!" });
    } catch (err) {
      if (axios.isAxiosError(err)) {
        const message =
          err.response?.data?.message ||
          "לא ניתן לשלוח כרגע. אנא נסי שוב מאוחר יותר.";
        setModal({ text: message });
      } else {
        setModal({
          text: "אירעה שגיאה בלתי צפויה. אנא נסי שוב מאוחר יותר.",
        });
      }
    } finally {
      setLoadingInterest(false);
    }
  };

  const toggleOwner = async () => {
    if (!showOwner && !owner) {
      const res = await getDressOwner(dress.dressID);
      setOwner(res);
    }
    setShowOwner((p) => !p);
  };

  const handleToggleStatus = async () => {
    if (loadingActivate) return;
    setLoadingActivate(true);
    try {
      await activateDress(dress.dressID);
      clearDressesCache();
      reload();
      setModal({ text: "הסטטוס עודכן." });
    } catch {
      setModal({ text: "שגיאה בעדכון הסטטוס." });
    } finally {
      setLoadingActivate(false);
    }
  };

  const handleDelete = async () => {
    if (loadingDelete) return;
    setLoadingDelete(true);
    try {
      await deleteDress(dress.dressID);
      clearDressesCache();
      router.push("/dresses");
    } catch {
      setModal({ text: "שגיאה במחיקה." });
    } finally {
      setLoadingDelete(false);
    }
  };

  return (
    <div className="space-y-8">
      <div className="grid md:grid-cols-2 gap-8">
        <Gallery images={galleryImages} views={dress.views} />

        <div className="flex flex-col space-y-5">
          <div className="flex items-start justify-between">
            <h1 className="text-3xl font-bold text-gray-900">{dress.name}</h1>
            <FavoriteButton dressId={dress.dressID} />
          </div>

          <div className="text-3xl font-semibold">₪{dress.price}</div>
          <p className="text-gray-500">{dress.description}</p>

          <button
            className={toggleButtonClasses}
            onClick={() => setShowDetails((p) => !p)}
          >
            {showDetails ? "הסתר פרטי שמלה" : "הצג פרטי שמלה"}
          </button>

          {showDetails && <Details dress={dress} />}

          {!isAdmin && (
            <>
              <button
                className="w-full bg-black text-white py-4 rounded-lg"
                onClick={handleInterestClick}
              >
                {showInterest ? "סגירת התעניינות" : "מתעניינת בשמלה"}
              </button>

              {showInterest && (
                <Interest
                  dress={dress}
                  interestMessage={interestMessage}
                  setInterestMessage={setInterestMessage}
                  onSubmit={submitInterest}
                  onClose={() => setShowInterest(false)}
                  loading={loadingInterest}
                />
              )}
            </>
          )}
        </div>
      </div>

      {isAdmin && (
        <div className="border-t pt-6 space-y-6">
          <button className={toggleButtonClasses} onClick={toggleOwner}>
            {showOwner ? "הסתר פרטי מפרסם" : "הצג פרטי מפרסם"}
          </button>

          {showOwner && owner && (
            <div className="border p-4 rounded bg-gray-50">
              <h3 className="font-bold pb-2 text-lg">פרטי מפרסם:</h3>
              <p>
                שם: {owner.firstName} {owner.lastName}
              </p>
              <p>טלפון: {owner.phone}</p>
              <p>אימייל: {owner.email}</p>
            </div>
          )}

          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <Button onClick={() => setIsEditing((x) => !x)} className="py-3">
              {isEditing ? "סגור עריכה" : "עריכת שמלה"}
            </Button>

            <Button
              onClick={handleToggleStatus}
              disabled={dress.status === 2 || loadingActivate}
              className="py-3"
            >
              {loadingActivate
                ? "מעדכן..."
                : dress.status === 2
                ? "שמלה פעילה"
                : "הפעל שמלה"}
            </Button>

            {dress.status !== 3 && (
              <Button onClick={handleDelete} disabled={loadingDelete}>
                {loadingDelete ? "מוחק..." : "מחק שמלה"}
              </Button>
            )}
          </div>

          {isEditing && (
            <div className="bg-gray-50 p-6 rounded-lg shadow-inner">
              <DressEditForm
                dress={dress}
                onCancel={() => setIsEditing(false)}
                onSaved={() => {
                  reload();
                  setIsEditing(false);
                }}
              />
            </div>
          )}
        </div>
      )}

      {modal && (
        <ConfirmModal
          message={modal.text}
          onConfirm={modal.confirm}
          onCancel={() => setModal(null)}
        />
      )}
    </div>
  );
}
