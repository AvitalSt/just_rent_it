"use client";

import { useState } from "react";
import { ChevronRight, ChevronLeft, Eye } from "lucide-react";

export default function Gallery({
  images,
  views,
}: {
  images: string[];
  views: number;
}) {
  const [current, setCurrent] = useState(0);

  const next = () => {
    if (current === images.length - 1) {
      setCurrent(0);
    } else {
      setCurrent(current + 1);
    }
  };

  const prev = () => {
    if (current === 0) {
      setCurrent(images.length - 1);
    } else {
      setCurrent(current - 1);
    }
  };

  return (
    <div className="relative w-full">
      <div className="absolute top-2 right-2 flex items-center gap-1 bg-black/40 text-white px-2 py-1 rounded-md text-xs z-20 backdrop-blur-sm">
        <Eye className="w-4 h-4" />
        <span>{views}</span>
      </div>

      <div className="relative aspect-[3/4] bg-gray-100 rounded-lg overflow-hidden shadow-sm">
        <img src={images[current]} className="w-full h-full object-cover" />

        {images.length > 1 && (
          <>
            <button
              onClick={prev}
              className="absolute left-2 top-1/2 -translate-y-1/2 bg-white/80 hover:bg-white rounded-full p-2 transition shadow"
            >
              <ChevronLeft className="w-6 h-6 text-gray-700" />
            </button>

            <button
              onClick={next}
              className="absolute right-2 top-1/2 -translate-y-1/2 bg-white/80 hover:bg-white rounded-full p-2 transition shadow"
            >
              <ChevronRight className="w-6 h-6 text-gray-700" />
            </button>

            <div className="absolute bottom-4 left-1/2 -translate-x-1/2 flex gap-2">
              {images.map((_, idx) => (
                <div
                  key={idx}
                  className={`w-2 h-2 rounded-full transition ${
                    idx === current ? "bg-white" : "bg-white/50"
                  }`}
                />
              ))}
            </div>
          </>
        )}
      </div>
    </div>
  );
}
