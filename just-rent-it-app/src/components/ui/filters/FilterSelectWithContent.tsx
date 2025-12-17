"use client";

import { useState, useRef, useEffect } from "react";
import { ChevronDown, X } from "lucide-react";
import { FilterSelectWithContentProps } from "@/models/types/ui/FilterSelectWithContent.types";

export default function FilterSelectWithContent({
  title,
  hasValue = false,
  onClear,
  children,
}: FilterSelectWithContentProps) {
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!open) return;

    const handler = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
      }
    };

    document.addEventListener("mousedown", handler);
    return () => document.removeEventListener("mousedown", handler);
  }, [open]);

  return (
    <div ref={ref} className="relative w-full mb-2">
      {/* בלוק בחירה - לא כפתור! */}
      <div
        onClick={() => setOpen(!open)}
        className={`flex items-center justify-between px-4 py-2.5 rounded-lg border cursor-pointer bg-white
          ${open ? "border-black shadow-sm" : "border-gray-300"}
          ${hasValue ? "text-black font-medium" : "text-gray-500"}
        `}
      >
        <span>{title}</span>

        <div className="flex items-center gap-2">
          {hasValue && onClear && (
            <span
              onClick={(e) => {
                e.stopPropagation();
                onClear();
              }}
              className="p-1 rounded-full hover:bg-gray-100 cursor-pointer"
            >
              <X className="w-4 h-4 text-gray-500" />
            </span>
          )}

          <ChevronDown
            className={`w-4 h-4 text-gray-600 transition-transform ${
              open ? "rotate-180" : ""
            }`}
          />
        </div>
      </div>

      {open && (
        <div className="absolute z-30 mt-2 p-4 w-full bg-white border rounded-lg shadow-xl">
          {children}
        </div>
      )}
    </div>
  );
}
