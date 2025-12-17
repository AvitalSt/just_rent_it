"use client";

export default function ErrorPage({ error, reset }: { error: Error; reset: () => void }) {
  return (
    <div
      className="h-screen w-full flex items-center justify-center bg-white text-center px-6"
      dir="rtl"
    >
      <div>
        <h1 className="text-5xl font-light mb-4 text-black">משהו השתבש…</h1>
        <p className="text-gray-500 mb-8 text-lg">
          חלה תקלה בטעינת הדף. אפשר לנסות שוב.
        </p>

        <button
          onClick={() => reset()}
          className="px-10 py-4 bg-black text-white text-sm tracking-widest uppercase hover:bg-gray-800 transition-all duration-300"
        >
          נסי שוב
        </button>
      </div>
    </div>
  );
}
