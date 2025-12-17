"use client";

import { DressEditImagesProps } from "@/models/types/dress/DressEditImages.types";
import { X } from "lucide-react";

export default function DressEditImages({
  dress,
  removeImages,
  handleRemoveImage,
  selectedMainImage,
  handleSetMain,
  newPreview,
  onAddImages,
  setPreviewImage,
  setNewPreview,
  setNewFiles,
}: DressEditImagesProps) {
  const API_BASE_ORIGIN = process.env.NEXT_PUBLIC_API_BASE_ORIGIN;

  return (
    <div className="space-y-6">
      <h3 className="font-bold mb-2">תמונות קיימות</h3>

      <div className="grid grid-cols-3 gap-4">
        {dress.images
          .filter((img) => !removeImages.includes(img.imageID))
          .map((img) => {
            const full = `${API_BASE_ORIGIN}${img.imagePath}`;
            return (
              <div key={img.imageID} className="relative border rounded p-2">
                <button
                  onClick={() => handleRemoveImage(img)}
                  className="absolute top-1 right-1 text-white p-1 rounded-full shadow-md hover:bg-black"
                >
                  <X size={14} />
                </button>

                <img
                  src={full}
                  className="w-full h-32 object-cover rounded cursor-pointer"
                  onClick={() => setPreviewImage(full)}
                />

                <button
                  onClick={() => handleSetMain(img)}
                  className={`mt-2 w-full py-1 rounded text-sm transition ${
                    selectedMainImage === img.imagePath
                      ? "bg-black text-white border-black"
                      : "bg-white text-black hover:bg-gray-200"
                  }`}
                >
                  {selectedMainImage === img.imagePath
                    ? "תמונה ראשית"
                    : "סמן כראשית"}
                </button>
              </div>
            );
          })}
      </div>

      <h3 className="font-bold mt-4">תמונות חדשות</h3>

      <input
        type="file"
        multiple
        accept="image/*"
        onChange={(e) => onAddImages(e.target.files)}
      />

      <div className="grid grid-cols-3 gap-4 mt-4">
        {newPreview.map((src, i) => (
          <div key={i} className="relative border rounded p-2">
            <button
              onClick={() => {
                setNewPreview((prev) => prev.filter((_, index ) => index  !== i));
                setNewFiles((prev) => prev.filter((_, index ) => index  !== i));
              }}
              className="absolute top-1 right-1 text-white p-1 rounded-full shadow-md hover:bg-black"
            >
              <X size={14} />
            </button>
            <img
              src={src}
              className="w-full h-32 object-cover rounded cursor-pointer"
              onClick={() => setPreviewImage(src)}
            />

            <button
              onClick={() => handleSetMain(src)}
              className={`mt-2 w-full py-1 rounded text-sm transition ${
                selectedMainImage === src
                  ? "bg-black text-white border-black"
                  : "bg-white text-black hover:bg-gray-200"
              }`}
            >
              {selectedMainImage === src ? "תמונה ראשית" : "סמן כראשית"}
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}
