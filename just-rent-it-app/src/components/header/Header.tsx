"use client";

import { Menu, Heart } from "lucide-react";
import IconButton from "./IconButton";
import Logo from "./Logo";
import UserButton from "./UserButton";
import Link from "next/link";
import { useState } from "react";
import SidebarMenu from "./SidebarMenu";

export default function Header() {
  const [open, setOpen] = useState(false);

  return (
    <header className="bg-white border-b border-gray-100" dir="rtl">
      <div className="max-w-9xl mx-auto px-8">
        <div className="flex items-center justify-between h-24">
          <IconButton onClick={() => setOpen(true)}>
            <Menu className="w-6 h-6 text-gray-800 stroke-[1.5]" />
          </IconButton>

          <Logo />

          <div className="flex items-center gap-4">
            <Link href="/wish-list">
              <IconButton>
                <Heart className="w-5 h-5 text-gray-800 stroke-[1.5]" />
              </IconButton>
            </Link>
            <UserButton />
          </div>
        </div>
      </div>

      <SidebarMenu isOpen={open} onClose={() => setOpen(false)} />
    </header>
  );
}
