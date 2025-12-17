"use client";

import { Button } from "@/components/ui/Button";
import { ErrorMessage } from "@/components/ui/ErrorMessage";
import { SuccessMessage } from "@/components/ui/SuccessMessage";
import { sendMonthlySummary } from "@/services/monthlySummaryService";
import { useState } from "react";

export default function SendMonthlySummaryButton({ onSuccess }: { onSuccess: () => void }) {
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  const handleSendSummary = async () => {
    setLoading(true);
    setSuccess("");
    setError("");
    try {
      await sendMonthlySummary();
      setSuccess("נשלח בהצלחה");
      onSuccess();
    } catch {
      setError("תקלה בשליחה");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="my-6">
      {error && <ErrorMessage message={error} />}
      {success && <SuccessMessage message={success} />}
      <Button onClick={handleSendSummary} disabled={loading} variant="primary">
        שליחת הודעה חודשית לכל משתמשי האתר
      </Button>
    </div>
  );
}
