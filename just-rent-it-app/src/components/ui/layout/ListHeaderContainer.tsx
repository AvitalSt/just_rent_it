"use client";

import { ListHeaderContainerProps } from "@/models/types/ui/layout/ListHeaderContainer.type";
import { Loader2 } from "lucide-react";

export default function ListHeaderContainer({
  title, // כותרת הרשימה
  total, // מספר תוצאות
  loading, // האם בעיצוב טוען
}: ListHeaderContainerProps) {
  return (
    <div className="flex items-center gap-3">
      <div className="h-10 w-1 bg-black rounded-full"></div>
      <div>
        <h1 className="text-xl font-bold text-gray-900">{title}</h1>

        <p className="text-sm text-gray-500 mt-0.5">
          {loading ? (
            <span className="flex items-center gap-2">
              <Loader2 className="w-3 h-3 animate-spin" />
              טוען...
            </span>
          ) : (
            <span>נמצאו {total} תוצאות</span>
          )}
        </p>
      </div>
    </div>
  );
}
