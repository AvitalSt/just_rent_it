"use client";

import React from "react";
import Image from "next/image";
import Link from "next/link";
import { Heart, X } from "lucide-react";
import { useWishlist } from "@/hooks/useWishlist";
import { DressProps } from "@/models/DTOs/DressListProps";
import ConfirmModal from "@/components/ui/ConfirmModal";

export default function DressCard({
  dress,
  onRemoved,
  isWishlistPage,
}: DressProps) {
  const API_BASE_ORIGIN = process.env.NEXT_PUBLIC_API_BASE_ORIGIN;

  const { isFavorite, add, remove, showConfirm, handleConfirm, handleCancel } =
    useWishlist();

  const toggleWishlistItem = (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();

    if (isWishlistPage) {
      onRemoved?.(dress.dressID);
    } else {
      isFavorite(dress.dressID) ? remove(dress.dressID) : add(dress.dressID);
    }
  };

  const imageUrl = dress.mainImage
    ? dress.mainImage.startsWith("http")
      ? dress.mainImage
      : `${API_BASE_ORIGIN}${dress.mainImage.startsWith("/") ? "" : "/"}${
          dress.mainImage
        }`
    : null;

  return (
    <>
      <Link
        href={`/dresses/${dress.dressID}`}
        className="group relative block bg-white overflow-hidden shadow-sm hover:shadow-xl transition-all duration-300"
      >
        <div className="relative w-full aspect-[2/3] bg-gray-200 overflow-hidden">
          {imageUrl ? (
            <Image
              src={imageUrl}
              alt={dress.name}
              fill
              unoptimized
              className="object-cover"
            />
          ) : (
            <div className="w-full h-full flex items-center justify-center text-gray-500">
              ללא תמונה
            </div>
          )}

          <button
            onClick={toggleWishlistItem}
            className="absolute top-3 left-3 w-10 h-10 bg-white/80 rounded-full shadow-md flex items-center justify-center z-10"
          >
            {isWishlistPage ? (
              <X className="w-5 h-5 text-black" />
            ) : (
              <Heart
                className={`w-5 h-5 ${
                  isFavorite(dress.dressID)
                    ? "fill-black text-black"
                    : "text-black"
                }`}
              />
            )}
          </button>
        </div>

        <div className="p-1" dir="rtl">
          <h3 className="text-sm text-gray-900 line-clamp-2">{dress.name}</h3>
          <p className="text-sm font-semibold text-gray-700">₪{dress.price}</p>
        </div>
      </Link>
      {showConfirm && (
        <ConfirmModal
          message="עלייך להתחבר או להירשם כדי לשמור שמלה במודעפים לתמיד"
          onConfirm={handleConfirm}
          onCancel={handleCancel}
        />
      )}
    </>
  );
}
