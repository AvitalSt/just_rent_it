import { AddDressDTO } from "@/models/DTOs/AddDressDTO";

export type DressFormChangeHandler = <K extends keyof AddDressDTO>(
  key: K,
  value: AddDressDTO[K]
) => void;
