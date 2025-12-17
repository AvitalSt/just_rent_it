import Link from "next/link"; 
import { MostViewedSectionProps } from "@/models/types/home/MostViewedSection.types";

export default function MostViewedSection({ initial }: MostViewedSectionProps) {
  const API_BASE_ORIGIN = process.env.NEXT_PUBLIC_API_BASE_ORIGIN;

  return (
    <section className="py-24 px-6 bg-gray-50">
      <div className="max-w-7xl mx-auto text-center mb-16">
        <h2 className="text-4xl md:text-5xl font-extralight text-black mb-4">
          השמלות הכי נצפות
        </h2>
        <p className="text-gray-500 font-light">
          השמלות שכולם מתעניינים בהן
        </p>
      </div>

      <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
        {initial.map((dress) => (
          <Link
            key={dress.dressID}
            href={`/dresses/${dress.dressID}`}
            className="aspect-[3/4] bg-gray-200 relative overflow-hidden group"
          >
            <img
              src={`${API_BASE_ORIGIN}${dress.mainImage}`}
              alt={dress.name}
              className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
            />
            <div className="absolute inset-0 bg-black opacity-0 group-hover:opacity-10 transition-opacity duration-300" />
          </Link>
        ))}
      </div>

      <div className="text-center mt-12">
        <Link
          href="/dresses"
          className="inline-block px-10 py-4 border-2 border-black text-black text-sm tracking-widest uppercase font-medium hover:bg-black hover:text-white transition-all duration-300"
        >
          לכל השמלות
        </Link>
      </div>
    </section>
  );
}
