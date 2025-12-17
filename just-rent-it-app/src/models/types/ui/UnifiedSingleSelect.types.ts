interface Option {
  value: number;
  label: string;
}

export interface UnifiedSingleSelectProps {
  label: string;
  value: number | null;
  options: Option[];
  onChange: (v: number) => void;
  required?: boolean;
}