"use client";

import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { UnifiedMultiSelect } from "@/components/ui/filters/UnifiedMultiSelect";
import { DressStatusMap } from "@/models/Enums/filtersMap";
import { FilterMultiSelectProps } from "@/models/types/dress/FilterMultiSelect.types";

export default function StatusGroupFilter({
  value,
  onChange,
}: FilterMultiSelectProps) {
  const options = Object.entries(DressStatusMap).map(([id, label]) => ({
    value: Number(id),
    label,
  }));

  return (
    <UnifiedSelector
      label="סטטוס שמלה (מנהל)"
      hasValue={value.length > 0}
      onClear={() => onChange([])}
      count={value.length}
    >
      <UnifiedMultiSelect
        label="סטטוס שמלה (מנהל)"
        options={options}
        value={value}
        onChange={onChange}
      />
    </UnifiedSelector>
  );
}
