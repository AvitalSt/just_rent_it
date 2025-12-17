import { CheckboxFieldProps } from "@/models/types/ui/CheckboxField.types";

export function CheckboxField({
  label,
  checked,
  onChange,
  id,
  required = false,
}: CheckboxFieldProps) {
  return (
    <div className="flex items-center mb-4">
      <input
        type="checkbox"
        id={id}
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
        className="w-4 h-4 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
      />

      <label
        htmlFor={id}
        className="mr-2 text-sm text-gray-700 cursor-pointer"
      >
        {label} {required && <span className="text-red-500">*</span>}
      </label>
    </div>
  );
}
