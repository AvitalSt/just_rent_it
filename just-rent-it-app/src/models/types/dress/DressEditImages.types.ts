import { DressDTO } from "@/models/DTOs/DressDTO";
import { DressImageDTO } from "@/models/DTOs/ImageDTO";

export interface DressEditImagesProps {
  dress: DressDTO;
  removeImages: number[];
  handleRemoveImage: (img: DressImageDTO) => void;
  selectedMainImage: string;
  handleSetMain: (img: DressImageDTO | string) => void;
  newPreview: string[];
  onAddImages: (files: FileList | null) => void;
  setPreviewImage: (src: string | null) => void;
  setNewPreview: (value: string[] | ((prev: string[]) => string[])) => void;
  setNewFiles: (value: File[] | ((prev: File[]) => File[])) => void;
}
