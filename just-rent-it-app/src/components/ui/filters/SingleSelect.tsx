"use client";

interface SingleSelectProps<T extends string | number> {
  options: { value: T; label: string }[];
  selected: T | null;
  onChange: (value: T) => void;
}

export function SingleSelect<T extends string | number>({
  options,
  selected,
  onChange,
}: SingleSelectProps<T>) {
  return (
    <div className="bg-white max-h-48 overflow-y-auto border rounded p-2">
      {options.map((opt) => (
        <button
          key={opt.value}
          type="button"
          className={`w-full text-right px-3 py-2 rounded-md text-sm border mb-1
            ${
              selected === opt.value
                ? "bg-black text-white border-black"
                : "bg-white text-gray-700 border-gray-300 hover:border-black"
            }`}
          onClick={() => onChange(opt.value)}
        >
          {opt.label}
        </button>
      ))}
    </div>
  );
}
