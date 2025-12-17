import { DressDTO } from "@/models/DTOs/DressDTO";

export interface DressViewProps {
  dress: DressDTO;
  reload: () => void;
}