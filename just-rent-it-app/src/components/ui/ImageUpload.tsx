"use client";

import { useRef } from "react";
import { X, Check } from "lucide-react";
import { ImageUploadProps } from "@/models/types/ui/ImageUpload.types";

export default function ImageUpload({
  previews,
  setPreviews,
  files,
  setFiles,
  mainIndex,
  setMainIndex,
  maxImages = 4,
  required = false,
}: ImageUploadProps) {
  const inputRef = useRef<HTMLInputElement | null>(null);

  const pickFiles = () => inputRef.current?.click();

  const handleFiles = (fileList: FileList | null) => {
    if (!fileList) return;

    const allowed = maxImages - previews.length;
    const selectedFiles = Array.from(fileList).slice(0, allowed);

    const newPreviews: string[] = [];
    const newFiles: File[] = [...files];

    selectedFiles.forEach((file, idx) => {
      newFiles.push(file);

      const reader = new FileReader();
      reader.onload = (e) => {
        if (e.target?.result) {
          newPreviews.push(e.target.result as string);

          if (newPreviews.length === selectedFiles.length) {
            setPreviews([...previews, ...newPreviews]);
            setFiles(newFiles);
          }
        }
      };
      reader.readAsDataURL(file);
    });
  };

  const removeImage = (index: number) => {
    const updatedPreviews = previews.filter((_, i) => i !== index);
    const updatedFiles = files.filter((_, i) => i !== index);

    setPreviews(updatedPreviews);
    setFiles(updatedFiles);

    if (mainIndex === index) setMainIndex(null);
    else if (mainIndex !== null && mainIndex > index)
      setMainIndex(mainIndex - 1);
  };

  return (
    <div className="flex flex-col gap-3">
      <label className="block text-sm font-medium text-gray-700 text-right">
        תמונות {required && <span className="text-red-500">*</span>}
      </label>
      <p className="text-xs text-gray-500">
        אין להעלות תמונות הכוללות פרטי זיהוי כגון טלפון או כתובת מייל.{" "}
      </p>
      <div
        onClick={pickFiles}
        className="border-2 border-dashed border-gray-300 rounded-xl p-6 text-center cursor-pointer hover:bg-gray-50 transition"
      >
        <p className="text-sm text-gray-600">
          <span className="font-semibold">לחצי כדי לבחור תמונות</span>
          <br />
          ניתן להעלות עד {maxImages} תמונות
        </p>
      </div>

      {previews.length > 0 && (
        <div className="grid grid-cols-2 sm:grid-cols-3 gap-3">
          {previews.map((src, idx) => (
            <div
              key={idx}
              className={`relative border rounded-lg overflow-hidden shadow-sm aspect-square ${
                mainIndex === idx
                  ? "bg-black text-white border-black"
                  : "bg-white text-black hover:bg-gray-200"
              }`}
            >
              <img src={src} className="w-full h-full object-cover" />

              <button
                type="button"
                onClick={() => removeImage(idx)}
                className="absolute top-1 right-1 bg-black/70 text-white rounded-full p-1 hover:bg-black transition"
              >
                <X size={14} />
              </button>

              <button
                type="button"
                onClick={() => setMainIndex(idx)}
                className={`absolute bottom-1 right-1 flex items-center gap-1 px-2 py-1 rounded-md text-xs ${
                  mainIndex === idx
                    ? "bg-black text-white border-black"
                    : "bg-white text-black hover:bg-gray-200"
                }`}
              >
                <Check size={12} />
                {mainIndex === idx ? "תמונה ראשית" : "קבע כראשית"}
              </button>
            </div>
          ))}
        </div>
      )}

      <input
        ref={inputRef}
        type="file"
        accept="image/*"
        multiple
        className="hidden"
        onChange={(e) => handleFiles(e.target.files)}
      />
    </div>
  );
}
