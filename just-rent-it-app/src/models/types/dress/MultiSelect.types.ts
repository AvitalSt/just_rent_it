export interface MultiSelectProps<T> {
  label: string;
  options: { value: T; label: string }[];
  selected: T[];
  onChange: (selected: T[]) => void;
  multiple?: boolean; 
  required?:boolean;
}