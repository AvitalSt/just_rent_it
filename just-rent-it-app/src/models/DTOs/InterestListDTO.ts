export interface InterestListDTO {
  interestID: number;
  dressID: number;
  dressName: string;
  ownerID: number;
  ownerName: string;
  ownerEmail:string;
  userID: number;
  userName: string;
  userEmail:string;
  sentDate: string;
  status: string;
   amountPaid: number;
  isPaymentTransferred: boolean;
  paymentAmount: number;
  notes: string;
  ownerComment: string;
  userComment: string;
  message: string;
  ownerMailCount:number;
  userMailCount:number;
}
