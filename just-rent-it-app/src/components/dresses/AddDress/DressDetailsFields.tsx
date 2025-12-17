import { InputField } from "@/components/ui/InputField";
import { DressDetailsProps } from "@/models/types/dress/DressDetails.types";

export default function DressDetailsFields({ formData, handleChange }: DressDetailsProps) {
  return (
    <>
      <div className="grid grid-cols-5 gap-2 mb-1">
        <div className="col-span-5 md:col-span-3">
          <InputField
            label="שם השמלה"
            value={formData.Name}
            onChange={(e) => handleChange("Name", e.target.value)}
            required
          />
        </div>

        <div className="col-span-5 md:col-span-2">
          <InputField
            label="מחיר"
            type="number"
            value={formData.Price?.toString() ?? ""}
            onChange={(e) => handleChange("Price", e.target.value === "" ? null : Number(e.target.value))}
            required
          />
        </div>
      </div>

      <InputField
        label="תיאור"
        value={formData.Description}
        onChange={(e) => handleChange("Description", e.target.value)}
        className="py-1.5 px-3 text-sm"
      />
    </>
  );
}
