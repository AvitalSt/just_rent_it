import { axiosInstance } from "@/services/axiosInstance";

function authHeader() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export async function updateCatalog() {
  const res = await axiosInstance.post(
    "/Catalog/update",
    {},
    { headers: authHeader() }
  );

  return res.data;
}

export async function downloadCatalog() {
  const res = await axiosInstance.get("/Catalog", {
    responseType: "arraybuffer",
    headers: {
      Accept: "application/pdf",
    },
  });

  return new Blob([res.data], { type: "application/pdf" });
}
