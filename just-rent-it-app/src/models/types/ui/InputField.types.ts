import { Icon } from "next/dist/lib/metadata/types/metadata-types";

export type InputFieldProps = {
  label: string;
  type?: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  onKeyPress?: (e: React.KeyboardEvent<HTMLInputElement>) => void;
  required?: boolean;
  placeholder?: string;
  className?: string;
};
