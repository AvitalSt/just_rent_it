import { MonthlySummaryLastDTO } from "@/models/DTOs/MonthlySummaryLastDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;
const API_BASE_MonthlySummary = `${API_BASE}/MonthlySummary`;

function getToken() {
  return localStorage.getItem("token");
}

export async function sendMonthlySummary() {
  const token = getToken();
  return axiosInstance.post(
    `${API_BASE_MonthlySummary}/send`,
    {},
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );
}

export async function getLastMonthlySummary():Promise<MonthlySummaryLastDTO> {
  const token = getToken();

  const res = await axiosInstance.get(`${API_BASE_MonthlySummary}/last`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data.data;
}
