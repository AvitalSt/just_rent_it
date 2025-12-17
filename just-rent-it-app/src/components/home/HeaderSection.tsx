import Link from "next/link";

export default function HeaderSection() {
  return (
    <section className="relative h-screen flex items-center justify-center overflow-hidden bg-black">
      <div className="absolute inset-0 opacity-10">
        <div
          className="absolute inset-0"
          style={{
            backgroundImage:
              "repeating-linear-gradient(0deg, transparent, transparent 2px, white 2px, white 4px)",
          }}
        ></div>
      </div>

      <div className="relative z-10 text-center px-6 max-w-5xl">
        <h1 className="text-6xl sm:text-7xl md:text-8xl lg:text-9xl font-extralight text-white tracking-tight leading-none mb-6">
          Just Rent It
        </h1>

        <div className="h-px w-24 bg-white mx-auto mb-8 opacity-60"></div>

        <p className="text-lg sm:text-xl md:text-2xl text-gray-300 font-light tracking-wider mb-16">
          אתר השכרת שמלות שיתופי, מאפשר לך למצוא את השמלה המושלמת בקלות.
        </p>

        <div className="flex flex-col sm:flex-row gap-4 justify-center">
          <Link
            href="/dresses"
            className="group relative px-10 py-4 bg-white text-black overflow-hidden"
          >
            <span className="relative z-10 text-sm tracking-widest font-medium uppercase">
              גלי את הקולקציה
            </span>
            <div className="absolute inset-0 bg-gray-200 scale-x-0 group-hover:scale-x-100 transition-transform origin-left duration-300"></div>
          </Link>

          <Link
            href="/catalog"
            className="group relative px-10 py-4 border border-white text-white overflow-hidden"
          >
            <span className="relative z-10 text-sm tracking-widest font-medium uppercase group-hover:text-gray-500">
              הורידי קטלוג
            </span>

            <div className="absolute inset-0 bg-white scale-x-0 group-hover:scale-x-100 transition-transform origin-left duration-300"></div>

            <span className="absolute inset-0 flex items-center justify-center text-black opacity-0 group-hover:opacity-100 transition-opacity duration-300 text-sm tracking-widest font-medium uppercase">
              הורידי קטלוג
            </span>
          </Link>
        </div>
      </div>

      <div className="absolute bottom-8 left-1/2 -translate-x-1/2">
        <div className="w-px h-16 bg-white opacity-30 animate-pulse"></div>
      </div>
    </section>
  );
}
