"use client";

import { Filter, RotateCcw } from "lucide-react";

import CityFilter from "./CityFilter";
import ColorFilter from "./ColorFilter";
import SizeFilter from "./SizeFilter";
import AgeGroupFilter from "./AgeGroupFilter";
import EventTypeFilter from "./EventTypeFilter";
import SaleTypeFilter from "./SaleTypeFilter";
import StateGroupFilter from "./StateGroupFilter";
import StatusGroupFilter from "./StatusGroupFilter";

import { FilterPanelProps } from "@/models/types/dress/DressFilter.types";
import { useAppSelector } from "@/store/hooks";

export default function FilterPanel({
  draftFilters,
  setDraftFilters,
  applyFilters,
}: FilterPanelProps) {
  const user = useAppSelector((s) => s.user.user);
  const isAdmin = user?.role === 2;


  const resetFilters = () => {
    const reset = {
      cityIds: [],
      sizeIds: [],
      colorIds: [],
      ageGroupIds: [],
      eventTypeIds: [],
      saleTypeIds: [],
      stateGroupIds: [],
      statusGroupIds: [],
      orderBy: "",
    };
    setDraftFilters(reset);
    applyFilters(reset);
  };

  return (
    <aside
      className="bg-white shadow p-4 rounded-lg space-y-2 flex flex-col"
      dir="rtl"
    >
      <div className="bg-gradient-to-l from-gray-50 to-white px-5 py-4 border-b border-gray-200">
        <div className="flex items-center gap-2">
          <Filter className="w-5 h-5 text-gray-700" />
          <h2 className="font-bold text-lg text-gray-900">סינון תוצאות</h2>
        </div>
      </div>

      <div className="px-5 py-4 bg-gray-50 border-b border-gray-200">
        <div className="flex gap-2">
          <button
            onClick={() => applyFilters()}
            className="flex-1 bg-black text-white py-3 rounded-lg font-semibold hover:bg-gray-900 active:scale-98 transition-all duration-200 shadow-sm"
          >
            החל סינון
          </button>

          <button
            onClick={resetFilters}
            className="flex items-center justify-center px-4 py-3 rounded-lg font-semibold transition duration-200"
          >
            <RotateCcw className="w-4 h-4" />
          </button>
        </div>
      </div>

      <CityFilter
        value={draftFilters.cityIds}
        onChange={(v) => setDraftFilters((p) => ({ ...p, cityIds: v }))}
      />

      <ColorFilter
        value={draftFilters.colorIds}
        onChange={(v) => setDraftFilters((p) => ({ ...p, colorIds: v }))}
      />

      <SizeFilter
        value={draftFilters.sizeIds}
        onChange={(v) => setDraftFilters((p) => ({ ...p, sizeIds: v }))}
      />

      <AgeGroupFilter
        value={draftFilters.ageGroupIds}
        onChange={(v) => setDraftFilters((p) => ({ ...p, ageGroupIds: v }))}
      />

      <EventTypeFilter
        value={draftFilters.eventTypeIds}
        onChange={(v) => setDraftFilters((p) => ({ ...p, eventTypeIds: v }))}
      />

      <SaleTypeFilter
        value={draftFilters.saleTypeIds}
        onChange={(v) => setDraftFilters((p) => ({ ...p, saleTypeIds: v }))}
      />

      <StateGroupFilter
        value={draftFilters.stateGroupIds}
        onChange={(v) => setDraftFilters((p) => ({ ...p, stateGroupIds: v }))}
      />

      {isAdmin && (
        <StatusGroupFilter
          value={draftFilters.statusGroupIds}
          onChange={(v) =>
            setDraftFilters((p) => ({ ...p, statusGroupIds: v }))
          }
        />
      )}
    </aside>
  );
}