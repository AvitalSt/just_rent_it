import { DressImageDTO } from "@/models/DTOs/ImageDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;
const API_BASE_Images = `${API_BASE}/Images`;

export async function uploadImages(
  files: File[],
  dressId?: number
): Promise<DressImageDTO[]> {
  const token = localStorage.getItem("token");

  const formData = new FormData();

  // הוספת כל הקבצים ל-FormData
  files.forEach((f) => formData.append("files", f));

  // אם יש dressId מוסיפים אותו
  if (dressId !== undefined) {
    formData.append("dressId", dressId.toString());
  }

  const res = await axiosInstance.post(`${API_BASE_Images}/upload`, formData, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data.data;
}
