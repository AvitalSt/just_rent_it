import "./globals.css";
import type { Metadata } from "next";
import { GoogleOAuthProvider } from "@react-oauth/google";
import { ProviderWrapper } from "@/store/ProviderWrapper";
import { AppWrapper } from "@/store/AppWrapper";
import { InitializeOptions } from "@/store/InitializeOptions";
import Header from "@/components/header/Header";
import Footer from "@/components/Footer/Footer";
import { Heebo } from "next/font/google";

const heebo = Heebo({
  subsets: ["hebrew", "latin"],
  weight: ["300", "400", "500", "700", "900"],
  variable: "--font-heebo",
});

export const metadata: Metadata = {
  title: "Just Rent It",
  description: "Dress rental platform",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="he" dir="rtl" className={`${heebo.variable}`}>
      <body className="flex flex-col min-h-screen">
        <GoogleOAuthProvider
          clientId={process.env.NEXT_PUBLIC_GOOGLE_CLIENT_ID!}
        >
          <ProviderWrapper>
            <InitializeOptions />
            <AppWrapper>
              <Header />
              {children}
              <Footer />
            </AppWrapper>
          </ProviderWrapper>
        </GoogleOAuthProvider>
      </body>
    </html>
  );
}
