"use client";

import { useEffect, useRef } from "react";
import noUiSlider from "nouislider";
import "nouislider/dist/nouislider.css";

interface PriceFilterProps {
  min: number;
  max: number;
  maxRange: number;
  onChange: (v: { min: number; max: number }) => void;
}

export default function PriceFilter({
  min,
  max,
  maxRange,
  onChange,
}: PriceFilterProps) {
  const sliderRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    const slider = sliderRef.current;
    if (!slider) return;

    const api = (slider as any).noUiSlider;

    // אם כבר קיים – רק עדכון
    if (api) {
      api.updateOptions(
        {
          start: [min, max],
          range: { min: 0, max: maxRange },
        },
        true
      );
      return;
    }

    // יצירת סליידר
    noUiSlider.create(slider, {
      start: [min, max],
      connect: true,
      direction: "rtl",
      range: {
        min: 0,
        max: maxRange,
      },
      step: 1,
    });

    const newApi = (slider as any).noUiSlider;

    newApi.on("update", (values: string[]) => {
      onChange({
        min: Number(values[0]),
        max: Number(values[1]),
      });
    });

    return () => newApi.destroy();
  }, [min, max, maxRange]);

  return (
    <div dir="rtl" className="space-y-4">
      <div className="flex justify-between text-black font-semibold text-base">
        <span>₪{min.toLocaleString()}</span>
        <span>₪{max.toLocaleString()}</span>
      </div>

      <div ref={sliderRef} className="mt-4" />

      <p className="text-xs text-gray-500 text-center">
        ₪0 - ₪{maxRange.toLocaleString()}
      </p>
    </div>
  );
}
