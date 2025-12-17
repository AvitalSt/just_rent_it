"use client";

import { useEffect } from "react";
import { useAppDispatch } from "@/store/hooks";
import { fetchOptions } from "@/store/optionsSlice";

export function InitializeOptions() {
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(fetchOptions());
  }, [dispatch]);

  return null;
}
