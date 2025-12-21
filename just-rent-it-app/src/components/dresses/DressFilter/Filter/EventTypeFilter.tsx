"use client";

import { useAppSelector } from "@/store/hooks";
import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { MultiSelect } from "@/components/ui/filters/MultiSelect";
import { FilterMultiSelectProps } from "@/models/types/dress/FilterMultiSelect.types";

export default function EventTypeFilter({
  value,
  onChange,
}: FilterMultiSelectProps) {
  const list = useAppSelector((s) => s.options.eventTypes);

  return (
    <UnifiedSelector
      label="קטגוריה שמלות"
      hasValue={value.length > 0}
      onClear={() => onChange([])}
      count={value.length}
    >
      <MultiSelect
        label="קטגוריה שמלות"
        options={list.map((item) => ({
          value: item.id,
          label: item.nameHebrew,
        }))}
        selected={value}
        onChange={onChange}
      />
    </UnifiedSelector>
  );
}
