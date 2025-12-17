"use client";

import { useState } from "react";
import { downloadCatalog } from "@/services/catalogService";
import { Download, FileText, CheckCircle } from "lucide-react";
import { ErrorMessage } from "@/components/ui/ErrorMessage";
import { SuccessMessage } from "@/components/ui/SuccessMessage";

export default function CatalogDownloadButton() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);

  const handleDownload = async () => {
    try {
      setError("");
      setSuccess(false);
      setLoading(true);
      const blob = await downloadCatalog();

      //Binary Large Object
      //יוצר URL זמני בזכרון הדפדפן שמצביע על קובץ Blob
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement("a");
      //הקישור מצביע על ה-URL של הקובץ
      link.href = url;
      //מגדיר כשאר לוחצים על הקישור במקום לנווט לכתובת URL צריך להוריד את התכון בשם ..
      link.download = "catalog-just-rent-it.pdf";
      //מבצע את הלחיצה מאוחרי הקלעים
      //   window.open(url)
      document.body.appendChild(link);
      link.click();
      link.remove();
      setSuccess(true);

      window.URL.revokeObjectURL(url);
    } catch (err) {
      console.error(err);
      setError("התרחשה שגיאה בהורדת הקטלוג");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      className="flex-grow flex items-center justify-center bg-gray-100 p-4"
      dir="rtl"
    >
      <div className="bg-white shadow-lg border border-gray-300 rounded-xl p-15 w-full max-w-md">
        <div className="text-center mb-8">
          <div className="inline-flex items-center justify-center w-20 h-20 bg-black rounded-full mb-6 shadow-md">
            <FileText className="w-10 h-10 text-white" />
          </div>
          <h1 className="text-3xl font-bold text-black mb-3">הורדת קטלוג</h1>
          <p className="text-gray-600 text-lg">
            הורדי את הקטלוג המלא של JustRentIt בלחיצה אחת
          </p>
        </div>

        {error && <ErrorMessage message={error} />}
        {success && <SuccessMessage message="הקטלוג הורד בהצלחה!" />}

        <button
          onClick={handleDownload}
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
              <span>מוריד קטלוג...</span>
            </>
          ) : (
            <>
              <Download className="w-6 h-6" />
              <span>הורד קטלוג</span>
            </>
          )}
        </button>
      </div>
    </div>
  );
}
