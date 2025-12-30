import {
  AgeGroupDTO,
  CityDTO,
  ColorDTO,
  EventTypeDTO,
  SizeDTO,
} from "@/models/DTOs/OptionsDTO";
import { axiosInstance } from "@/services/axiosInstance";

const API_BASE_Options = "/Options";

export async function getColors(): Promise<ColorDTO[]> {
  const response = await axiosInstance.get<{ data: ColorDTO[] }>(
    `${API_BASE_Options}/colors`
  );
  return response.data.data;
}

export async function getSizes(): Promise<SizeDTO[]> {
  const response = await axiosInstance.get<{ data: SizeDTO[] }>(
    `${API_BASE_Options}/sizes`
  );
  return response.data.data;
}

export async function getCities(): Promise<CityDTO[]> {
  const response = await axiosInstance.get<{ data: CityDTO[] }>(
    `${API_BASE_Options}/cities`
  );
  return response.data.data;
}

export async function getAgeGroups(): Promise<AgeGroupDTO[]> {
  const response = await axiosInstance.get<{ data: AgeGroupDTO[] }>(
    `${API_BASE_Options}/age-groups`
  );
  return response.data.data;
}

export async function getEventTypes(): Promise<EventTypeDTO[]> {
  const response = await axiosInstance.get<{ data: EventTypeDTO[] }>(
    `${API_BASE_Options}/event-types`
  );
  return response.data.data;
}

const optionsAPI = {
  getColors,
  getSizes,
  getCities,
  getAgeGroups,
  getEventTypes,
};

export default optionsAPI;
