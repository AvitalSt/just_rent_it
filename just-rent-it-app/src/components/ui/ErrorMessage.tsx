import { ErrorMessageProps } from "@/models/types/ui/ErrorMessage.types";

export function ErrorMessage({ message }: ErrorMessageProps) {
  return (
    <div className="mb-4 bg-yellow-100 border-r-4 border-yellow-500 p-4 rounded shadow-sm">
      <div className="flex items-start">
        <div className="flex-shrink-0 ml-3">
          <svg
            className="h-5 w-5 text-yellow-600"
            viewBox="0 0 20 20"
            fill="currentColor"
          >
            <circle
              cx="10"
              cy="10"
              r="9"
              stroke="currentColor"
              strokeWidth="2"
              fill="none"
            />
            <path
              d="M10 6v5M10 13v1"
              stroke="currentColor"
              strokeWidth="2"
              strokeLinecap="round"
            />
          </svg>
        </div>

        <div className="flex-1">
          <p className="text-sm text-gray-800">
            <strong>שגיאה:</strong> {message}
          </p>
        </div>
      </div>
    </div>
  );
}
