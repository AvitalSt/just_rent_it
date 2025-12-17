import { axiosInstance } from "@/services/axiosInstance";

const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;
const API_BASE_Wishlist = `${API_BASE}/Wishlist`;

function getToken() {
  return localStorage.getItem("token");
}

export const addToWishlist = async (dressId: number) => {
  const token = getToken();

  const res = await axiosInstance.post(`${API_BASE_Wishlist}`, dressId, {
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
  });

  return res.data;
};

export const removeFromWishlist = async (dressId: number) => {
  const token = getToken();

  const res = await axiosInstance.delete(`${API_BASE_Wishlist}/${dressId}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data;
};

export const getMyWishlist = async () => {
  const token = getToken();

  const res = await axiosInstance.get(`${API_BASE_Wishlist}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data;
};

export const getDressesByIds = async (ids: number[]) => {
  if (ids.length === 0) return { items: [] };

  const token = getToken();

  const query = ids.join(",");

  const res = await axiosInstance.get(`${API_BASE_Wishlist}/by-ids?ids=${query}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data;
};
