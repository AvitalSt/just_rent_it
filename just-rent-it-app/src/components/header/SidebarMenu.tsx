"use client";

import Link from "next/link";
import { X, LogOut, LogIn } from "lucide-react";
import { SidebarMenuProps } from "@/models/types/header/SidebarMenu.types";
import { useAppSelector } from "@/store/hooks";
import { useRouter } from "next/navigation";

export default function SidebarMenu({ isOpen, onClose }: SidebarMenuProps) {
  const user = useAppSelector((state) => state.user.user);
  const isLoggedIn = !!user;
  const isAdmin = user?.role === 2;

  const router = useRouter();

  const menuItems = [
    { href: "/", label: "דף הבית" },
    { href: "/dresses", label: "כל השמלות" },
    { href: "/add-dress", label: "העלאת שמלה" },
    { href: "/catalog", label: "הורדת קטלוג" },
    { href: "/wish-list", label: "המועדפים שלי" },
    ...(isLoggedIn ? [{ href: "/profile", label: "הפרופיל שלי" }] : []),
    { href: "/site-policy", label: "תקנון האתר" },
  ];
  if (isAdmin) {
    menuItems.push(
      { href: "/interest", label: "ניהול התעניינויות" },
      { href: "/monthly-summary", label: "שליחת סיכום חודשי" },
      { href: "/catalog-update", label: "עדכון קטלוג" }
    );
  }

  return (
    <>
      {/* מחשיך רקע מסביב  */}
      <div
        className={`
          fixed inset-0 bg-black z-40 transition-opacity duration-300
          ${isOpen ? "opacity-50" : "opacity-0 pointer-events-none"}
        `}
        onClick={onClose}
      />

      {/* הגדרות של התפריט */}
      <div
        className={`
          fixed top-0 right-0 h-full w-80 bg-white z-50
          transform transition-all duration-300 ease-in-out
          ${
            isOpen ? "translate-x-0 shadow-2xl" : "translate-x-full shadow-none"
          }
        `}
        dir="rtl"
      >
        <div className="relative border-b border-gray-100 px-6 py-6">
          <button
            onClick={onClose}
            className="absolute top-6 left-6 p-2 rounded-full hover:bg-gray-100 transition-colors duration-200"
            aria-label="סגור תפריט"
          >
            <X className="w-5 h-5 text-gray-700" />
          </button>

          <div className="pr-2">
            <h2 className="text-2xl font-bold text-black">תפריט</h2>
            <p className="text-sm text-gray-500 mt-1">JustRentIt</p>
          </div>
        </div>

        <nav className="flex flex-col py-4 px-3">
          {menuItems.map((item) => {
            return (
              <Link
                key={item.href}
                href={item.href}
                onClick={onClose}
                className="
                  flex items-center px-4 py-3.5 rounded-lg
                  text-gray-700 font-medium text-base
                  hover:bg-gray-50 hover:text-black
                  transition-all duration-200
                "
              >
                <span>{item.label}</span>
              </Link>
            );
          })}
        </nav>

        {/* קו מפריד */}
        <div className="mx-6 my-2">
          <div className="h-px bg-gradient-to-l from-gray-200 via-gray-300 to-gray-200" />
        </div>

        <div className="px-3 py-2">
          {isLoggedIn ? (
            <button
              onClick={() => {
                localStorage.removeItem("token");
                onClose();
                router.push("/");
              }}
              className="
                w-full flex items-center gap-4 px-4 py-3.5 rounded-lg
                text-black font-medium text-base
                hover:bg-gray-100
                transition-all duration-200
                group
            "
            >
              <LogOut className="w-5 h-5 group-hover:translate-x-1 transition-transform duration-200" />
              <span>יציאה מהחשבון</span>
            </button>
          ) : (
            <Link
              href="/login"
              onClick={onClose}
              className="
                w-full flex items-center gap-4 px-4 py-3.5 rounded-lg
                bg-black text-white font-medium text-base
                hover:bg-gray-800
                transition-all duration-200
                group
              "
            >
              <LogIn className="w-5 h-5 group-hover:translate-x-1 transition-transform duration-200" />
              <span>התחברות / הרשמה</span>
            </Link>
          )}
        </div>

        <div className="absolute bottom-0 left-0 right-0 px-6 py-4 border-t border-gray-100">
          {isAdmin && (
            <div className="mb-3 px-3 py-2 bg-black rounded-lg">
              <p className="text-xs text-white text-center font-medium">
                מצב מנהל פעיל
              </p>
            </div>
          )}
          <p className="text-xs text-gray-400 text-center">
            © 2025 JustRentIt. כל הזכויות שמורות.
          </p>
        </div>
      </div>
    </>
  );
}
