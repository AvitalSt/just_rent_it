"use client";

import { Filter } from "lucide-react";
import FilterPanel from "../Filter/FilterPanel";
import { MobileFilterDrawerProps } from "@/models/types/dress/MobileFilterDrawer.types";

export default function MobileFilterDrawer({
  mobileOpen,
  setMobileOpen,
  draftFilters,
  setDraftFilters,
  applyFilters,
}: MobileFilterDrawerProps) {
  return (
    <>
    {/* lg:hidden הכפתור מוצג במסכים קטנים */}
      <div className="lg:hidden flex justify-end mb-4">
        <button
          onClick={() => setMobileOpen(true)}
          className="flex items-center gap-2 bg-black text-white px-4 py-2 rounded-lg shadow"
        >
          <Filter className="w-4 h-4" />
          סינון
        </button>
      </div>

      {mobileOpen && (
        <div
          className="fixed inset-0 bg-black/40 z-40 lg:hidden"
          onClick={() => setMobileOpen(false)}
        >
          <div
            className="absolute right-0 top-0 h-screen w-80 bg-white shadow-xl p-4 overflow-y-auto"
            //אל תעביר את הקליק להורה שלי — תשאיר אותו כאן
            onClick={(e) => e.stopPropagation()}
          >
            <button
              className="text-gray-600 mb-4"
              onClick={() => setMobileOpen(false)}
            >
              ✕ סגור
            </button>

            <FilterPanel
              draftFilters={draftFilters}
              setDraftFilters={setDraftFilters}
              applyFilters={(v) => {
                applyFilters(v);
                setMobileOpen(false);
              }}
            />
          </div>
        </div>
      )}
    </>
  );
}