import { AddDressDTO } from "@/models/DTOs/AddDressDTO";
import { DressFormChangeHandler } from "./DressForm.types";

export interface DressImageFieldsProps {
  formData: AddDressDTO;
  handleChange: DressFormChangeHandler;

  files: File[];
  setFiles: (val: File[]) => void;

  previewImages: string[];
  setPreviewImages: (val: string[]) => void;
}
