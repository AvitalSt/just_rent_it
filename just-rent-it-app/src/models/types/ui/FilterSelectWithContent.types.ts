export interface FilterSelectWithContentProps {
  title: string;
  hasValue?: boolean;
  onClear?: () => void;
  children: React.ReactNode;
}