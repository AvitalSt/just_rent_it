export interface ImageUploadProps {
  previews: string[];
  setPreviews: (val: string[]) => void;

  files: File[];
  setFiles: (val: File[]) => void;

  mainIndex: number | null;
  setMainIndex: (val: number | null) => void;

  maxImages?: number;
  required?: boolean;
}
