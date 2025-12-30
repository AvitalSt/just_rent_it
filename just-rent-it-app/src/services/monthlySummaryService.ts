import { MonthlySummaryLastDTO } from "@/models/DTOs/MonthlySummaryLastDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE_MonthlySummary = "/MonthlySummary";

function authHeader() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export async function sendMonthlySummary() {
  return axiosInstance.post(
    `${API_BASE_MonthlySummary}/send`,
    {},
    { headers: authHeader() }
  );
}

export async function getLastMonthlySummary(): Promise<MonthlySummaryLastDTO> {
  const res = await axiosInstance.get(`${API_BASE_MonthlySummary}/last`, {
    headers: authHeader(),
  });

  return res.data.data;
}
