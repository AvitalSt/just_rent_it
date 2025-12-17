import { DressFilters } from "@/models/types/dress/DressFilter.types";
import { InterestDraftFilters } from "@/models/types/interest/InterestFilterParams";

// היא מקבלת פילטרים (cityIds, sizeIds וכו’)
// ואם הם לא ריקים → מחברת אותם למחרוזת עם _
// ומחזירה אובייקט עם כל הפרמטרים מוכנים.

export function buildFilterParams(filters: DressFilters) {
  const params: Record<string, string | undefined> = {};

  if (filters.cityIds.length) params.city = filters.cityIds.join("_");

  if (filters.colorIds.length) params.colorGroup = filters.colorIds.join("_");

  if (filters.sizeIds.length) params.sizeGroup = filters.sizeIds.join("_");

  if (filters.ageGroupIds.length)
    params.ageGroup = filters.ageGroupIds.join("_");

  if (filters.eventTypeIds.length)
    params.eventType = filters.eventTypeIds.join("_");

  if (filters.saleTypeIds.length)
    params.saleType = filters.saleTypeIds.join("_");

  if (filters.stateGroupIds.length)
    params.stateGroup = filters.stateGroupIds.join("_");

  if (filters.statusGroupIds.length)
    params.statusGroup = filters.statusGroupIds.join("_");

  if (filters.priceMin !== null && filters.priceMax !== null)
    params.priceGroup = `${filters.priceMin}_${filters.priceMax}`;

  if (filters.orderBy) params.orderBy = filters.orderBy;

  return params;
}

export function buildInterestParams(filters: InterestDraftFilters) {
  const params: Record<string, string | undefined> = {};

  if (filters.status.length > 0) {
    params.status = filters.status.join("_");
  }

//trim() מוחק רווחים מיותרים.
  if (filters.ownerName.trim()) {
    params.ownerName = filters.ownerName.trim();
  }

  if (filters.userName.trim()) {
    params.userName = filters.userName.trim();
  }

  if (filters.dressName.trim()) {
    params.dressName = filters.dressName.trim();
  }

  if (filters.from) {
    params.from = filters.from;
  }

  if (filters.to) {
    params.to = filters.to;
  }

  return params;
}
