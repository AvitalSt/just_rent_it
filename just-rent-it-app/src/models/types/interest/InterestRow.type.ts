import { InterestListDTO } from "@/models/DTOs/InterestListDTO";

export interface InterestRowProps {
  page: number;
  row: InterestListDTO;
  sendingOwner: Record<number, boolean>;
  sendingUser: Record<number, boolean>;
  sendingPayment: Record<number, boolean>;
  updateField: (id: number, field: string, value: any) => void;
  sendOwnerMail: (row: InterestListDTO) => Promise<void>;
  sendUserMail: (row: InterestListDTO) => Promise<void>;
  sendOwnerPaymentMail: (row: InterestListDTO) => Promise<void>;
}
