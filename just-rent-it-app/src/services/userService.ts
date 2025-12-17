import { AuthResponse } from "@/models/DTOs/AuthResponse";
import { UpdateUserDTO } from "@/models/DTOs/UpdateUserDTO";
import { UserDTO } from "@/models/DTOs/UserDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;
const API_BASE_Users = `${API_BASE}/Users`;

function getToken() {
  return localStorage.getItem("token");
}

export async function sendPasswordResetEmail(email: string): Promise<any> {
  const response = await axiosInstance.post(`${API_BASE_Users}/forgot-password`, {
    email,
  });
  return response.data;
}

export async function resetPassword(
  token: string,
  newPassword: string
): Promise<AuthResponse> {
  const response = await axiosInstance.post(`${API_BASE_Users}/reset-password`, {
    token,
    newPassword,
  });
  return response.data;
}

export async function currentUser(token: string): Promise<UserDTO> {
  const response = await axiosInstance.get(`${API_BASE_Users}/current`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
  return response.data.data;
}

export async function updateUser(
  userId: number,
  data: UpdateUserDTO
): Promise<UserDTO> {
  const token = getToken();
  const response = await axiosInstance.patch(`${API_BASE_Users}/${userId}`, data, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return response.data.data;
}
