interface Option {
  value: number;
  label: string;
}

export interface UnifiedMultiSelectProps {
  label: string;
  options: Option[];
  value: number[];
  onChange: (v: number[]) => void;
  required?: boolean;
}