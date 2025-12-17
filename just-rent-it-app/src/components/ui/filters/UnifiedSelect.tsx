"use client";

import { UnifiedSingleSelectProps } from "@/models/types/ui/UnifiedSingleSelect.types";
import { SingleSelect } from "./SingleSelect";

export default function UnifiedSelect({
  label,
  value,
  options,
  onChange,
  required = false,
}: UnifiedSingleSelectProps) {
  return (
    <div className="w-full space-y-1" dir="rtl">

      <label className="block text-sm font-semibold text-gray-700">
        {label}
        {required && <span className="text-red-500">*</span>}
      </label>

      <SingleSelect
        options={options}
        selected={value}         
        onChange={(v) => onChange(v)}
      />
    </div>
  );
}
