import { InterestListDTO } from "@/models/DTOs/InterestListDTO";
import { PagedResultDTO } from "@/models/DTOs/PagedResultDTO";
import {
  InterestDraftFilters,
  InterestFilterParams,
} from "@/models/types/interest/InterestFilterParams";
import { buildInterestParams } from "@/utils/filterUtils";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;
const API_BASE_Interest = `${API_BASE}/Interest`;

function getToken() {
  return localStorage.getItem("token");
}

export function mapFiltersToInterestParams(filters: InterestDraftFilters) {
  const params = buildInterestParams(filters);

  for (const key in params) {
    if (!params[key]) delete params[key];
  }

  return params;
}

// שליחת התעניינות
export async function sendInterest(
  dressId: number,
  message: string
): Promise<void> {
  const token = getToken();

  await axiosInstance.post(
    `${API_BASE_Interest}`,
    { dressID: dressId, message },
    { headers: { Authorization: `Bearer ${token}` } }
  );
}

// הבאת כל ההתעניינויות (מנהל)
export async function getFilteredInterests(
  params: InterestFilterParams
): Promise<PagedResultDTO<InterestListDTO>> {
  const token = getToken();

  const res = await axiosInstance.get(API_BASE_Interest, {
    params,
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data.data;
}

// עדכון סטטוס
export const updateStatus = async (id: number, status: string) => {
  const token = getToken();

  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/rent-status`,
    JSON.stringify(status),
    {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    }
  );
};

// עדכון הערות מנהלת
export const updateNotes = async (id: number, notes: string) => {
  const token = getToken();

  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/notes`,
    JSON.stringify(notes),
    {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    }
  );
};

// הערת בעלת השמלה
export const updateOwnerComment = async (id: number, comment: string) => {
  const token = getToken();

  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/owner-comment`,
    JSON.stringify(comment),
    {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    }
  );
};

// הערת המשתמשת
export const updateUserComment = async (id: number, comment: string) => {
  const token = getToken();

  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/user-comment`,
    JSON.stringify(comment),
    {
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    }
  );
};

// שליחת מייל לבעלת השמלה
export const messageOwner = async (id: number) => {
  const token = getToken();

  return axiosInstance.post(
    `${API_BASE_Interest}/${id}/message-owner`,
    {},
    { headers: { Authorization: `Bearer ${token}` } }
  );
};

// שליחת מייל למתעניינת
export const messageUser = async (id: number) => {
  const token = getToken();

  return axiosInstance.post(
    `${API_BASE_Interest}/${id}/message-user`,
    {},
    { headers: { Authorization: `Bearer ${token}` } }
  );
};

export function messageOwnerPayment(interestId: number) {
  const token = getToken();
  return axiosInstance.post(
    `${API_BASE_Interest}/${interestId}/message-owner-payment`,
    {},
    { headers: { Authorization: `Bearer ${token}` } }
  );
}

export function updatePaymentAmount(id: number, paymentAmount: number) {
  const token = getToken();
  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/paymentAmount`,
    { paymentAmount },
    { headers: { Authorization: `Bearer ${token}` } }
  );
}

export function exportInterests(params: InterestFilterParams) {
  const token = getToken();

  return axiosInstance.get(`${API_BASE_Interest}/export`, {
    params,
    responseType: "blob",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
}
