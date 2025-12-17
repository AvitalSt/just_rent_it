"use client";

import { useEffect, useState } from "react";
import { Edit2, Check, X } from "lucide-react";
import { useAppDispatch, useAppSelector } from "@/store/hooks";
import { setUser } from "@/store/userSlice";
import { updateUser } from "@/services/userService";
import { useRouter } from "next/navigation";
import { InputField } from "../ui/InputField";
import { Button } from "../ui/Button";
import { ErrorMessage } from "../ui/ErrorMessage";
import Loading from "../ui/Loading";

export default function ProfileDetailsCard() {
  const router = useRouter();
  const dispatch = useAppDispatch();
  const user = useAppSelector((state) => state.user.user);

  const [isEditing, setIsEditing] = useState(false);
  const [editData, setEditData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    phone: "",
  });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) router.push("/login");
  }, []);

  if (!user) return <Loading/>

  const handleSave = async () => {
    if (loading) return;

    setError("");
    setLoading(true);

    try {
      const updatedUser = await updateUser(user.userID, editData);
      dispatch(setUser({ user: updatedUser }));
      setIsEditing(false);
    } catch (err) {
      setError("אירעה שגיאה בעדכון הפרטים");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div dir="rtl">
      <div className="max-w-4xl mx-auto px-4">
        <div className="bg-white rounded-2xl shadow-sm border p-5 mb-4">
          {error && <ErrorMessage message={error} />}

          <div className="flex items-center justify-between mb-6">
            <h2 className="text-xl font-light text-gray-900">פרטים אישיים</h2>

            {!isEditing ? (
              <Button
                onClick={() => {
                  setEditData({
                    firstName: user.firstName,
                    lastName: user.lastName,
                    email: user.email,
                    phone: user.phone,
                  });
                  setIsEditing(true);
                }}
                className="!w-auto px-4 py-2 flex items-center gap-2 rounded-full !bg-gray-900 text-white"
              >
                <Edit2 className="w-4 h-4" /> עריכה
              </Button>
            ) : (
              <div className="flex gap-2 mb-4">
                <Button
                  onClick={handleSave}
                  disabled={loading}
                  className="!w-auto px-4 py-2 flex items-center gap-2 rounded-full"
                >
                  <Check className="w-4 h-4" />
                  שמירה
                </Button>

                <Button
                  variant="secondary"
                  onClick={() => {
                    setEditData({
                      firstName: user.firstName,
                      lastName: user.lastName,
                      email: user.email,
                      phone: user.phone,
                    });
                    setIsEditing(false);
                  }}
                  className="!w-auto px-4 py-2 flex items-center gap-2 rounded-full !bg-gray-100 !text-gray-700"
                >
                  <X className="w-4 h-4" /> ביטול
                </Button>
              </div>
            )}
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-4">
            <div>
              {!isEditing ? (
                <>
                  <label className="text-sm text-gray-700 block mb-1">
                    שם פרטי
                  </label>
                  <div className="px-4 py-3 bg-gray-50 rounded-lg">
                    {user.firstName}
                  </div>
                </>
              ) : (
                <InputField
                  label="שם פרטי"
                  value={editData.firstName}
                  onChange={(e) =>
                    setEditData({ ...editData, firstName: e.target.value })
                  }
                />
              )}
            </div>

            <div>
              {!isEditing ? (
                <>
                  <label className="text-sm text-gray-700 block mb-1">
                    שם משפחה
                  </label>
                  <div className="px-4 py-3 bg-gray-50 rounded-lg">
                    {user.lastName}
                  </div>
                </>
              ) : (
                <InputField
                  label="שם משפחה"
                  value={editData.lastName}
                  onChange={(e) =>
                    setEditData({ ...editData, lastName: e.target.value })
                  }
                />
              )}
            </div>

            <div>
              {!isEditing ? (
                <>
                  <label className="text-sm text-gray-700 block mb-1">
                    אימייל
                  </label>
                  <div className="px-4 py-3 bg-gray-50 rounded-lg">
                    {user.email}
                  </div>
                </>
              ) : (
                <InputField
                  label="אימייל"
                  type="email"
                  value={editData.email}
                  onChange={(e) =>
                    setEditData({ ...editData, email: e.target.value })
                  }
                />
              )}
            </div>

            <div>
              {!isEditing ? (
                <>
                  <label className="text-sm text-gray-700 block mb-1">
                    טלפון
                  </label>
                  <div className="px-4 py-3 bg-gray-50 rounded-lg">
                    {user.phone}
                  </div>
                </>
              ) : (
                <InputField
                  label="טלפון"
                  value={editData.phone}
                  onChange={(e) =>
                    setEditData({ ...editData, phone: e.target.value })
                  }
                />
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
