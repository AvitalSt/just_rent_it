"use client";

import { useAppSelector } from "@/store/hooks";
import AdminOnly from "../AdminOnly";
import SendMonthlySummaryButton from "./SendMonthlySummaryButton";
import { getLastMonthlySummary } from "@/services/monthlySummaryService";
import { useEffect, useState } from "react";

export default function AdminMonthlySummaryPage() {
  const [lastSent, setLastSent] = useState<string | null>(null);
  const [reload, setReload] = useState(0);

  const user = useAppSelector((state) => state.user.user);
  const isAdmin = user?.role === 2;

  useEffect(() => {
    if (!isAdmin) return;

    async function load() {
      try {
        const data = await getLastMonthlySummary();
        setLastSent(data?.lastSent);
      } catch {
        setLastSent(null);
      }
    }
    load();
  }, [reload]);

  return (
    <AdminOnly>
      <div
        className="min-h-[85vh] bg-gray-100 flex justify-center py-10 px-4"
        dir="rtl"
      >
        <div className="bg-white w-full max-w-3xl shadow-md rounded-xl p-8 text-center">
          <h1 className="text-2xl font-bold mb-6 text-gray-800">
            שליחת סיכום חודשי לכל משתמשי האתר
          </h1>

          <p className="text-gray-600 mb-4">
            {lastSent
              ? `נשלח לאחרונה: ${new Date(lastSent).toLocaleDateString(
                  "he-IL"
                )}`
              : "עדיין לא נשלח אף סיכום חודשי"}
          </p>

          <div className="mb-8 flex justify-center">
            <SendMonthlySummaryButton
              onSuccess={() => setReload((r) => r + 1)}
            />
          </div>
        </div>
      </div>
    </AdminOnly>
  );
}
