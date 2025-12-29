import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  reactStrictMode: false,

  images: {
    remotePatterns: [
      {
        protocol: "https",
        hostname: "localhost",
        port: "7144",
        pathname: "/Uploads/**",
      },
      {
        protocol: "https",
        hostname: "localhost",
        port: "7144",
        pathname: "/images/**",
      },
    ],
  },
};

export default nextConfig;
