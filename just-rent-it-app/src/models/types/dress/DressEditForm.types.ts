import { DressDTO } from "@/models/DTOs/DressDTO";

export interface DressEditFormProps {
  dress: DressDTO;
  onCancel: () => void;
  onSaved: () => void;
}