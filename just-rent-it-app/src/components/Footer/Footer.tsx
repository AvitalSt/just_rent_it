import { Linkedin } from "lucide-react";

export default function Footer() {
  const MAIL = process.env.MAIL;

  return (
    <footer className="bg-white border-t border-gray-200 pt-6 pb-4 mt-12">
      <div className="max-w-7xl mx-auto px-8 space-y-6">
        <div className="bg-gradient-to-br from-black to-neutral-800 text-white py-4 px-5 rounded-2xl text-center shadow-md">
          <h2 className="text-base font-bold mb-1">צרו קשר</h2>
          <p className="text-xs mb-2 opacity-90">יש לכם שאלות? נשמח לעזור.</p>

          <a
            href={`mailto:${MAIL}`}
            className="text-white font-semibold text-sm border-b border-white pb-0.5 hover:opacity-80 transition-opacity duration-300"
          >
            {MAIL}
          </a>
        </div>

        <div className="bg-gray-50 py-2 rounded-lg shadow-sm">
          <div className="flex items-center justify-center gap-2 text-gray-600">
            <p className="text-xs">Create by AVITAL © 058-3130909</p>

            <a
              href="https://www.linkedin.com/in/avital-steinmetz/"
              target="_blank"
              rel="noopener noreferrer"
              className="flex items-center gap-1 font-medium hover:text-gray-900 transition-colors group"
            >
              <Linkedin className="w-3 h-3 text-blue-600 group-hover:text-blue-700 transition-colors" />
            </a>
          </div>
        </div>
      </div>
    </footer>
  );
}
