import { AddDressDTO } from "@/models/DTOs/AddDressDTO";
import { DressFormChangeHandler } from "./DressForm.types";

export interface DressDetailsProps {
  formData: AddDressDTO;
  handleChange: DressFormChangeHandler;
}
