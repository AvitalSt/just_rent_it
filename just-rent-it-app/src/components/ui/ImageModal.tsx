"use client";

import { useState } from "react";
import { ChevronRight, ChevronLeft, X } from "lucide-react";

interface ImageModalProps {
  images: string[];
  startIndex?: number; // אינדקס התמונה שמתחילים ממנו
  onClose: () => void;
}

export default function ImageModal({
  images,
  startIndex = 0,
  onClose,
}: ImageModalProps) {
  const [current, setCurrent] = useState(startIndex);

  const next = () =>
    setCurrent((prev) => (prev === images.length - 1 ? 0 : prev + 1));
  const prev = () =>
    setCurrent((prev) => (prev === 0 ? images.length - 1 : prev - 1));

  return (
    <div
      className="fixed inset-0 bg-black bg-opacity-70 flex justify-center items-center z-50"
      onClick={onClose}
    >
      <button
        onClick={(e) => {
          e.stopPropagation();
          onClose();
        }}
        className="absolute top-4 right-4 text-white p-2 rounded-full bg-black/50 hover:bg-black z-50"
      >
        <X size={24} />
      </button>

      {/* התמונה הגדולה */}
      <img
        src={images[current]}
        className="max-h-[90%] max-w-[90%] rounded shadow-lg"
      />

      {images.length > 1 && (
        <>
          <button
            onClick={(e) => {
              e.stopPropagation();
              prev();
            }}
            className="absolute left-4 top-1/2 -translate-y-1/2 bg-white/80 hover:bg-white rounded-full p-2 shadow z-50"
          >
            <ChevronLeft className="w-6 h-6 text-gray-700" />
          </button>

          <button
            onClick={(e) => {
              e.stopPropagation();
              next();
            }}
            className="absolute right-4 top-1/2 -translate-y-1/2 bg-white/80 hover:bg-white rounded-full p-2 shadow z-50"
          >
            <ChevronRight className="w-6 h-6 text-gray-700" />
          </button>
        </>
      )}
    </div>
  );
}
