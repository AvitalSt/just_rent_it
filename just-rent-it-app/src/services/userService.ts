import { AuthResponse } from "@/models/DTOs/AuthResponse";
import { UpdateUserDTO } from "@/models/DTOs/UpdateUserDTO";
import { UserDTO } from "@/models/DTOs/UserDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE_Users = "/Users";

function authHeader() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export async function sendPasswordResetEmail(email: string): Promise<any> {
  const response = await axiosInstance.post(
    `${API_BASE_Users}/forgot-password`,
    { email }
  );
  return response.data;
}

export async function resetPassword(
  token: string,
  newPassword: string
): Promise<AuthResponse> {
  const response = await axiosInstance.post(
    `${API_BASE_Users}/reset-password`,
    { token, newPassword }
  );
  return response.data;
}

export async function currentUser(): Promise<UserDTO> {
  const response = await axiosInstance.get(`${API_BASE_Users}/current`, {
    headers: authHeader(),
  });
  return response.data.data;
}

export async function updateUser(
  userId: number,
  data: UpdateUserDTO
): Promise<UserDTO> {
  const response = await axiosInstance.patch(
    `${API_BASE_Users}/${userId}`,
    data,
    { headers: authHeader() }
  );

  return response.data.data;
}
