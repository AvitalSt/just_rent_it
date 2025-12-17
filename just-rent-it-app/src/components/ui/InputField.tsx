import { InputFieldProps } from "@/models/types/ui/InputField.types";

export function InputField({
  label,
  type = "text",
  value,
  onChange,
  onKeyPress,
  required = false,
  placeholder = "",
  className = "",
}: InputFieldProps) {
  return (
    <div>
      <label className="block text-sm text-gray-700 mb-1.5 text-right">
        {label} {required && <span className="text-red-500">*</span>}
      </label>

      <div className="relative">
        <input
          type={type}
          value={value}
          onChange={onChange}
          onKeyPress={onKeyPress}
          placeholder={placeholder}
          className={`w-full px-4 py-3 border border-gray-300 rounded-md 
            focus:outline-none focus:ring-2 focus:ring-blue-500 
            focus:border-transparent transition text-right direction-rtl 
            ${className}`}
        />
      </div>
    </div>
  );
}
