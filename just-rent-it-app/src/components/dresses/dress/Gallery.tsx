"use client";

import { useState } from "react";
import { ChevronRight, ChevronLeft, Eye } from "lucide-react";
import ImageModal from "@/components/ui/ImageModal";

export default function Gallery({
  images,
  views,
}: {
  images: string[];
  views: number;
}) {
  const [current, setCurrent] = useState(0);
  const [modalOpen, setModalOpen] = useState(false);

  const next = () =>
    setCurrent((prev) => (prev === images.length - 1 ? 0 : prev + 1));
  const prev = () =>
    setCurrent((prev) => (prev === 0 ? images.length - 1 : prev - 1));

  return (
    <div className="relative w-full">
      <div className="absolute top-2 right-2 flex items-center gap-1 bg-black/40 text-white px-2 py-1 rounded-md text-xs z-20 backdrop-blur-sm">
        <Eye className="w-4 h-4" />
        <span>{views}</span>
      </div>

      <div className="relative aspect-[3/4] bg-gray-100 rounded-lg overflow-hidden shadow-sm">
        <img
          src={images[current]}
          className="w-full h-full object-cover cursor-pointer"
          onClick={() => setModalOpen(true)} // לחיצה פותחת את המודאל
        />
      </div>

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
        </>
      )}

      {modalOpen && (
        <ImageModal
          images={images}
          startIndex={current} // מתחילים מהתמונה הנוכחית
          onClose={() => setModalOpen(false)}
        />
      )}
    </div>
  );
}
