import { http } from "../lib/http";
import type { CreateServiceProvider, ServiceProvider, ServiceProviderDetail } from "../types/api";

export interface ProviderFilters {
  search?: string;
  location?: string;
  verifiedOnly?: boolean;
}

export async function getProviders(filters: ProviderFilters = {}) {
  const { data } = await http.get<ServiceProvider[]>("/service-providers", { params: filters });
  return data;
}

export async function getFeaturedProviders(count = 6) {
  const { data } = await http.get<ServiceProvider[]>("/service-providers/featured", { params: { count } });
  return data;
}

export async function getProvider(id: number) {
  const { data } = await http.get<ServiceProviderDetail>(`/service-providers/${id}`);
  return data;
}

export async function createProvider(payload: CreateServiceProvider) {
  const { data } = await http.post<ServiceProvider>("/service-providers", payload);
  return data;
}
