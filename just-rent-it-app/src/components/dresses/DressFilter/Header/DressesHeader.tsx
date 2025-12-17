"use client";

import { OrderByMap } from "@/models/Enums/filtersMap";
import UnifiedSelectString from "@/components/ui/filters/UnifiedSelectString";
import ListHeaderContainer from "@/components/ui/layout/ListHeaderContainer";
import { DressesHeaderProps } from "@/models/types/dress/DressesHeader.type";

export default function DressesHeader({
  loading,
  totalResults,
  draftFilters,
  setDraftFilters,
  applyFilters,
}: DressesHeaderProps) {
  return (
    <div className="bg-white rounded-xl shadow-sm border border-gray-100 p-4 mb-6">
      <div className="flex items-center justify-between">

        <ListHeaderContainer
          title="שמלות להשכרה ולמכירה"
          total={totalResults}
          loading={loading}
        />

        <div className="w-48">
          <UnifiedSelectString
            label="מיון"
            value={draftFilters.orderBy || null}
            onChange={(v) => {
              const updated = { ...draftFilters, orderBy: v };
              setDraftFilters(updated);
              applyFilters(updated);
            }}
            options={Object.keys(OrderByMap).map((key) => ({
              value: key,
              label: OrderByMap[key],
            }))}
          />
        </div>
      </div>
    </div>
  );
}
