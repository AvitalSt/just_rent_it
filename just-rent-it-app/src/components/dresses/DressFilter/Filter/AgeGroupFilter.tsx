"use client";

import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { UnifiedMultiSelect } from "@/components/ui/filters/UnifiedMultiSelect";
import { useAppSelector } from "@/store/hooks";
import { FilterMultiSelectProps } from "@/models/types/dress/FilterMultiSelect.types";

export default function AgeGroupFilter({
  value,
  onChange,
}: FilterMultiSelectProps) {
  const list = useAppSelector((s) => s.options.ageGroups);

  return (
    <UnifiedSelector
      label="קהל יעד"
      hasValue={value.length > 0}
      onClear={() => onChange([])}
      count={value.length}
    >
      <UnifiedMultiSelect
        label="קהל יעד"
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
