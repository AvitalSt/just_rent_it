import { DressImageDTO } from "./ImageDTO";

export interface DressDTO {
  dressID: number;
  name: string;
  description: string;
  price: number;
  colors: string[];
  sizes: string[];
  cities: string[];
  ageGroups: string[];
  eventTypes: string[];
  saleType: number;
  state: number;
  status: number;
  mainImage: string;
  images: DressImageDTO[];
  categories: string[];
  isFavorite: boolean;
  views: number;
}