import { setUser, setWishlistDressIds } from "@/store/userSlice";
import { addToWishlist, getMyWishlist } from "@/services/wishlistService";
import { SuccessfulAuthProps } from "@/models/types/auth/handleSuccessfulAuth.types";
import { DressListDTO } from "@/models/DTOs/DressListDTO";

export async function mergeLocalWishlistWithServer() {
  const stored = JSON.parse(localStorage.getItem("local-wishlist") || "[]");

  if (!Array.isArray(stored) || stored.length === 0) return;

  for (const id of stored) {
    try {
      await addToWishlist(id);
    } catch {}
  }

  localStorage.removeItem("local-wishlist");
  localStorage.removeItem("seen-wishlist-modal");
}

export async function handleSuccessfulAuth({
  dispatch,
  router,
  user,
  token,
}: SuccessfulAuthProps) {
  localStorage.setItem("token", token);

  await mergeLocalWishlistWithServer();

  const refreshed = await getMyWishlist();
  const updatedUser = {
    ...user,
    wishlistDressIds: refreshed.data?.map((d: DressListDTO) => d.dressID) || [],
  };

  dispatch(setUser({ user: updatedUser }));
  dispatch(setWishlistDressIds(updatedUser.wishlistDressIds));

  router.push("/");
}
