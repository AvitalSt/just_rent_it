import { UserRole } from "../Enums/UserRole";

export interface UserDTO {
  userID: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  role: UserRole;
  createdDate: string;
  updateAt: string;
  wishlistDressIds: number[];
}
