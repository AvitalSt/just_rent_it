"use client";

import { useAppSelector } from "@/store/hooks";
import Loading from "../ui/Loading";

export default function AdminOnly({ children }: { children: React.ReactNode }) {
  const user = useAppSelector((state) => state.user.user);

  if (user === undefined) return <Loading />;
  
  if (user === null || user.role !== 2) {
    return (
      <div
        dir="rtl"
        className="w-full h-[70vh] flex items-center justify-center text-3xl font-bold text-gray-400 tracking-wide"
      >
        עמוד זה מיועד למנהלים בלבד
      </div>
    );
  }

  return <>{children}</>;
}
