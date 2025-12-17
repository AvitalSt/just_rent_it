"use client";

import { Button } from "@/components/ui/Button";
import { InterestProps } from "@/models/types/dress/Interest.types";
import Link from "next/link";

export default function Interest({
  dress,
  interestMessage,
  setInterestMessage,
  onSubmit,
  onClose,
  loading,
}: InterestProps) {
  return (
    <div className="bg-gray-50 border border-gray-200 rounded-xl p-5 mt-6 space-y-4">
      <div className="flex justify-between items-start">
        <h2 className="text-xl font-semibold">מתעניינת בשמלה: {dress.name}</h2>

        <button
          onClick={onClose}
          className="text-sm text-gray-500 hover:text-black"
        >
          ✕
        </button>
      </div>

      <p className="text-gray-600 text-sm">
        הודעה עם הפרטים שלך תישלח לבעלת השמלה. ניתן לעדכן את הפרטים בעמוד{" "}
        <Link href="/profile" className="text-blue-600 underline">
          הפרופיל →
        </Link>
      </p>

      <p className="text-gray-600 text-sm">תקבלי למייל הודעה עם פרטי השמלה.</p>

      <label className="text-gray-700 text-sm">
        רוצה להוסיף הודעה? (אופציונלי)
      </label>

      <textarea
        value={interestMessage}
        onChange={(e) => setInterestMessage(e.target.value)}
        rows={3}
        className="w-full border border-gray-300 rounded-lg p-2 text-sm"
        placeholder="הודעה לבעלת השמלה..."
      />

      <div className="bg-yellow-50 border border-yellow-300 text-yellow-800 text-sm p-3 rounded-lg">
        ניתן להתעניין עד <strong>2 שמלות ביום</strong>.
      </div>
      <Button
        onClick={onSubmit}
        disabled={loading}
        variant="primary"
        className="mt-2"
      >
        {loading ? "השליחה מתבצעת..." : "שליחת הודעה לבעלת השמלה"}
      </Button>
    </div>
  );
}
