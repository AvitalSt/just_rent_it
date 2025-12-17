import { getMostViewedDresses } from "@/services/dressService";

import HeaderSection from "./HeaderSection";
import MostViewedSection from "./MostViewedSection";
import CategoriesSection from "./CategoriesSection";
import HowItWorksSection from "./HowItWorksSection";
import ViewMoreSection from "./ViewMoreSection";

let mostViewedCache: any = null;
let cacheTime: number = 0;
const CACHE_TTL = 1000 * 60 * 60 * 24; // 24 שעות

export default async function Home() {
  const now = Date.now();
  let mostViewed;

  if (mostViewedCache && now - cacheTime < CACHE_TTL) {
    mostViewed = mostViewedCache; 
  } else {
    mostViewed = await getMostViewedDresses(); 
    mostViewedCache = mostViewed;
    cacheTime = now;
  }

  return (
    <div className="bg-white" dir="rtl">
      <HeaderSection />
      <MostViewedSection initial={mostViewed} />
      <CategoriesSection />
      <HowItWorksSection />
      <ViewMoreSection />
    </div>
  );
}
