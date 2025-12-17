"use client";

import { useRouter } from "next/navigation";
import { useState } from "react";
import { login } from "@/services/authService";
import { ErrorMessage } from "@/components/ui/ErrorMessage";
import { InputField } from "@/components/ui/InputField";
import { PasswordInput } from "@/components/ui/PasswordInput";
import { CheckboxField } from "@/components/ui/CheckboxField";
import GoogleAuthButton from "@/components/auth/GoogleAuthButton";
import { Button } from "@/components/ui/Button";
import axios from "axios";
import { useDispatch } from "react-redux";
import { handleSuccessfulAuth } from "@/utils/authHelpers";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const router = useRouter();
  const dispatch = useDispatch();

  const handleSubmit = async () => {
    if (loading) return;
    setError("");

    if (!email && !password) return setError("נדרש שם משתמש וסיסמה");
    if (!email) return setError("נדרש שם משתמש");
    if (!password) return setError("לא הוזנה סיסמה");

    setLoading(true);
    try {
      const res = await login(email, password, rememberMe);

      await handleSuccessfulAuth({
        dispatch,
        router,
        user: res.user,
        token: res.token,
      });
    } catch (err) {
      if (axios.isAxiosError(err)) {
        const message =
          err.response?.data?.message ||
          `הסיסמה או כתובת האימייל ${email} אינה נכונה. שחזור סיסמה`;
        setError(message);
      } else {
        setError("אירעה שגיאה בלתי צפויה. אנא נסי שוב מאוחר יותר.");
      }
    } finally {
      setLoading(false);
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") {
      handleSubmit();
    }
  };

  return (
    <div
      className="min-h-screen bg-gray-50 flex items-center justify-center p-4"
      dir="rtl"
    >
      <div className="w-full max-w-md">
        {error && <ErrorMessage message={error} />}

        <div className="bg-white rounded-lg shadow-md p-8">
          <h1 className="text-3xl font-bold text-gray-800 mb-8 text-center">
            התחברות
          </h1>

          <div className="space-y-6">
            <InputField
              label="כתובת אימייל"
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              onKeyPress={handleKeyPress}
              required
            />

            <PasswordInput
              label="סיסמה"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              onKeyPress={handleKeyPress}
              required
            />

            <Button onClick={handleSubmit} variant="primary" disabled={loading}>
              התחברות
            </Button>

            <GoogleAuthButton rememberMe={rememberMe} />

            <div className="flex items-center mt-4">
              <CheckboxField
                id="remember"
                label="זכור אותי"
                checked={rememberMe}
                onChange={setRememberMe}
              />
              <Button
                onClick={() => router.push("/forgot-password")}
                variant="link"
                className="mr-auto text-sm text-blue-600 hover:underline"
              >
                שכחת את הסיסמה שלך?
              </Button>
            </div>

            <div className="mt-6 text-center">
              <p className="text-gray-600 text-sm">
                אין לך חשבון?{" "}
                <Button onClick={() => router.push("/register")} variant="link">
                  הירשמי
                </Button>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
