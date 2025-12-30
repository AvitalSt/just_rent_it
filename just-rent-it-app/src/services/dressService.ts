import { AddDressDTO } from "@/models/DTOs/AddDressDTO";
import { DressDTO } from "@/models/DTOs/DressDTO";
import { DressListDTO } from "@/models/DTOs/DressListDTO";
import { DressOwnerDTO } from "@/models/DTOs/DressOwnerDTO";
import { PagedResultDTO } from "@/models/DTOs/PagedResultDTO";
import { UpdateDressDTO } from "@/models/DTOs/UpdateDressDTO";
import { DressFilterParams } from "@/models/types/dress/DressFilterParams";
import { DressFilters } from "@/models/types/dress/DressFilter.types";
import { buildFilterParams } from "@/utils/filterUtils";
import { DressListPublicDTO } from "@/models/DTOs/DressListPublicDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE_Dresses = "/Dresses";

function authHeader() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}

export async function addDress(dto: AddDressDTO): Promise<DressDTO> {
  const res = await axiosInstance.post(`${API_BASE_Dresses}`, dto, {
    headers: authHeader(),
  });
  return res.data.data;
}

export function mapFiltersToParams(filters: DressFilters): DressFilterParams {
  const params = buildFilterParams(filters);
  for (const key in params) {
    if (params[key] === undefined || params[key] === "") delete params[key];
  }
  return params as DressFilterParams;
}

export async function getFilteredDresses(
  params: DressFilterParams
): Promise<PagedResultDTO<DressListDTO>> {
  const res = await axiosInstance.get(`${API_BASE_Dresses}`, {
    params,
    headers: authHeader(),
  });
  return res.data.data;
}

export async function getDressByIdPublic(id: number): Promise<DressDTO> {
  const res = await axiosInstance.get(`${API_BASE_Dresses}/${id}`);
  return res.data.data;
}

export async function getDressById(id: number): Promise<DressDTO> {
  const res = await axiosInstance.get(`${API_BASE_Dresses}/${id}`, {
    headers: authHeader(),
  });
  return res.data.data;
}

export async function updateDress(
  id: number,
  dto: UpdateDressDTO
): Promise<DressDTO> {
  const res = await axiosInstance.patch(`${API_BASE_Dresses}/${id}`, dto, {
    headers: authHeader(),
  });
  return res.data.data;
}

export async function activateDress(id: number): Promise<DressDTO> {
  const res = await axiosInstance.patch(
    `${API_BASE_Dresses}/${id}/activate`,
    {},
    { headers: authHeader() }
  );
  return res.data.data;
}

export async function deleteDress(id: number): Promise<void> {
  await axiosInstance.delete(`${API_BASE_Dresses}/${id}`, {
    headers: authHeader(),
  });
}

export async function getDressOwner(dressId: number): Promise<DressOwnerDTO> {
  const res = await axiosInstance.get(`${API_BASE_Dresses}/${dressId}/owner`, {
    headers: authHeader(),
  });
  return res.data.data;
}

export async function getMostViewedDresses(): Promise<DressListPublicDTO[]> {
  const res = await axiosInstance.get(`${API_BASE_Dresses}/most-viewed`);
  return res.data.data;
}
