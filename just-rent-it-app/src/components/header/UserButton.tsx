"use client";

import React, { useState } from "react";
import { User } from "lucide-react";
import { useAppSelector } from "@/store/hooks";
import Link from "next/link";

export default function UserButton() {
  const [isHovered, setIsHovered] = useState(false);

  const user = useAppSelector((state) => state.user.user);

  if (user) {
    return (
    <Link href="/profile">
      <button
        className="p-2.5 bg-white border border-gray-200 rounded-full hover:bg-gray-50 transition-all duration-300"
      >
        <User className="w-5 h-5 text-gray-800 stroke-[1.5]" />
      </button>
    </Link>
  );
  }

  return (
    <div className="relative">
      <button
        className="flex items-center gap-2 bg-white border border-gray-200 rounded-full hover:bg-gray-50 transition-all duration-300 overflow-hidden"
        onMouseEnter={() => setIsHovered(true)}
        onMouseLeave={() => {
          setIsHovered(false);
        }}
      >
        <div
          className={`flex items-center gap-2 transition-all duration-300 ${
            isHovered ? "px-4 py-2.5" : "p-2.5"
          }`}
        >
          <User className="w-5 h-5 text-gray-800 stroke-[1.5]" />
          {isHovered && (
            <div className="flex items-center gap-1 text-sm font-light whitespace-nowrap">
              <span
                className="text-gray-800 cursor-pointer hover:underline"
                onClick={(e) => {
                  e.stopPropagation(); // כשלוחצים פה באופן רגיל האירוע עולה להורה ..
                  //זה עוצר ולא יפעיל תא האירוע שיש בהורה
                  //"stopPropagation() מונע מהאירוע להתפשט להורה שלו. אני משתמשת בזה כאשר יש אלמנט פנימי שמבצע פעולה משלו, ואני לא רוצה שהלחיצה עליו תפעיל גם את ה־onClick של ההורה
                }}
              >
                <Link href="/login">התחברות</Link>
              </span>

              <span className="mx-1 text-gray-400">/</span>
              <span
                className="text-gray-800 cursor-pointer hover:underline"
                onClick={(e) => {
                  e.stopPropagation();
                }}
              >
                <Link href="/register">הירשמות</Link>
              </span>
            </div>
          )}
        </div>
      </button>
    </div>
  );
}
