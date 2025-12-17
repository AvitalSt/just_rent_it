"use client";

import { MultiSelect } from "@/components/ui/filters/MultiSelect";
import { UnifiedMultiSelectProps } from "@/models/types/ui/UnifiedMultiSelect.types";

export function UnifiedMultiSelect({
  label,
  options,
  value,
  onChange,
  required = false,
}: UnifiedMultiSelectProps) {
  return (
    <div className="w-full space-y-1" dir="rtl">
      <MultiSelect
        label={label}
        options={options}
        selected={value}
        onChange={(v) => onChange(v as number[])}
        multiple={true}
        required={required}
      />
    </div>
  );
}
