"use client";

import { Button } from "@/components/ui/Button";
import { CheckboxField } from "@/components/ui/CheckboxField";
import { ErrorMessage } from "@/components/ui/ErrorMessage";
import { InputField } from "@/components/ui/InputField";
import { PasswordInput } from "@/components/ui/PasswordInput";
import { RegisterDTO } from "@/models/DTOs/RegisterDTO";
import { register } from "@/services/authService";
import { setUser, setWishlistDressIds } from "@/store/userSlice";
import { handleSuccessfulAuth } from "@/utils/authHelpers";
import axios from "axios";
import { useRouter } from "next/navigation";
import { useState } from "react";
import { useDispatch } from "react-redux";

export default function Register() {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    phone: "",
    rememberMe: false,
  });
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const router = useRouter();
  const dispatch = useDispatch();

  const handleChange = (field: string, value: string | boolean) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
    setError("");
  };

  const handleSubmit = async () => {
    if (loading) return;
    setError("");

    if (
      !formData.firstName ||
      !formData.lastName ||
      !formData.email ||
      !formData.password ||
      !formData.phone
    ) {
      setError("אנא מלאי את כל השדות הנדרשים.");
      return;
    }

    const data: RegisterDTO = {
      FirstName: formData.firstName,
      LastName: formData.lastName,
      Email: formData.email,
      Password: formData.password,
      Phone: formData.phone,
      RememberMe: formData.rememberMe,
    };

    setLoading(true);
    try {
      const res = await register(data);
      await handleSuccessfulAuth({ dispatch, router, user: res.user, token: res.token});
    } catch (err) {
      if (axios.isAxiosError(err)) {
        const message =
          err.response?.data?.message || "אירעה שגיאה ברישום. אנא נסי שוב.";
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
      className="min-h-screen flex items-center justify-center bg-gray-50 p-4"
      dir="rtl"
    >
      <div className="w-full max-w-md">
        {error && <ErrorMessage message={error} />}
        <div className="bg-white rounded-lg shadow-md p-8">
          <h1 className="text-3xl font-bold text-center mb-8">הרשמה</h1>

          <div className="space-y-5">
            <div className="grid grid-cols-2 gap-3">
              <InputField
                label="שם פרטי"
                value={formData.firstName}
                onChange={(e) => handleChange("firstName", e.target.value)}
                required
              />
              <InputField
                label="שם משפחה"
                value={formData.lastName}
                onChange={(e) => handleChange("lastName", e.target.value)}
                required
              />
            </div>

            <InputField
              label="אימייל"
              type="email"
              value={formData.email}
              onChange={(e) => handleChange("email", e.target.value)}
              required
            />

            <PasswordInput
              label="סיסמה"
              value={formData.password}
              onChange={(e) => handleChange("password", e.target.value)}
              required
            />

            <InputField
              label="טלפון"
              value={formData.phone}
              onChange={(e) => handleChange("phone", e.target.value)}
              required
            />

            <CheckboxField
              id="remember"
              label="זכור אותי"
              checked={formData.rememberMe}
              onChange={(checked) => handleChange("rememberMe", checked)}
            />

            <Button onClick={handleSubmit} variant="primary" disabled={loading}>
              הירשם
            </Button>

            <div className="mt-6 text-center">
              <p className="text-gray-600 text-sm">
                כבר יש לך חשבון?{" "}
                <Button onClick={() => router.push("/login")} variant="link">
                  התחברי
                </Button>
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}