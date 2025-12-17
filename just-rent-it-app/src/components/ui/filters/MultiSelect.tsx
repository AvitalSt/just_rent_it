"use client";

import React, { useState } from "react";
import { X, ChevronDown, ChevronUp } from "lucide-react";
import { MultiSelectProps } from "@/models/types/dress/MultiSelect.types";

export function MultiSelect<T extends string | number>({
  label,
  options,
  selected,
  onChange,
  multiple = true,
  required = false
}: MultiSelectProps<T>) {
  const [isOpen, setIsOpen] = useState(false);

  const toggleOption = (option: T) => {
    if (multiple) {
      if (selected.includes(option)) {
        onChange(selected.filter((o) => o !== option));
      } else {
        onChange([...selected, option]);
      }
    } else {
      onChange([option]);
      setIsOpen(false);
    }
  };

  const selectedItems = options.filter((o) => selected.includes(o.value));

  return (
    <div>
      {/* Selected items */}
      {selectedItems.length > 0 && (
        <div className="mb-3">
          <div className="flex flex-wrap gap-1.5">
            {selectedItems.map((item) => (
              <div
                key={item.value}
                className="bg-black text-white px-2 py-0.5 rounded-full text-xs flex items-center gap-1"
              >
                <button
                  type="button"
                  onClick={() => toggleOption(item.value)}
                >
                  <X size={12} />
                </button>
                <span>{item.label}</span>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Options */}
      <div className="bg-white max-h-48 overflow-y-auto border rounded p-2">
        {options.map((opt) => (
          <button
            key={opt.value}
            type="button"
            className={`w-full text-right px-3 py-2 rounded-md text-sm border mb-1 
              ${selected.includes(opt.value)
                ? "bg-black text-white border-black"
                : "bg-white text-gray-700 border-gray-300 hover:border-black"
              }`}
            onClick={() => toggleOption(opt.value)}
          >
            {opt.label}
          </button>
        ))}
      </div>
    </div>
  );
}
