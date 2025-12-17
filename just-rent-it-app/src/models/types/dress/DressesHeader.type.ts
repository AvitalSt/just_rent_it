import { DressFilters } from "./DressFilter.types";

export interface DressesHeaderProps {
  loading: boolean;
  totalResults: number;
  draftFilters: DressFilters;
  setDraftFilters: (f: DressFilters) => void;
  applyFilters: (f: DressFilters) => void;
}