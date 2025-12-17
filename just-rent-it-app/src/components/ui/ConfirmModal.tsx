"use client";

import { ConfirmModalProps } from "@/models/types/ui/ConfirmModal.types";
import { X } from "lucide-react";

export default function ConfirmModal({
  message,
  onConfirm,
  onCancel,
}: ConfirmModalProps) {
  const showButtons = !!onConfirm;
  return (
    <div className="fixed inset-0 bg-black/40 flex items-center justify-center z-[999]">
      <div className="bg-white w-full max-w-md rounded-3xl p-8 relative">
        <button
          className="absolute top-4 left-4 text-gray-500 hover:text-black"
          onClick={onCancel}
        >
          <X className="w-6 h-6" />
        </button>

        <p className="text-center text-lg text-gray-900 mb-8 font-light">
          {message}
        </p>

        {showButtons && (
          <div className="flex items-center justify-center gap-4">
            {onCancel && (
              <button
                onClick={onCancel}
                className="px-8 py-3 rounded-full bg-black text-white hover:bg-gray-800 transition"
              >
                ביטול
              </button>
            )}

            {onConfirm && (
              <button
                onClick={onConfirm}
                className="px-8 py-3 rounded-full border border-gray-400 text-gray-800 hover:bg-gray-100 transition"
              >
                אישור
              </button>
            )}
          </div>
        )}
      </div>
    </div>
  );
}
