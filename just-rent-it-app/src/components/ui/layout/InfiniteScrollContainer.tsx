"use client";

import { useEffect, useRef } from "react";

export default function InfiniteScrollContainer({
  children,
  hasMore, // האם יש עוד נתונים לטעון
  loadMore, // פונקציה לטעינה נוספת
}: InfiniteScrollContainerProps) {
  const ref = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    if (!ref.current) return; // אם REF לא מצביע על שום DOM

    //יוצרים IntersectionObserver כלי של דפדפן שבודק אם האלמנט נכנס ל- view
    const observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting && hasMore) {
        loadMore();
      }
    });

    //מחברים את observer לאלמנט
    observer.observe(ref.current);

    //ניקוי זיכרון
    return () => observer.disconnect();
  }, [hasMore, loadMore]);

  return (
    <>
      {children}

      <div ref={ref} className="h-16 flex justify-center items-center">
        {hasMore && <div className="text-gray-500">טוען...</div>}
      </div>
    </>
  );
}
