import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { MultiSelect } from "@/components/ui/filters/MultiSelect";
import { useAppSelector } from "@/store/hooks";
import { FilterMultiSelectProps } from "@/models/types/dress/FilterMultiSelect.types";

export default function CityFilter({ value, onChange }: FilterMultiSelectProps) {
  const list = useAppSelector((s) => s.options.cities);

  return (
    <UnifiedSelector
      label="עיר"
      hasValue={value.length > 0}
      onClear={() => onChange([])}
       count={value.length}
    >
      <MultiSelect
        label="עיר"
        options={list.map((c) => ({
          value: c.id,
          label: c.nameHebrew,
        }))}
        selected={value}
        onChange={onChange}
      />
    </UnifiedSelector>
  );
}
