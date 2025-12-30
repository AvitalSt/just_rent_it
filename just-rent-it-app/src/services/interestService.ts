import { InterestListDTO } from "@/models/DTOs/InterestListDTO";
import { PagedResultDTO } from "@/models/DTOs/PagedResultDTO";
import {
  InterestDraftFilters,
  InterestFilterParams,
} from "@/models/types/interest/InterestFilterParams";
import { buildInterestParams } from "@/utils/filterUtils";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE_Interest = "/Interest";

function authHeader() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export function mapFiltersToInterestParams(filters: InterestDraftFilters) {
  const params = buildInterestParams(filters);

  for (const key in params) {
    if (!params[key]) delete params[key];
  }

  return params;
}

export async function sendInterest(
  dressId: number,
  message: string
): Promise<void> {
  await axiosInstance.post(
    API_BASE_Interest,
    { dressID: dressId, message },
    { headers: authHeader() }
  );
}

export async function getFilteredInterests(
  params: InterestFilterParams
): Promise<PagedResultDTO<InterestListDTO>> {
  const res = await axiosInstance.get(API_BASE_Interest, {
    params,
    headers: authHeader(),
  });

  return res.data.data;
}

export const updateStatus = (id: number, status: string) => {
  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/rent-status`,
    JSON.stringify(status),
    {
      headers: { ...authHeader(), "Content-Type": "application/json" },
    }
  );
};

export const updateNotes = (id: number, notes: string) => {
  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/notes`,
    JSON.stringify(notes),
    {
      headers: { ...authHeader(), "Content-Type": "application/json" },
    }
  );
};

export const updateOwnerComment = (id: number, comment: string) => {
  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/owner-comment`,
    JSON.stringify(comment),
    {
      headers: { ...authHeader(), "Content-Type": "application/json" },
    }
  );
};

export const updateUserComment = (id: number, comment: string) => {
  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/user-comment`,
    JSON.stringify(comment),
    {
      headers: { ...authHeader(), "Content-Type": "application/json" },
    }
  );
};

export const messageOwner = (id: number) => {
  return axiosInstance.post(
    `${API_BASE_Interest}/${id}/message-owner`,
    {},
    { headers: authHeader() }
  );
};

export const messageUser = (id: number) => {
  return axiosInstance.post(
    `${API_BASE_Interest}/${id}/message-user`,
    {},
    { headers: authHeader() }
  );
};

export function messageOwnerPayment(interestId: number) {
  return axiosInstance.post(
    `${API_BASE_Interest}/${interestId}/message-owner-payment`,
    {},
    { headers: authHeader() }
  );
}

export function updatePaymentAmount(id: number, paymentAmount: number) {
  return axiosInstance.patch(
    `${API_BASE_Interest}/${id}/paymentAmount`,
    { paymentAmount },
    { headers: authHeader() }
  );
}

export function exportInterests(params: InterestFilterParams) {
  return axiosInstance.get(`${API_BASE_Interest}/export`, {
    params,
    responseType: "blob",
    headers: authHeader(),
  });
}
