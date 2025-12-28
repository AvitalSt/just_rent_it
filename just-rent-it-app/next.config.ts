import type { NextConfig } from "next";

const nextConfig = {
  reactStrictMode: false,

  async headers() {
    return [
      {
        source: '/(.*)',
        headers: [
          {
            key: 'Cross-Origin-Opener-Policy',
            value: 'same-origin-allow-popups',
          },
        ],
      },
    ];
  },
  
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
