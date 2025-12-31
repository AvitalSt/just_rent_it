import { axiosInstance } from "@/services/axiosInstance";

const API_BASE_Wishlist = "/Wishlist";

function authHeader() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export const addToWishlist = async (dressId: number) => {
  const res = await axiosInstance.post(`${API_BASE_Wishlist}`, dressId, {
    headers: {
      ...authHeader(),
      'Content-Type': 'application/json',
    },
  });
  return res.data;
};

export const removeFromWishlist = async (dressId: number) => {
  const res = await axiosInstance.delete(`${API_BASE_Wishlist}/${dressId}`, {
    headers: authHeader(),
  });
  return res.data;
};

export const getMyWishlist = async () => {
  const res = await axiosInstance.get(`${API_BASE_Wishlist}`, {
    headers: authHeader(),
  });
  return res.data;
};

export const getDressesByIds = async (ids: number[]) => {
  if (ids.length === 0) return { items: [] };

  const query = ids.join(",");

  const res = await axiosInstance.get(`${API_BASE_Wishlist}/by-ids`, {
    params: { ids: query }, // יותר נקי מלהדביק ל-URL
    headers: authHeader(),
  });

  return res.data;
};
