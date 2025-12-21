"use client";

import React, { useEffect } from "react";
import { clearUser, setUser, setWishlistDressIds } from "@/store/userSlice";
import { currentUser } from "@/services/userService";
import { useAppDispatch, useAppSelector } from "@/store/hooks";

interface AppWrapperProps {
  children: React.ReactNode;
}

export const AppWrapper: React.FC<AppWrapperProps> = ({ children }) => {
  const dispatch = useAppDispatch();
  const user = useAppSelector((state) => state.user.user);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token || user) return;

    const fetchCurrentUser = async () => {
      try {
        const res = await currentUser(token);
        dispatch(setUser({ user: res }));
        dispatch(setWishlistDressIds(res.wishlistDressIds));
      } catch {
        localStorage.removeItem("token");
        dispatch(clearUser());
      }
    };

    fetchCurrentUser();
  }, [user, dispatch]);

  return <>{children}</>;
};
