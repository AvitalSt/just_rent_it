import { AppDispatch } from "@/store";
import { AppRouterInstance } from "next/dist/shared/lib/app-router-context.shared-runtime";
import { UserDTO } from "../../DTOs/UserDTO";

export interface SuccessfulAuthProps {
  dispatch: AppDispatch;
  router: AppRouterInstance;
  user: UserDTO;
  token: string;
}