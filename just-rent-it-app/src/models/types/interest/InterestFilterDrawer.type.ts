import { InterestDraftFilters } from "./InterestFilterParams";

export interface InterestFilterDrawerProps {
  open: boolean;
  setOpen: (v: boolean) => void;
  draftFilters: InterestDraftFilters;
  setDraftFilters: (f: InterestDraftFilters) => void;
  applyFilters: () => void;
  refresh: () => void;
}