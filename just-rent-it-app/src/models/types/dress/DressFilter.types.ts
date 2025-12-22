import { ReadonlyURLSearchParams } from "next/navigation";

export interface DressFilters {
  cityIds: number[];
  sizeIds: number[];
  colorIds: number[];
  ageGroupIds: number[];
  eventTypeIds: number[];
  saleTypeIds: number[];
  stateGroupIds: number[];
  statusGroupIds: number[];
  orderBy: string;
}

export interface FilterPanelProps {
  draftFilters: DressFilters;
  setDraftFilters: React.Dispatch<React.SetStateAction<DressFilters>>;
  applyFilters: (override?: DressFilters) => void;
}

export function parseUrlToFilters(
  params: ReadonlyURLSearchParams,
  fallback: DressFilters,
): DressFilters {
  const getList = (key: string): number[] => {
    const val = params.get(key);
    return val ? val.split("_").map(Number) : [];
  };

  return {
    ...fallback,
    cityIds: getList("city"),
    sizeIds: getList("sizeGroup"),
    colorIds: getList("colorGroup"),
    ageGroupIds: getList("ageGroup"),
    eventTypeIds: getList("eventType"),
    saleTypeIds: getList("saleType"),
    stateGroupIds: getList("stateGroup"),
    statusGroupIds: getList("statusGroup"),
    orderBy: params.get("orderBy") ?? "",
  };
}
