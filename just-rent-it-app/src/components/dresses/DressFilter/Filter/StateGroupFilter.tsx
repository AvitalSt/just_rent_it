"use client";

import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { UnifiedMultiSelect } from "@/components/ui/filters/UnifiedMultiSelect";
import { DressStateMap } from "@/models/Enums/filtersMap";
import { FilterMultiSelectProps } from "@/models/types/dress/FilterMultiSelect.types";

export default function StateGroupFilter({
  value,
  onChange,
}: FilterMultiSelectProps) {
  const options = Object.entries(DressStateMap).map(([id, label]) => ({
    value: Number(id),
    label,
  }));

  return (
    <UnifiedSelector
      label="מצב שמלה"
      hasValue={value.length > 0}
      onClear={() => onChange([])}
      count={value.length}
    >
      <UnifiedMultiSelect
        label="מצב שמלה"
        options={options}
        value={value}
        onChange={onChange}
      />
    </UnifiedSelector>
  );
}
