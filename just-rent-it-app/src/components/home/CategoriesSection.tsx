import Link from "next/link";

export default function CategoriesSection() {
  return (
    <section className="py-24 px-6">
      <div className="max-w-7xl mx-auto text-center mb-20">
        <h2 className="text-4xl md:text-5xl font-extralight">מה את מחפשת?</h2>
        <p className="text-gray-500 font-light">בחרי את הקטגוריה המתאימה לך</p>
      </div>

      <div className="grid md:grid-cols-3 gap-px bg-gray-200">

        <Link
          href="/dresses"
          className="group bg-white p-16 hover:bg-black transition-colors duration-500"
        >
          <div className="text-center">
            <div className="mb-8 flex justify-center">
              <div className="relative w-20 h-20">
                <div className="absolute inset-0 border-t-2 border-l-2 border-black group-hover:border-white"></div>
                <div className="absolute inset-0 border-b-2 border-r-2 border-black group-hover:border-white translate-x-2 translate-y-2"></div>
              </div>
            </div>

            <h3 className="text-2xl font-light group-hover:text-white mb-3">
              כל השמלות
            </h3>

            <p className="text-sm text-gray-500 group-hover:text-gray-300">
              מאות שמלות לכל אירוע
            </p>
          </div>
        </Link>

        <Link
          href="/add-dress"
          className="group bg-white p-16 hover:bg-black transition-colors duration-500"
        >
          <div className="text-center">
            <div className="mb-8 flex justify-center">
              <div className="relative w-20 h-20">
                <div className="absolute top-0 left-1/2 w-px h-full bg-black group-hover:bg-white -translate-x-1/2"></div>
                <div className="absolute left-0 top-1/2 h-px w-full bg-black group-hover:bg-white -translate-y-1/2"></div>
              </div>
            </div>

            <h3 className="text-2xl font-light group-hover:text-white mb-3">
              העלאת שמלה
            </h3>

            <p className="text-sm text-gray-500 group-hover:text-gray-300">
              הוסיפי שמלה לאוסף
            </p>
          </div>
        </Link>

        <Link
          href="/wish-list"
          className="group bg-white p-16 hover:bg-black transition-colors duration-500"
        >
          <div className="text-center">
            <div className="mb-8 flex justify-center">
              <div className="relative w-20 h-20">
                <div className="absolute inset-0 border border-black group-hover:border-white rotate-45"></div>
                <div className="absolute inset-0 border border-black group-hover:border-white"></div>
              </div>
            </div>

            <h3 className="text-2xl font-light group-hover:text-white mb-3">
              המועדפים שלי
            </h3>

            <p className="text-sm text-gray-500 group-hover:text-gray-300">
              השמלות שאהבת
            </p>
          </div>
        </Link>

      </div>
    </section>
  );
}
