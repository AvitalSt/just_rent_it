interface InfiniteScrollContainerProps {
  children: React.ReactNode;
  hasMore: boolean;       // האם יש עוד נתונים לטעון
  loadMore: () => void;   // פונקציה לטעינה נוספת
}