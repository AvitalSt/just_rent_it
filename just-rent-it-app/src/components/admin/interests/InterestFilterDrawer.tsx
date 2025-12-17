"use client";

import { Filter } from "lucide-react";
import InterestFilterPanel from "./InterestFilterPanel";
import { InterestFilterDrawerProps } from "@/models/types/interest/InterestFilterDrawer.type";

export default function InterestFilterDrawer({
  open,
  setOpen,
  draftFilters,
  setDraftFilters,
  applyFilters,
  refresh,
}: InterestFilterDrawerProps) {
  return (
    <>
      <button
        onClick={() => setOpen(true)}
        className="flex items-center gap-2 bg-black text-white px-4 py-2 rounded-lg shadow"
      >
        <Filter className="w-4 h-4" />
        סינון
      </button>

      {open && (
        <div
          className="fixed inset-0 bg-black/40 z-50"
          onClick={() => setOpen(false)}
        >
          <div
            className="absolute right-0 top-0 h-screen w-[370px] bg-white shadow-xl p-6 overflow-y-auto"
            onClick={(e) => e.stopPropagation()}
          >
            <button
              className="text-gray-600 mb-4"
              onClick={() => setOpen(false)}
            >
              ✕ סגור
            </button>

            <InterestFilterPanel
              draftFilters={draftFilters}
              setDraftFilters={setDraftFilters}
              applyFilters={() => {
                applyFilters();
                setOpen(false);
              }}
              resetFilters={() => {
                refresh();
                setOpen(false);
              }}
            />
          </div>
        </div>
      )}
    </>
  );
}
