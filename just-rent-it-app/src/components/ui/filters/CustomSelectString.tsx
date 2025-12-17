// "use client";

// import { useState, useRef, useEffect } from "react";
// import { ChevronDown } from "lucide-react";

// interface Option {
//   value: string;
//   label: string;
// }

// interface Props {
//   label: string;
//   value: string | null;
//   options: Option[];
//   onChange: (value: string) => void;
//   required?: boolean;
// }

// export default function CustomSelectString({
//   label,
//   value,
//   options,
//   onChange,
//   required = false
// }: Props) {
//   const [open, setOpen] = useState(false);
//   const ref = useRef<HTMLDivElement>(null);

//   const selectedLabel =
//     options.find((o) => o.value === value)?.label || "בחרי אפשרות";

//   // סגירה בלחיצה מחוץ
//   useEffect(() => {
//     const close = (e: MouseEvent) => {
//       if (ref.current && !ref.current.contains(e.target as Node)) {
//         setOpen(false);
//       }
//     };
//     document.addEventListener("mousedown", close);
//     return () => document.removeEventListener("mousedown", close);
//   }, []);

//   return (
//     <div className="w-full space-y-1" dir="rtl" ref={ref}>
//       <label className="block font-semibold text-gray-700 text-sm">
//         {label} {required && <span className="text-red-500">*</span>}
//       </label>

//       {/* טריגר */}
//       <button
//         type="button"
//         onClick={() => setOpen((o) => !o)}
//         className="
//           w-full border border-gray-300 rounded-lg 
//           p-2.5 text-sm bg-white text-black flex justify-between items-center
//           hover:bg-gray-50 transition
//         "
//       >
//         <span>{selectedLabel}</span>
//         <ChevronDown
//           className={`w-4 h-4 text-gray-600 transition-transform ${
//             open ? "rotate-180" : ""
//           }`}
//         />
//       </button>

//       {/* תפריט */}
//       {open && (
//         <div
//           className="
//             mt-1 absolute w-full bg-white border border-gray-200 
//             rounded-lg shadow-lg z-30 max-h-64 overflow-y-auto
//           "
//         >
//           {options.map((opt) => (
//             <div
//               key={opt.value}
//               onClick={() => {
//                 onChange(opt.value);
//                 setOpen(false);
//               }}
//               className={`
//                 px-3 py-2 cursor-pointer text-sm 
//                 hover:bg-gray-100 transition 
//                 ${opt.value === value ? "bg-gray-200 font-semibold" : ""}
//               `}
//             >
//               {opt.label}
//             </div>
//           ))}
//         </div>
//       )}
//     </div>
//   );
// }
