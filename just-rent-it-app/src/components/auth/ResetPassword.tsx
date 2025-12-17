"use client";

import { useState } from "react";
import { useRouter, usePathname } from "next/navigation";
import { ErrorMessage } from "@/components/ui/ErrorMessage";
import { resetPassword } from "@/services/userService";
import { SuccessMessage } from "@/components/ui/SuccessMessage";
import { PasswordInput } from "@/components/ui/PasswordInput";
import { Button } from "@/components/ui/Button";
import axios from "axios";
import { useDispatch } from "react-redux";
import {  setUser, setWishlistDressIds } from "@/store/userSlice";

export default function ResetPassword() {
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

  const router = useRouter();
  const pathname = usePathname();
  const token = pathname.split("/").pop() || "";
  const dispatch = useDispatch();
  const handleSubmit = async () => {
    setError("");
    setSuccess("");

    if (!newPassword || !confirmPassword) {
      return setError("יש למלא את כל השדות");
    }
    if (newPassword !== confirmPassword) {
      return setError("הסיסמאות לא תואמות");
    }

    setLoading(true);
    try {
      const res = await resetPassword(token, newPassword);
      localStorage.setItem("token", res.token);
      dispatch(setUser({ user: res.user }));
      setSuccess("הסיסמה עודכנה בהצלחה!");
      setTimeout(() => router.push("/"), 2000);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        const message =
          err.response?.data?.message ||
          "אירעה שגיאה בעדכון הסיסמה. אנא נסי שוב.";
        setError(message);
      } else {
        setError("אירעה שגיאה בלתי צפויה. אנא נסי שוב מאוחר יותר.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div
      className="min-h-screen bg-gray-50 flex items-center justify-center p-4"
      dir="rtl"
    >
      <div className="w-full max-w-md">
        {(!token || error) && !success && (
          <ErrorMessage
            message={!token ? "קישור לא תקין לאיפוס סיסמה" : error}
          />
        )}

        {success && <SuccessMessage message={success} />}

        {token && !success && (
          <div className="bg-white rounded-lg shadow-md p-8">
            <h1 className="text-2xl font-bold text-gray-800 mb-6 text-center">
              עדכון סיסמה
            </h1>

            <div className="space-y-4">
              <PasswordInput
                label="סיסמה חדשה"
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                required
              />
              <PasswordInput
                label="אימות סיסמה"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                required
              />

              <Button
                onClick={handleSubmit}
                disabled={loading}
                variant="primary"
              >
                עדכן סיסמה
              </Button>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}
