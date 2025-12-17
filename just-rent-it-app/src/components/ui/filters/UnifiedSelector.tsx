"use client";

import { useState, useRef, useEffect } from "react";
import { ChevronDown, X } from "lucide-react";

interface UnifiedSelectorProps {
  label: string;
  hasValue?: boolean;
  onClear?: () => void;
  children: React.ReactNode;
  required?: boolean;
  count?: number;
}

export default function UnifiedSelector({
  label,
  hasValue = false,
  onClear,
  children,
  required = false,
  count,   
}: UnifiedSelectorProps) {
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (!open) return;

    const close = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) {
        setOpen(false);
      }
    };

    document.addEventListener("mousedown", close);
    return () => document.removeEventListener("mousedown", close);
  }, [open]);

  return (
    <div ref={ref} className="relative w-full" dir="rtl">
      {/* Header */}
     <div
  onClick={() => setOpen(!open)}
  className={`flex items-center justify-between px-4 py-2.5 rounded-lg border cursor-pointer bg-white
    ${open ? "border-black shadow-sm" : "border-gray-300"}
    ${hasValue ? "text-black font-medium" : "text-gray-500"}
  `}
>
  <div className="flex items-center gap-2">
    <span>
      {label}
      {required && <span className="text-red-500">*</span>}
    </span>

    {count !== undefined && count > 0 && (
      <span className="text-gray-400 text-xs ml-1">({count})</span>
    )}
  </div>

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
      className={`w-4 h-4 transition-transform ${
        open ? "rotate-180" : ""
      }`}
    />
  </div>
</div>


      {/* Dropdown */}
      {open && (
        <div className="absolute z-40 mt-2 p-4 w-full bg-white border rounded-lg shadow-xl">
          {children}
        </div>
      )}
    </div>
  );
}
