"use client";

import { Heart } from "lucide-react";
import { useWishlist } from "@/hooks/useWishlist";

export default function FavoriteButton({ dressId }: { dressId: number }) {
  const { isFavorite, add, remove } = useWishlist();

  const toggle = () => {
    isFavorite(dressId) ? remove(dressId) : add(dressId);
  };

  return (
    <button onClick={toggle} className="p-2 rounded-full hover:bg-gray-100">
      <Heart
        className={`w-6 h-6 ${
          isFavorite(dressId) ? "fill-black text-black" : "text-black"
        }`}
      />
    </button>
  );
}
