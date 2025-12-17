import { AddDressDTO } from "@/models/DTOs/AddDressDTO";
import { DressDTO } from "@/models/DTOs/DressDTO";
import { DressListDTO } from "@/models/DTOs/DressListDTO";
import { DressOwnerDTO } from "@/models/DTOs/DressOwnerDTO";
import { PagedResultDTO } from "@/models/DTOs/PagedResultDTO";
import { UpdateDressDTO } from "@/models/DTOs/UpdateDressDTO";
import { DressFilterParams } from "@/models/types/dress/DressFilterParams";
import { DressFilters } from "@/models/types/dress/DressFilter.types";
import { buildFilterParams } from "@/utils/filterUtils";
import axios from "axios";
import { DressListPublicDTO } from "@/models/DTOs/DressListPublicDTO";

const API_BASE = process.env.NEXT_PUBLIC_API_BASE_URL;
const API_BASE_Dresses = `${API_BASE}/Dresses`;

function getToken() {
  return localStorage.getItem("token");
}

export async function addDress(dto: AddDressDTO): Promise<DressDTO> {
  const token = getToken();

  const res = await axios.post(`${API_BASE_Dresses}`, dto, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data.data;
}

export function mapFiltersToParams(filters: DressFilters): DressFilterParams {
  const params = buildFilterParams(filters);
  for (const key in params) {
    if (params[key] === undefined || params[key] === "") {
      delete params[key];
    }
  }
  return params as DressFilterParams;
}

export async function getFilteredDresses(
  params: DressFilterParams
): Promise<PagedResultDTO<DressListDTO>> {
  const token = getToken();

  const headers: Record<string, string> = {
    "Content-Type": "application/json",
  };
  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }
  const res = await axios.get(`${API_BASE_Dresses}`, {
    params,
    headers,
  });

  return res.data.data;
}

export async function getDressByIdFirst(id: number): Promise<DressDTO> {
  const res = await axios.get(`${API_BASE_Dresses}/${id}`);
  return res.data.data;
}

export async function getDressById(id: number): Promise<DressDTO> {
  const token = getToken();

  const res = await axios.get(`${API_BASE_Dresses}/${id}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
  return res.data.data;
}

export async function updateDress(
  id: number,
  dto: UpdateDressDTO
): Promise<DressDTO> {
  const token = getToken();

  const res = await axios.patch(`${API_BASE_Dresses}/${id}`, dto, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data.data;
}

export async function activateDress(id: number): Promise<DressDTO> {
  const token = getToken();

  const res = await axios.patch(
    `${API_BASE_Dresses}/${id}/activate`,
    {},
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  return res.data.data;
}

export async function deleteDress(id: number): Promise<void> {
  const token = getToken();

  await axios.delete(`${API_BASE_Dresses}/${id}`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
}

export async function getDressOwner(dressId: number): Promise<DressOwnerDTO> {
  const token = getToken();

  const res = await axios.get(`${API_BASE_Dresses}/${dressId}/owner`, {
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  return res.data.data;
}

export async function getMostViewedDresses(): Promise<DressListPublicDTO[]> {
  const res = await axios.get(`${API_BASE_Dresses}/most-viewed`);
  return res.data.data;
}
