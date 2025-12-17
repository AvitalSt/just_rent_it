"use client";

import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { UnifiedMultiSelect } from "@/components/ui/filters/UnifiedMultiSelect";
import { useAppSelector } from "@/store/hooks";
import { FilterMultiSelectProps } from "@/models/types/dress/FilterMultiSelect.types";

export default function SizeFilter({
  value,
  onChange,
}: FilterMultiSelectProps) {
  const list = useAppSelector((s) => s.options.sizes);

  const options = list.map((s) => ({
    value: s.id,
    label: s.name,
  }));

  return (
    <UnifiedSelector
      label="מידה"
      hasValue={value.length > 0}
      onClear={() => onChange([])}
      count={value.length}
    >
      <UnifiedMultiSelect
        label="מידה"
        options={options}
        value={value}
        onChange={onChange}
      />
    </UnifiedSelector>
  );
}
