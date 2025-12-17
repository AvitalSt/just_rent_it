"use client";

import dynamic from "next/dynamic";

const DressListPage = dynamic(
  () => import("@/components/dresses/DressFilter/DressListPage"),
  { ssr: false } // מבטל Server Side Rendering
);

export default function DressesPage() {
  return <DressListPage />;
}
