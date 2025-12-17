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
  priceMin: number;
  priceMax: number;
  orderBy: string;
}

export interface FilterPanelProps {
  draftFilters: DressFilters;
  setDraftFilters: React.Dispatch<React.SetStateAction<DressFilters>>;
  applyFilters: (override?: DressFilters) => void;
  maxPrice: number;
}

export function parseUrlToFilters(
  params: ReadonlyURLSearchParams,
  fallback: DressFilters,
  dynamicMaxPrice: number
): DressFilters {
  const getList = (key: string): number[] => {
    const val = params.get(key);
    return val ? val.split("_").map(Number) : [];
  };

  const price = params.get("priceGroup")?.split("-") ?? [];

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

    priceMin: price[0] ? Number(price[0]) : 0,
    priceMax: price[1] ? Number(price[1]) : dynamicMaxPrice,

    orderBy: params.get("orderBy") ?? "",
  };
}
