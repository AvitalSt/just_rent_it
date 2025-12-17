"use client";

import LogoutButton from "./LogoutButton";
import ProfileDetailsCard from "./ProfileDetailsCard";
import ProfileHeader from "./ProfileHeader";

export default function UserProfile() {
  return (
    <div className="min-h-screen bg-gray-50 py-10" dir="rtl">
      <div className="max-w-2xl mx-auto px-4 flex flex-col gap-6">
        
        <ProfileHeader />

        <ProfileDetailsCard />

        <LogoutButton />

      </div>
    </div>
  );
}
