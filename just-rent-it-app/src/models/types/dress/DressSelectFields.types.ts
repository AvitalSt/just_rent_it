import { AddDressDTO } from "@/models/DTOs/AddDressDTO";
import {
  AgeGroupDTO,
  CityDTO,
  ColorDTO,
  EventTypeDTO,
  SizeDTO,
} from "@/models/DTOs/OptionsDTO";
import { DressFormChangeHandler } from "./DressForm.types";

export interface DressSelectFieldsProps {
  formData: AddDressDTO;
  handleChange: DressFormChangeHandler;
  colors: ColorDTO[];
  sizes: SizeDTO[];
  cities: CityDTO[];
  ageGroups: AgeGroupDTO[];
  eventTypes: EventTypeDTO[];
}
