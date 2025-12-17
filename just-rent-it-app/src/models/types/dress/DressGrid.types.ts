import { DressListDTO } from "../../DTOs/DressListDTO";

export interface DressGridProps {
  dresses: DressListDTO[];
  pageSize?: number;
  loadMore: () => void;
  hasMore: boolean;
  loading: boolean;
  pageNumber?: number;
  onRemoved?: (id: number) => void;
  isWishlistPage?: boolean;
}
