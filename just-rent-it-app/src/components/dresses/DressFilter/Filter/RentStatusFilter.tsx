"use client";

import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { UnifiedMultiSelect } from "@/components/ui/filters/UnifiedMultiSelect";
import { RentStatusMap } from "@/models/Enums/filtersMap";

interface Props {
  value: number[];
  onChange: (v: number[]) => void;
}

export default function RentStatusFilter({ value, onChange }: Props) {
  const options = Object.entries(RentStatusMap).map(([key, label]) => ({
    value: Number(key),
    label,
  }));

  return (
    <UnifiedSelector
      label="סטטוס"
      hasValue={value.length > 0}
      onClear={() => onChange([])}
      count={value.length}
    >
      <UnifiedMultiSelect
        label="סטטוס"
        options={options}
        value={value}
        onChange={onChange}
      />
    </UnifiedSelector>
  );
}
