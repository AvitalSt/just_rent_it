export interface PagedResultDTO<T> {
  items: T[];
  totalCount: number;
  maxPrice: number;
}