export interface UpdateDressDTO {
  Name?: string;
  Description?: string;
  Price?: number;

  ColorIDs?: number[];
  SizeIDs?: number[];
  CityIDs?: number[];
  AgeGroupIDs?: number[];
  EventTypeIDs?: number[];

  SaleType?: number;
  State?: number;
  Status?: number;

  MainImage?: string;
  RemoveImageIds?: number[];
  AddImagePaths?: string[];
}
