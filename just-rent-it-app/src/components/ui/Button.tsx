import { ButtonProps } from "@/models/types/ui/Button.types";
import React from "react";

export const Button: React.FC<ButtonProps> = ({
  children, // תוכן הכפתור
  onClick, // הפונקציה שתתבצע בלחיצה
  variant = "primary", // סוג הכפתור - כפתור מלא
  disabled = false,
  className = "",
}) => (
  <button
    onClick={onClick}
    disabled={disabled}
    type="button"
    className={`font-semibold rounded-md transition duration-200 
      ${
        variant === "primary"
          ? `w-full py-3 text-white bg-black hover:bg-gray-800 
           disabled:bg-gray-400 disabled:cursor-not-allowed disabled:text-gray-200`
          : `text-blue-600 hover:text-blue-700 font-medium 
           disabled:text-gray-400 disabled:cursor-not-allowed`
      } ${className}`}
  >
    {children}
  </button>
);
