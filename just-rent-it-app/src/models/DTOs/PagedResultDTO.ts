export interface PagedResultDTO<T> {
  items: T[];
  totalCount: number;
}