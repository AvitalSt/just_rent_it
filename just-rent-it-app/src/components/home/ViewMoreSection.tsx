import Link from "next/link";

export default function ViewMoreSection() {
  return (
    <section className="py-32 px-6 bg-black text-white">
      <div className="max-w-4xl mx-auto text-center">
        <h2 className="text-4xl md:text-6xl font-extralight mb-8">
          מוכנה למצוא את השמלה המושלמת?
        </h2>
        <p className="text-lg text-gray-400 font-light mb-12">
          גלי את הקולקציה שלנו עכשיו
        </p>

        <Link
          href="/dresses"
          className="inline-block px-12 py-5 bg-white text-black text-sm tracking-widest uppercase font-medium hover:bg-gray-200 transition-colors duration-300"
        >
          לקולקציה המלאה
        </Link>
      </div>
    </section>
  );
}
