"use client";

import { useLocalWishlist } from "./useLocalWishlist";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { toggleWishlist } from "@/store/userSlice";
import {
  addToWishlist,
  removeFromWishlist,
} from "@/services/wishlistService";

export function useWishlist() {
  const dispatch = useAppDispatch();
  const user = useAppSelector((s) => s.user.user);

  const {
    localWishlist,
    toggle: toggleLocal,
    remove: removeLocal,
    isFavorite: isLocalFavorite,
    showConfirm,
    handleConfirm,
    handleCancel,
  } = useLocalWishlist();

  const isFavorite = (id: number) =>
    user
      ? user.wishlistDressIds?.includes(id)
      : isLocalFavorite(id);

  const add = async (id: number) => {
    if (!user) {
      toggleLocal(id, false);
      return;
    }

    dispatch(toggleWishlist(id));
    try {
      await addToWishlist(id);
    } catch {
      dispatch(toggleWishlist(id));
    }
  };

  const remove = async (id: number) => {
    if (!user) {
      removeLocal(id);
      return;
    }

    dispatch(toggleWishlist(id));
    try {
      await removeFromWishlist(id);
    } catch {
      dispatch(toggleWishlist(id));
    }
  };

  return {
    isFavorite,
    add,
    remove,
    showConfirm,
    handleConfirm,
    handleCancel,
  };
}
