import { DressListDTO } from "./DressListDTO";

export type DressListProps = {
  dresses: DressListDTO[];
  onRemoved?: (id: number) => void;
};

export type DressProps = {
  dress: DressListDTO;
  onRemoved?: (id: number) => void;
  isWishlistPage?: boolean;
};
