"use client";

import { useEffect, useState } from "react";

export function useLocalWishlist() {
  const [localWishlist, setLocalWishlist] = useState<number[]>([]);
  const [showConfirm, setShowConfirm] = useState(false);
  const [pendingDressId, setPendingDressId] = useState<number | null>(null);

  useEffect(() => {
    try {
      const stored = JSON.parse(localStorage.getItem("local-wishlist") || "[]");
      if (Array.isArray(stored)) {
        setLocalWishlist(stored);
      }
    } catch {
      setLocalWishlist([]);
    }
  }, []);

  const toggle = (dressId: number, isUserLoggedIn: boolean) => {
  const stored: number[] = JSON.parse(localStorage.getItem("local-wishlist") || "[]");
  const exists = stored.includes(dressId);

  const newList = exists ? stored.filter(id => id !== dressId) : [...stored, dressId];
  localStorage.setItem("local-wishlist", JSON.stringify(newList));
  setLocalWishlist(newList);

  // אם המשתמש לא מחובר והרשימה ריקה לפני הוספה, הצג מודל
  if (!isUserLoggedIn && stored.length === 0 && !exists) {
    setPendingDressId(dressId);
    setShowConfirm(true);
  }

  return !exists;
};


  const remove = (dressId: number) => {
    const stored: number[] = JSON.parse(localStorage.getItem("local-wishlist") || "[]");
    const newList = stored.filter(id => id !== dressId);
    localStorage.setItem("local-wishlist", JSON.stringify(newList));
    setLocalWishlist(newList);
  };

  const isFavorite = (dressId: number) => localWishlist.includes(dressId);

  // פונקציות למודל
  const handleConfirm = () => {
    if (pendingDressId !== null) {
      // כאן אפשר להפנות ללוגין
      window.location.href = "/login";
    }
  };

  const handleCancel = () => { 
    setShowConfirm(false);
  };

  return {
    localWishlist,
    toggle,
    remove,
    isFavorite,
    showConfirm,
    handleConfirm,
    handleCancel,
  };
}
