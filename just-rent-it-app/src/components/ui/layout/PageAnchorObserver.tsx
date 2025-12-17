"use client";

import { useEffect, useRef } from "react";
import { useRouter } from "next/navigation";

export default function PageAnchorObserver({
  list,
  basePath,
}: {
  list: unknown[];
  basePath: string;
}) {
  const router = useRouter();
  const lastPageRef = useRef<number>(1);

  useEffect(() => {
    // הדפדפן מאזין ל-scroll 
    const observer = new IntersectionObserver(
      (entries) => {
        const visible = entries
        //משאיר רק אלמנטים שנמצאים כרגע בviewport
          .filter((e) => e.isIntersecting)
          .map((e) => ({
            page: Number(e.target.getAttribute("data-page")),
            //מחזיר את המיקום בפיקסלים יחסי ל-viewport 
            top: e.boundingClientRect.top,
          }))
          //ניקוי ערכים לא חוקיים
          .filter((p) => p.page);

        if (!visible.length) return;

        //reduce עובר על מערך ומחזיר ערך הנמשוך ביותר - שנמצא בראש המסך 
        const current = visible.reduce((a, b) =>
          Math.abs(b.top) < Math.abs(a.top) ? b : a
        );

        if (current.page === lastPageRef.current) return;

        lastPageRef.current = current.page;

        if (current.page === 1) {
          router.replace(basePath, { scroll: false });
        } else {
          router.replace(`${basePath}?pageNumber=${current.page}`, {
            scroll: false,
          });
        }
      },
      {
        //עמוד נחשב פעיל רק כשהוא במרכז
        rootMargin: "-40% 0px -40% 0px",
        threshold: 0,
      }
    );

    document
      .querySelectorAll<HTMLElement>("[data-page]")
      .forEach((el) => observer.observe(el));

    return () => observer.disconnect();
  }, [list]);

  return null;
}
