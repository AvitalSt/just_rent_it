"use client";

import { useState } from "react";
import { X } from "lucide-react";

export default function Contact({ onClose }: { onClose: () => void }) {
  const [notes, setNotes] = useState("");

  const send = () => {
    if (!notes.trim()) {
      alert("נא להוסיף הודעה");
      return;
    }
    alert("הודעה נשלחה!");
    setNotes("");
    onClose();
  };

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-lg max-w-md w-full p-6 relative">
        <button onClick={onClose} className="absolute top-4 left-4 text-gray-400 hover:text-gray-600">
          <X className="w-6 h-6" />
        </button>

        <h2 className="text-2xl font-bold mb-2">שליחת הודעה לבעלת השמלה</h2>
        <p className="text-sm text-gray-600 mb-6">ההודעה שלך תישלח לבעלת השמלה</p>

        <textarea
          rows={4}
          value={notes}
          onChange={(e) => setNotes(e.target.value)}
          placeholder="כתבי הודעה..."
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-gray-900 resize-none"
        />

        <button
          onClick={send}
          className="w-full mt-4 bg-black hover:bg-gray-800 text-white font-medium py-3 px-4 rounded-lg transition"
        >
          שלחי הודעה
        </button>
      </div>
    </div>
  );
}
