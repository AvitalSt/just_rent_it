"use client";

import { LogOut } from "lucide-react";
import { useState } from "react";
import ConfirmModal from "@/components/ui/ConfirmModal";
import { useAppDispatch } from "@/store/hooks";
import { clearUser } from "@/store/userSlice";
import { useRouter } from "next/navigation";

export default function LogoutButton() {
  const [showModal, setShowModal] = useState(false);
  const dispatch = useAppDispatch();
  const router = useRouter();

  const handleLogout = () => {
    dispatch(clearUser());
    localStorage.removeItem("token");
    router.push("/");
  };

  return (
    <div className="bg-white rounded-2xl shadow-sm border border-gray-200 p-4">
      <button
        onClick={() => setShowModal(true)}
        className="w-full flex items-center justify-center gap-3 px-4 py-3 bg-white text-black border border-gray-300 rounded-lg hover:bg-gray-100 transition-colors font-medium"
      >
        <LogOut className="w-5 h-5 text-black" />
        <span>התנתק מהחשבון</span>
      </button>

      {showModal && (
        <ConfirmModal
          message="האם ברצונך להתנתק מהאתר?"
          onConfirm={() => {
            setShowModal(false);
            handleLogout();
          }}
          onCancel={() => setShowModal(false)}
        />
      )}
    </div>
  );
}
