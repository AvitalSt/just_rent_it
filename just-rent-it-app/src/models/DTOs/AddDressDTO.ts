import { DressState } from "../Enums/DressState";
import { SaleType } from "../Enums/SaleType";

export interface AddDressDTO {
  Name: string;
  Description: string;
  Price: number | null;
  ColorIds: number[];
  SizeIds: number[];
  CityIds: number[];
  AgeGroupIds: number[];
  EventTypeIds: number[];
  MainImage: string;
  SaleType: SaleType | null;
  State: DressState | null;
  ImagePaths: string[];
}
