"use client";

import { Heart } from "lucide-react";
import { useRouter } from "next/navigation";

export default function WishlistEmpty() {
  const router = useRouter();

    return (
    <div className="min-h-screen bg-white flex items-center justify-center p-4">
      <div className="max-w-md w-full text-center" dir="rtl">
        <div className="flex justify-center mb-8">
          <div className="relative">
            <div className="w-40 h-40 rounded-full bg-gray-50 flex items-center justify-center">
              <Heart className="w-20 h-20 text-gray-300 stroke-[1.5]" />
            </div>

            <div className="absolute -top-2 -right-2 w-6 h-6 bg-black rounded-full opacity-10"></div>
            <div className="absolute -bottom-3 -left-3 w-8 h-8 bg-black rounded-full opacity-5"></div>
          </div>
        </div>

        <h1 className="text-3xl font-bold text-gray-900 mb-3">
          רשימת המשאלות שלך
        </h1>

        <p className="text-xl text-gray-400 mb-6 font-light">
          This wishlist is empty.
        </p>

        <p className="text-gray-600 leading-relaxed mb-8">
          עדיין אין לך שמלות מועדפות.  
          תוכלי למצוא שמלות יפות בעמוד השמלות שלנו.
        </p>

        <button
          onClick={() => router.push("/dresses")}
          className="w-full max-w-xs mx-auto py-3.5 px-8 bg-black text-white rounded-lg font-semibold hover:bg-gray-800 transition-colors duration-200"
        >
          חזור לרשימת השמלות
        </button>
      </div>
    </div>
  );
}