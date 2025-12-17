interface Option {
  value: string;
  label: string;
}

export interface UnifiedSelectStringProps {
  label: string;
  value: string | null;
  options: Option[];
  onChange: (v: string) => void;
  required?: boolean;
}