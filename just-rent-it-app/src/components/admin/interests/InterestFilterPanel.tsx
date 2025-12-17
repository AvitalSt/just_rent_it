"use client";

import { RotateCcw, Filter } from "lucide-react";
import RentStatusFilter from "@/components/dresses/DressFilter/Filter/RentStatusFilter";
import { InterestFilterPanelProps } from "@/models/types/interest/InterestFilterPanel.type";
import { InputField } from "@/components/ui/InputField";

export default function InterestFilterPanel({
  draftFilters,
  setDraftFilters,
  applyFilters,
  resetFilters,
}: InterestFilterPanelProps) {
  return (
    <aside className="bg-white shadow p-4 rounded-lg w-80 space-y-5" dir="rtl">
      <div className="flex items-center gap-2 border-b pb-3">
        <Filter className="w-5 h-5 text-gray-700" />
        <h2 className="font-bold text-lg">סינון התעניינויות</h2>
      </div>

      <div className="flex gap-2">
        <button
          onClick={applyFilters}
          className="flex-1 bg-black text-white py-2 rounded-lg font-semibold"
        >
          החל סינון
        </button>

        <button onClick={resetFilters} className="p-2 bg-gray-100 rounded-lg">
          <RotateCcw className="w-5 h-5" />
        </button>
      </div>

      <RentStatusFilter
        value={draftFilters.status}
        onChange={(v) => setDraftFilters({ ...draftFilters, status: v })}
      />

      <InputField
        label="שם בעלת שמלה"
        value={draftFilters.ownerName}
        onChange={(e) =>
          setDraftFilters({ ...draftFilters, ownerName: e.target.value })
        }
      />
      <InputField
        label="שם מתעניינת"
        value={draftFilters.userName}
        onChange={(e) =>
          setDraftFilters({ ...draftFilters, userName: e.target.value })
        }
      />

      <InputField
        label="שם שמלה"
        value={draftFilters.dressName}
        onChange={(e) =>
          setDraftFilters({ ...draftFilters, dressName: e.target.value })
        }
      />

      <InputField
        label="מתאריך"
        type="date"
        value={draftFilters.from}
        onChange={(e) =>
          setDraftFilters({ ...draftFilters, from: e.target.value })
        }
      />

      <InputField
        label="עד תאריך"
        type="date"
        value={draftFilters.to}
        onChange={(e) =>
          setDraftFilters({ ...draftFilters, to: e.target.value })
        }
      />
    </aside>
  );
}
