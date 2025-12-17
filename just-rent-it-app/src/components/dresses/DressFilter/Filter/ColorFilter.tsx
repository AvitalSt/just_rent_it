"use client";

import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { UnifiedMultiSelect } from "@/components/ui/filters/UnifiedMultiSelect";
import { useAppSelector } from "@/store/hooks";
import { FilterMultiSelectProps } from "@/models/types/dress/FilterMultiSelect.types";

export default function ColorFilter({
  value,
  onChange,
}: FilterMultiSelectProps) {
  const list = useAppSelector((s) => s.options.colors);

  return (
    <UnifiedSelector
      label="צבע"
      hasValue={value.length > 0}
      onClear={() => onChange([])}
      count={value.length}
    >
      <UnifiedMultiSelect
        label="צבע"
        options={list.map((c) => ({
          value: c.id,
          label: c.nameHebrew,
        }))}
        value={value}
        onChange={onChange}
      />
    </UnifiedSelector>
  );
}
