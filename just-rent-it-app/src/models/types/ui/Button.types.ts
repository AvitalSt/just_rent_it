export interface ButtonProps {
  children: React.ReactNode;//תוכן פנימי של הקומפוננה
  onClick?: () => void;
  variant?: "primary" |"secondary"| "link";
  disabled?: boolean;
  className?: string;
}