"use client";

import WishlistList from "./WishlistList";
import Loading from "@/components/ui/Loading";
import WishlistEmpty from "./WishlistEmpty";
import { useWishlist } from "@/hooks/useWishlist";
import { useAppSelector } from "@/store/hooks";
import { useEffect, useState } from "react";
import { getDressesByIds } from "@/services/wishlistService";
import { DressListDTO } from "@/models/DTOs/DressListDTO";
import { useLocalWishlist } from "@/hooks/useLocalWishlist";

export default function WishlistPage() {
  const { remove } = useWishlist();
  const { localWishlist } = useLocalWishlist();
  const user = useAppSelector((s) => s.user.user);

  const [dresses, setDresses] = useState<DressListDTO[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      setLoading(true);

      const idsToLoad = user?.wishlistDressIds?.length
        ? user.wishlistDressIds
        : localWishlist;

      if (!idsToLoad?.length) {
        setDresses([]);
        setLoading(false);
        return;
      }

      try {
        const res = await getDressesByIds(idsToLoad);
        setDresses(res.data ?? []);
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [user, localWishlist]);
  
 const handleRemove = async (id: number) => {
  await remove(id);

  setTimeout(() => {
    setDresses(prev => prev.filter(d => d.dressID !== id));
  }, 1000); 
};


  if (loading) return <Loading />;
  if (dresses.length === 0) return <WishlistEmpty />;

  return <WishlistList dresses={dresses} onRemoved={handleRemove} />;
}
