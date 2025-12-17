"use client";

import { useState } from "react";
import { InputField } from "@/components/ui/InputField";
import { ErrorMessage } from "@/components/ui/ErrorMessage";
import { sendPasswordResetEmail } from "@/services/userService";
import { SuccessMessage } from "../ui/SuccessMessage";
import { Button } from "../ui/Button";
import axios from "axios";

export default function ForgotPassword() {
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async () => {
    if (loading) return;

    setError("");
    setSuccess("");

    if (!email) return setError("יש להזין כתובת אימייל");

    setLoading(true);
    try {
      await sendPasswordResetEmail(email);
      setSuccess("אימייל לאיפוס סיסמה נשלח.");
      setEmail("");
    } catch (err) {
      if (axios.isAxiosError(err)) {
        const message =
          err.response?.data?.message ||
          "אירעה שגיאה בשליחת המייל. אנא נסי שוב.";
        setError(message);
      } else {
        setError("אירעה שגיאה בלתי צפויה. אנא נסי שוב מאוחר יותר.");
      }
    } finally {
      setLoading(false);
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") handleSubmit();
  };

  return (
    <div
      className="min-h-screen bg-gray-50 flex items-center justify-center p-4"
      dir="rtl"
    >
      <div className="w-full max-w-md">
        {success ? (
          <div>
            <SuccessMessage message={`אימייל לאיפוס סיסמה נשלח. `} />
            <h3>ייתכן שייקח מספר דקות עד שיופיע בתיבת הדואר הנכנס.</h3>
          </div>
        ) : (
          <div className="bg-white rounded-lg shadow-md p-8">
            {error && <ErrorMessage message={error} />}
            <h1 className="text-2xl font-bold text-gray-800 mb-6 text-center">
              שכחת את הסיסמה?
            </h1>
            <p className="text-gray-600 mb-6 text-center">
              יש להזין את כתובת האימייל שלך. הוראות איפוס הסיסמה יישלחו באימייל.
            </p>

            <div className="space-y-4">
              <InputField
                label="כתובת אימייל"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                onKeyPress={handleKeyPress}
                required
              />

              <Button
                onClick={handleSubmit}
                variant="primary"
                disabled={loading}
              >
                שלח הוראות איפוס
              </Button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
