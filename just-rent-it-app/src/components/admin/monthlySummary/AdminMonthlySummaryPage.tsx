"use client";

import { useAppSelector } from "@/store/hooks";
import AdminOnly from "../AdminOnly";
import SendMonthlySummaryButton from "./SendMonthlySummaryButton";
import {
  getLastMonthlySummary,
  previewMonthlySummary,
} from "@/services/monthlySummaryService";
import { useEffect, useState } from "react";
import { MonthlySummaryPreviewDTO } from "@/models/DTOs/MonthlySummaryPreviewDTO";
import {
  getMonthKey,
  loadProgress,
  saveProgress,
  MonthlyProgress,
} from "@/utils/monthlySummaryProgress";

export default function AdminMonthlySummaryPage() {
  const [lastSent, setLastSent] = useState<string | null>(null);
  const [preview, setPreview] = useState<MonthlySummaryPreviewDTO | null>(null);
  const [daysToSplit, setDaysToSplit] = useState(2);
  const [reload, setReload] = useState(0);
  const [progressKey, setProgressKey] = useState<string | null>(null);
  const [progress, setProgress] = useState<MonthlyProgress | null>(null);

  const user = useAppSelector((state) => state.user.user);
  const isAdmin = user?.role === 2;

  useEffect(() => {
    if (!isAdmin) return;

    async function load() {
      try {
        const last = await getLastMonthlySummary();
        setLastSent(last?.lastSent ?? null);
      } catch {
        setLastSent(null);
      }

      try {
        const p = await previewMonthlySummary();
        setPreview(p);

        const key = getMonthKey(p.fromUtc);
        setProgressKey(key);
        const saved = loadProgress(key);
        if (saved) {
          setDaysToSplit(saved.daysToSplit);
          setProgress(saved);
        } else {
          setProgress(null);
        }
      } catch {
        setPreview(null);
        setProgressKey(null);
        setProgress(null);
      }
    }
    load();
  }, [reload,isAdmin]);

  const monthLabel = preview
    ? new Date(preview.fromUtc).toLocaleDateString("he-IL", {
        month: "2-digit",
        year: "numeric",
      })
    : null;

  const lastSentDay = progress?.lastSentDay ?? 0;
  const activeDay = lastSentDay >= daysToSplit ? -1 : lastSentDay + 1;

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

          <div className="bg-gray-50 border rounded-lg p-4 mb-6 text-right">
            {preview ? (
              <>
                <div className="font-semibold mb-2">
                  תצוגה מקדימה לחודש: {monthLabel}
                </div>
                <div>מיילים לבעלות שמלות: {preview.ownerEmails}</div>
                <div>מיילים למתעניינות: {preview.userEmails}</div>
                <div className="font-semibold mt-2">
                  סה״כ מיילים: {preview.totalEmails}
                </div>
              </>
            ) : (
              <div className="text-gray-600">
                לא ניתן לטעון תצוגה מקדימה כרגע.
              </div>
            )}
          </div>

          <div className="flex items-center justify-center gap-3 mb-4">
            <span className="text-gray-700">לחלק לכמה ימים?</span>
            <select
              className="border rounded px-3 py-2"
              value={daysToSplit}
              onChange={(e) => setDaysToSplit(Number(e.target.value))}
            >
              {[1, 2, 3, 4, 5, 6, 7].map((n) => (
                <option key={n} value={n}>
                  {n}
                </option>
              ))}
            </select>
          </div>

          <div className="mb-8 flex flex-wrap justify-center gap-3">
            {Array.from({ length: daysToSplit }, (_, i) => i + 1).map((day) => {
              const disabled = activeDay === -1 ? true : day !== activeDay;

              return (
                <SendMonthlySummaryButton
                  key={day}
                  daysToSplit={daysToSplit}
                  dayIndex={day}
                  disabled={disabled}
                  onSuccess={() => {
                    if (progressKey) {
                      const newProg: MonthlyProgress = {
                        daysToSplit,
                        lastSentDay: day,
                      };
                      saveProgress(progressKey, newProg);
                      setProgress(newProg);
                    }
                    setReload((r) => r + 1);
                  }}
                />
              );
            })}
          </div>
        </div>
      </div>
    </AdminOnly>
  );
}
