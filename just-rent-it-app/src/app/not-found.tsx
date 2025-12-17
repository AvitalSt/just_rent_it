import Link from "next/link";

export default function NotFound() {
  return (
    <div
      className="h-screen w-full bg-white flex items-center justify-center p-6 relative overflow-hidden"
      dir="rtl"
    >
      <div className="absolute inset-0 flex items-center justify-center pointer-events-none">
        <div className="text-[280px] sm:text-[350px] md:text-[450px] lg:text-[550px] font-black text-gray-100 leading-none select-none">
          404
        </div>
      </div>

      <div className="relative z-10 text-center flex flex-col items-center">
        <h2 className="text-3xl md:text-4xl font-bold text-gray-800 mb-2 -mt-10">
          ,Oops
        </h2>

        <h1 className="text-5xl md:text-6xl font-black text-black mb-12">
          העמוד לא נמצא
        </h1>

        <Link
          href="/"
          className="inline-flex items-center gap-3 bg-black text-white px-8 py-4 font-semibold hover:bg-gray-800 transition-all duration-200"
        >
          <span>לדף הבית</span>
        </Link>
      </div>
    </div>
  );
}
