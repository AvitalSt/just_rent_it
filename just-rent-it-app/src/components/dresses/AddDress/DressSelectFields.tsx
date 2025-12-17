import UnifiedSelector from "@/components/ui/filters/UnifiedSelector";
import { MultiSelect } from "@/components/ui/filters/MultiSelect";
import { DressSelectFieldsProps } from "@/models/types/dress/DressSelectFields.types";

export default function DressSelectFields({
  formData,
  handleChange,
  colors,
  sizes,
  cities,
  ageGroups,
  eventTypes,
}: DressSelectFieldsProps) {
  return (
    <>
      <UnifiedSelector
        label="צבע"
        hasValue={formData.ColorIds.length > 0}
        count={formData.ColorIds.length}
        onClear={() => handleChange("ColorIds", [])}
        required
      >
        <p className="text-xs text-gray-400 mb-2">
          ניתן לבחור יותר מאפשרות אחת
        </p>

        <MultiSelect
          label="צבע"
          options={colors.map((c) => ({ value: c.id, label: c.nameHebrew }))}
          selected={formData.ColorIds}
          onChange={(v) => handleChange("ColorIds", v)}
        />
      </UnifiedSelector>

      <UnifiedSelector
        label="מידה"
        hasValue={formData.SizeIds.length > 0}
        count={formData.SizeIds.length}
        onClear={() => handleChange("SizeIds", [])}
        required
      >
        <p className="text-xs text-gray-400 mb-2">
          ניתן לבחור יותר מאפשרות אחת
        </p>

        <MultiSelect
          label="מידה"
          options={sizes.map((s) => ({ value: s.id, label: s.name }))}
          selected={formData.SizeIds}
          onChange={(v) => handleChange("SizeIds", v)}
        />
      </UnifiedSelector>

      <UnifiedSelector
        label="מיקום"
        hasValue={formData.CityIds.length > 0}
        count={formData.CityIds.length}
        onClear={() => handleChange("CityIds", [])}
        required
      >
        <p className="text-xs text-gray-400 mb-2">
          ניתן לבחור יותר מאפשרות אחת
        </p>

        <MultiSelect
          label="מיקום"
          options={cities.map((c) => ({ value: c.id, label: c.nameHebrew }))}
          selected={formData.CityIds}
          onChange={(v) => handleChange("CityIds", v)}
        />
      </UnifiedSelector>

      <UnifiedSelector
        label="מתאים לגיל"
        hasValue={formData.AgeGroupIds.length > 0}
        count={formData.AgeGroupIds.length}
        onClear={() => handleChange("AgeGroupIds", [])}
        required
      >
        <p className="text-xs text-gray-400 mb-2">
          ניתן לבחור יותר מאפשרות אחת
        </p>

        <MultiSelect
          label="מתאים לגיל"
          options={ageGroups.map((a) => ({ value: a.id, label: a.nameHebrew }))}
          selected={formData.AgeGroupIds}
          onChange={(v) => handleChange("AgeGroupIds", v)}
        />
      </UnifiedSelector>

      <UnifiedSelector
        label="מתאים לאירוע"
        hasValue={formData.EventTypeIds.length > 0}
        count={formData.EventTypeIds.length}
        onClear={() => handleChange("EventTypeIds", [])}
        required
      >
        <p className="text-xs text-gray-400 mb-2">
          ניתן לבחור יותר מאפשרות אחת
        </p>

        <MultiSelect
          label="מתאים לאירוע"
          options={eventTypes.map((e) => ({
            value: e.id,
            label: e.nameHebrew,
          }))}
          selected={formData.EventTypeIds}
          onChange={(v) => handleChange("EventTypeIds", v)}
        />
      </UnifiedSelector>
    </>
  );
}
