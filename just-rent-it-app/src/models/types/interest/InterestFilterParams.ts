export interface InterestFilterParams {
  status?: string;
  ownerName?: string;
  userName?: string;
  dressName?: string;
  from?: string;  
  to?: string;   
  pageNumber?: number;
  pageSize?: number;
}

export interface InterestDraftFilters {
  status: number[];
  ownerName: string;
  userName: string;
  dressName: string;
  from: string;
  to: string;
}