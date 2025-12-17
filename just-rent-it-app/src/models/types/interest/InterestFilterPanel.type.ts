import { InterestDraftFilters } from "./InterestFilterParams";

export interface InterestFilterPanelProps {
  draftFilters: InterestDraftFilters;
  setDraftFilters: (f: InterestDraftFilters) => void;
  applyFilters: () => void;
  resetFilters: () => void;
}