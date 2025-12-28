import type { NextConfig } from "next";

const nextConfig = {
  reactStrictMode: false,

 async headers() {
    return [
      {
        source: '/:path*',
        headers: [
          {
            key: 'Cross-Origin-Opener-Policy',
            value: 'same-origin-allow-popups',
          },
          {
            key: 'Cross-Origin-Embedder-Policy',
            value: 'unsafe-none', 
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
