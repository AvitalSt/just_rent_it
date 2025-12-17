import { SuccessMessageProps } from "@/models/types/ui/SuccessMessage.types";

export function SuccessMessage({ message }: SuccessMessageProps) {
  return (
    <div className="mb-4 bg-green-100 border-r-4 border-green-500 p-4 rounded shadow-sm">
      <div className="flex items-start">
        <div className="flex-shrink-0 ml-3">
          <svg
            className="h-5 w-5 text-green-600"
            viewBox="0 0 20 20"
            fill="currentColor"
          >
            <path
              fillRule="evenodd"
              d="M10 18a8 8 0 100-16 8 8 0 000 16zm3.707-9.707a1 1 0 00-1.414-1.414L9 10.586 7.707 9.293a1 1 0 00-1.414 1.414l2 2a1 1 0 001.414 0l4-4z"
              clipRule="evenodd"
            />
          </svg>
        </div>
        <div className="flex-1">
          <p className="text-sm text-gray-800">{message}</p>
        </div>
      </div>
    </div>
  );
}
