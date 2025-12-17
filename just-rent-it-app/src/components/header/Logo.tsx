import Image from "next/image";
import Link from "next/link";

export default function Logo() {
  return (
  <Link
      href="/"
      className="absolute left-1/2 transform -translate-x-1/2 cursor-pointer"
    >      <Image
        src="/images/logo-new.png"
        alt="Just Rent It Logo"
        width={80}
        height={40}
        style={{ width: "auto" }} 
        priority // גורם לנקסט לטוען את התמונה מהר יותר כי זה קובץ לוגו
      />
    </Link>
  );
}
