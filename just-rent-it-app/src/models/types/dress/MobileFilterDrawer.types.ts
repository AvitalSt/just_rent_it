import { DressFilters } from "./DressFilter.types";

export interface MobileFilterDrawerProps {
  mobileOpen: boolean;
  setMobileOpen: (v: boolean) => void;
  draftFilters: DressFilters;
  setDraftFilters: React.Dispatch<React.SetStateAction<DressFilters>>;
  applyFilters: (f?: DressFilters) => void;
}