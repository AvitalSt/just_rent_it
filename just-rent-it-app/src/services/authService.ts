import { AuthResponse } from "@/models/DTOs/AuthResponse";
import { RegisterDTO } from "@/models/DTOs/RegisterDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE_Auth = "/Auth";

export async function login(
  email: string,
  password: string,
  rememberMe: boolean
): Promise<AuthResponse> {
  const res = await axiosInstance.post(`${API_BASE_Auth}/login`, {
    email,
    password,
    rememberMe,
  });

  return res.data.data;
}

export async function register(data: RegisterDTO): Promise<AuthResponse> {
  const res = await axiosInstance.post(`${API_BASE_Auth}/register`, data);

  return res.data.data;
}

export async function loginGoogle(
  idToken: string,
  rememberMe: boolean
): Promise<AuthResponse> {
  const res = await axiosInstance.post(`${API_BASE_Auth}/google-login`, {
    idToken,
    rememberMe,
  });

  return res.data.data;
}
