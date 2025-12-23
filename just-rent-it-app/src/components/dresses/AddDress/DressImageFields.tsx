"use client";

import ImageUpload from "@/components/ui/ImageUpload";
import { DressImageFieldsProps } from "@/models/types/dress/DressImageFields.types";

export default function DressImageFields({
  formData,
  handleChange,
  files,
  setFiles,
  previewImages,
  setPreviewImages,
}: DressImageFieldsProps) {
  const mainIdx = previewImages.findIndex((x) => x === formData.MainImage);

  return (
    <div className="space-y-2">
      
      <ImageUpload
        previews={previewImages}
        setPreviews={setPreviewImages}
        files={files}
        setFiles={setFiles}
        mainIndex={mainIdx >= 0 ? mainIdx : null}
        setMainIndex={(idx) =>
          handleChange("MainImage", idx !== null ? previewImages[idx] : "")
        }
        maxImages={4}
        required
      />
    </div>
  );
}
