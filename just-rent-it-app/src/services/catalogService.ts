import { axiosInstance } from "@/services/axiosInstance";

function getToken() {
  return localStorage.getItem("token");
}

export async function updateCatalog() {
  const token = getToken();

  const res = await axiosInstance.post(
    `/Catalog/update`,
    {},
    {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    }
  );

  return res.data;
}

export async function downloadCatalog() {
  const res = await axiosInstance.get(`/Catalog`, {
    responseType: "arraybuffer",
    headers: {
      Accept: "application/pdf",
    },
  });

  return new Blob([res.data], { type: "application/pdf" });
}
