"use client";

import InfiniteScrollContainer from "@/components/ui/layout/InfiniteScrollContainer";
import DressCard from "./DressCard";
import { DressGridProps } from "@/models/types/dress/DressGrid.types";

export default function DressGrid({
  dresses,
  loadMore,
  hasMore,
  loading,
  onRemoved,
  isWishlistPage,
}: DressGridProps) {
  // הסרנו את הפיצול ל-totalPages כדי לאפשר זרימה רציפה של ה-Grid
  
  return (
    <InfiniteScrollContainer hasMore={hasMore} loadMore={loadMore}>
      {/* הגריד הראשי מכיל כעת את כל השמלות ישירות */}
      <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-5 gap-6">
        {dresses.map((dress, index) => (
          <div 
            key={dress.dressID}
            // אנחנו שומרים על ה-data-page ברמת הפריט כדי שה-Observer ימשיך לעבוד
            data-page={Math.floor(index / 25) + 1}
          >
            <DressCard
              dress={dress}
              onRemoved={onRemoved}
              isWishlistPage={isWishlistPage}
            />
          </div>
        ))}
      </div>

      {loading && (
        <div className="flex justify-center py-10">
          <div className="h-8 w-8 border-4 border-gray-300 border-t-black animate-spin rounded-full"></div>
        </div>
      )}
    </InfiniteScrollContainer>
  );
}