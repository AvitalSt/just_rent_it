"use client";

import React from "react";
import { DressListProps } from "@/models/DTOs/DressListProps";
import DressGrid from "../DressFilter/DressGrid";
import { Heart } from "lucide-react";

const WishlistList = ({ dresses, onRemoved }: DressListProps) => {
  return (
    <div className="min-h-screen bg-white p-6">
      <div className="max-w-7xl mx-auto">
        <div className="flex flex-col items-center mb-12">
          <Heart className="w-16 h-16 fill-black text-black" />
          <h1 className="text-4xl font-bold tracking-wider mt-4">MY LIST</h1>
        </div>

        <DressGrid
          dresses={dresses}
          loadMore={() => {}}
          hasMore={false}
          loading={false}
          onRemoved={onRemoved}
          isWishlistPage={true}
        />
      </div>
    </div>
  );
};

export default WishlistList;
