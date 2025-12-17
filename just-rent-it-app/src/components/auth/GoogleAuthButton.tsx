"use client";

import dynamic from "next/dynamic";
import { CredentialResponse } from "@react-oauth/google";
import { loginGoogle } from "@/services/authService";
import axios from "axios";
import { useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import { ErrorMessage } from "../ui/ErrorMessage";
import { useDispatch } from "react-redux";
import { handleSuccessfulAuth } from "@/utils/authHelpers";

// טעינה של GoogleLogin רק בצד לקוח
const GoogleLogin = dynamic(
  () => import("@react-oauth/google").then((mod) => mod.GoogleLogin),
  { ssr: false }
);

export default function GoogleAuthButton({
  rememberMe,
}: {
  rememberMe: boolean;
}) {
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const router = useRouter();
  const dispatch = useDispatch();

  const handleGoogleLogin = async (cred: CredentialResponse) => {
    if (loading) return;

    setError("");
    if (!cred.credential) {
      setError("לא התקבל אישור מגוגל. נסי שוב.");
      return;
    }

    setLoading(true);
    try {
      const idToken = cred.credential;
      const res = await loginGoogle(idToken, rememberMe);

      await handleSuccessfulAuth({
        dispatch,
        router,
        user: res.user,
        token: res.token,
      });
    } catch (err) {
      if (axios.isAxiosError(err)) {
        setError(err.response?.data?.message || "שגיאה בהתחברות עם Google.");
      } else {
        setError("שגיאה בלתי צפויה.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex flex-col items-center">
      {error && (
        <div className="w-full max-w-sm mb-3">
          <ErrorMessage message={error} />
        </div>
      )}

      <GoogleLogin
        theme="outline"
        size="large"
        text="signin_with"
        shape="rectangular"
        width="350"
        onSuccess={handleGoogleLogin}
        onError={() => setError("התחברות גוגל נכשלה")}
      />
    </div>
  );
}
