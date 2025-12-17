interface Option {
  value: number | string;
  label: string;
}

export interface SingleSelectProps {
  label: string;
  value: number | string | null;
  options: Option[];
  onChange: (value: number | string) => void;
  required?: boolean;
}