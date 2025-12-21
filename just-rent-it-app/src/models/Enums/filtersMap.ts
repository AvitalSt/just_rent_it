import { DressState } from "./DressState";
import { DressStatus } from "./DressStatus";
import { RentStatus } from "./RentStatus";
import { SaleType } from "./SaleType";

export const DressStateMap: Record<number, string> = {
  [DressState.New]: "חדש",
  [DressState.LikeNew]: "כמו חדש",
  [DressState.Used]: "משומש",
};

export const SaleTypeMap: Record<number, string> = {
  [SaleType.Rent]: "להשכרה",
  [SaleType.Sell]: "למכירה",
};

export const DressStatusMap: Record<number, string> = {
  [DressStatus.Pending]: "ממתין לאישור",
  [DressStatus.Active]: "פעיל",
  [DressStatus.Deleted]: "נמחק",
};

export const OrderByMap: Record<string, string> = {
  "": "ברירת מחדל",
  "price_asc": "מחיר - מהזול ליקר",
  "price_desc": "מחיר - מהיקר לזול",
  "most_viewed": "נצפה ביותר",
  "newest": "חדשים באתר",
};

export const RentStatusMap: Record<number, string> = {
  [RentStatus.Pending]: "בהמתנה",
  [RentStatus.Confirmed]: "אושרה",
  [RentStatus.Cancelled]: "בוטל",
  [RentStatus.Paid]: "שולם",
};