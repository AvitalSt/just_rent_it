"use client";

import { useState } from "react";
import { getDressById } from "@/services/dressService";
import DressView from "./DressView";
import { DressDTO } from "@/models/DTOs/DressDTO";

interface DressPageProps {
  initialDress: DressDTO;
}
export default function DressPage({ initialDress }:DressPageProps) {
  const [dress, setDress] = useState(initialDress);

  const reloadDress = async () => {
    const updated = await getDressById(initialDress.dressID);
    setDress(updated);
  };

  return (
    <div className="min-h-screen bg-gray-50 p-4 md:p-8" dir="rtl">
      <div className="max-w-6xl mx-auto bg-white rounded-lg shadow-lg p-6 md:p-8">
        <DressView dress={dress} reload={reloadDress} />
      </div>
    </div>
  );
}
