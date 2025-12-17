"use client";

import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { UnifiedMultiSelect } from "@/components/ui/filters/UnifiedMultiSelect";
import { SaleTypeMap } from "@/models/Enums/filtersMap";
import { FilterMultiSelectProps } from "@/models/types/dress/FilterMultiSelect.types";

export default function SaleTypeFilter({
  value,
  onChange,
}: FilterMultiSelectProps) {
  const options = Object.entries(SaleTypeMap).map(([id, label]) => ({
    value: Number(id),
    label,
  }));

  return (
    <UnifiedSelector
      label="סוג מודעה"
      hasValue={value.length > 0}
      onClear={() => onChange([])}
      count={value.length}
    >
      <UnifiedMultiSelect
        label="סוג מודעה"
        options={options}
        value={value}
        onChange={onChange}
      />
    </UnifiedSelector>
  );
}
