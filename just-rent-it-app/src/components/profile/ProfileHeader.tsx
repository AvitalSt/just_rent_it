import { User } from "lucide-react";

export default function ProfileHeader() {
  return (
    <div className="text-center mb-2">
      <div className="inline-flex items-center justify-center w-20 h-20 bg-gray-900 rounded-full mb-3">
        <User className="w-12 h-12 text-white" />
      </div>

      <h1 className="text-2xl font-light text-gray-900 mb-1">הפרופיל שלי</h1>
      <p className="text-gray-600 text-sm">נהל את הפרטים האישיים שלך</p>
    </div>
  );
}
