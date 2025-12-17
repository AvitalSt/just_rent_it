import { UserDTO } from "./UserDTO";

export interface AuthResponse {
  user: UserDTO;
  token: string;
}