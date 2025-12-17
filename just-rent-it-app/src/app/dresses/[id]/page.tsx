"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import { getDressById } from "@/services/dressService";
import DressPage from "@/components/dresses/dress/DressPage";
import { DressDTO } from "@/models/DTOs/DressDTO";
import NotFound from "@/app/not-found";
import Loading from "@/components/ui/Loading";

export default function DressIdPage() {
  const { id } = useParams();
  const numId = Number(id);

  const [dress, setDress] = useState<DressDTO | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (isNaN(numId)) {
      setError("not-found");
      setLoading(false);
      return;
    }

    getDressById(numId)
      .then((res) => setDress(res))
      .catch(() => setError("not-found"))
      .finally(() => setLoading(false));
  }, [numId]);

  if (loading) return <Loading />;

  if (error || !dress) return <NotFound />;

  return <DressPage initialDress={dress} />;
}
