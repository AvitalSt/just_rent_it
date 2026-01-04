import { MonthlySummaryLastDTO } from "@/models/DTOs/MonthlySummaryLastDTO";
import { MonthlySummaryPreviewDTO } from "@/models/DTOs/MonthlySummaryPreviewDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE_MonthlySummary = "/MonthlySummary";

function authHeader() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export async function previewMonthlySummary(): Promise<MonthlySummaryPreviewDTO> {
  const res = await axiosInstance.get(`${API_BASE_MonthlySummary}/preview`, {
    headers: authHeader(),
  });

  return res.data.data; 
}

export async function sendMonthlySummary(daysToSplit: number, dayIndex: number) {
  return axiosInstance.post(
    `${API_BASE_MonthlySummary}/send`,
    { daysToSplit, dayIndex },
    { headers: authHeader() }
  );
}

export async function getLastMonthlySummary(): Promise<MonthlySummaryLastDTO> {
  const res = await axiosInstance.get(`${API_BASE_MonthlySummary}/last`, {
    headers: authHeader(),
  });

  return res.data.data;
}
