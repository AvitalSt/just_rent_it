"use client";

import { useState } from "react";
import { updateCatalog } from "@/services/catalogService";
import { RefreshCcw, CheckCircle } from "lucide-react";
import { ErrorMessage } from "@/components/ui/ErrorMessage";
import { SuccessMessage } from "@/components/ui/SuccessMessage";
import AdminOnly from "../AdminOnly";
import { useAppSelector } from "@/store/hooks";

export default function AdminCatalogUpdateButton() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const user = useAppSelector((state) => state.user.user);
  const isAdmin = user?.role === 2;

  const handleUpdate = async () => {
    if (!isAdmin) return;

    try {
      setError("");
      setSuccess("");
      setLoading(true);

      await updateCatalog();
      setSuccess("הקטלוג עודכן בהצלחה!");
    } catch (err) {
      setError("התרחשה שגיאה בעדכון הקטלוג");
    } finally {
      setLoading(false);
    }
  };

  return (
    <AdminOnly>
      <div
        className="flex-grow flex items-center justify-center bg-gray-100 p-4"
        dir="rtl"
      >
        <div className="bg-white shadow-lg border border-gray-300 rounded-xl p-15 w-full max-w-md">
          <div className="text-center mb-8">
            <h1 className="text-3xl font-bold text-black mb-3">עדכון קטלוג</h1>

            <p className="text-gray-600 text-lg">
              פעולה זו תיצור את הקטלוג מחדש ותשמור אותו במערכת
            </p>
          </div>

          {error && <ErrorMessage message={error} />}
          {success && <SuccessMessage message={success} />}

          <button
            onClick={handleUpdate}
            disabled={loading}
            className="
            w-full bg-black text-white py-4 px-6 text-lg 
            font-semibold hover:bg-gray-800 
            disabled:bg-gray-400 disabled:cursor-not-allowed 
            transition-all duration-200 flex items-center 
            justify-center gap-3 rounded-lg shadow-md
          "
          >
            {loading ? (
              <>
                <div className="w-5 h-5 border-2 border-white border-t-transparent rounded-full animate-spin" />
                <span>מעדכן...</span>
              </>
            ) : (
              <>
                <RefreshCcw className="w-6 h-6" />
                <span>עדכן קטלוג</span>
              </>
            )}
          </button>
        </div>
      </div>
    </AdminOnly>
  );
}
