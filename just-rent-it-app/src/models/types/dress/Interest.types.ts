import { DressDTO } from "@/models/DTOs/DressDTO";

export interface InterestProps {
  dress: DressDTO;
  interestMessage: string;
  setInterestMessage: (v: string) => void;
  onSubmit: () => void;
  onClose: () => void;
  loading: boolean;
}
