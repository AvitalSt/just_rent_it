"use client";

import { Button } from "@/components/ui/Button";
import { ErrorMessage } from "@/components/ui/ErrorMessage";
import { SuccessMessage } from "@/components/ui/SuccessMessage";
import { sendMonthlySummary } from "@/services/monthlySummaryService";
import { useState } from "react";

export default function SendMonthlySummaryButton({
  onSuccess,
  daysToSplit,
  dayIndex,
  disabled,
}: {
  onSuccess: () => void;
  daysToSplit: number;
  dayIndex: number;
  disabled?: boolean;
}) {
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  const handleSendSummary = async () => {
    setLoading(true);
    setSuccess("");
    setError("");
    try {
      await sendMonthlySummary(daysToSplit, dayIndex);
      setSuccess(`נשלח יום ${dayIndex}/${daysToSplit} בהצלחה`);
      onSuccess();
    } catch {
      setError("תקלה בשליחה");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-w-[170px]">
      {error && <ErrorMessage message={error} />}
      {success && <SuccessMessage message={success} />}
      <Button onClick={handleSendSummary} disabled={loading || !!disabled} variant="primary">
        {loading ? "שולח..." : `שליחה יום ${dayIndex}/${daysToSplit}`}
      </Button>
    </div>
  );
}
