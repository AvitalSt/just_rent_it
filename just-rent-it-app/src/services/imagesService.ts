import { DressImageDTO } from "@/models/DTOs/ImageDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE_Images = "/Images";

function authHeader() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export async function uploadImages(
  files: File[],
  dressId?: number
): Promise<DressImageDTO[]> {
  const formData = new FormData();

  // הוספת כל הקבצים ל-FormData
  files.forEach((f) => formData.append("files", f));

  // אם יש dressId מוסיפים אותו
  if (dressId !== undefined) {
    formData.append("dressId", dressId.toString());
  }

  const res = await axiosInstance.post(`${API_BASE_Images}/upload`, formData, {
    headers: authHeader(),
  });

  return res.data.data;
}
