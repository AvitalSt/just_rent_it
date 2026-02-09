import { getMostViewedDresses } from "@/services/dressService";

import HeaderSection from "./HeaderSection";
import MostViewedSection from "./MostViewedSection";
import CategoriesSection from "./CategoriesSection";
import HowItWorksSection from "./HowItWorksSection";
import ViewMoreSection from "./ViewMoreSection";

export const revalidate = 60 * 60 * 24;

export default async function Home() {
  const mostViewed = await getMostViewedDresses();

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
