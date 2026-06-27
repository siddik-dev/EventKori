import { http } from "../lib/http";
import type { CreateEventRequest, EventRequest, EventRequestDetail, EventStatus, UpdateEventRequest } from "../types/api";

export interface EventFilters {
  eventType?: string;
  status?: EventStatus | "";
}

export async function getEvents(filters: EventFilters = {}) {
  const { data } = await http.get<EventRequest[]>("/event-requests", { params: filters });
  return data;
}

export async function getEvent(id: number) {
  const { data } = await http.get<EventRequestDetail>(`/event-requests/${id}`);
  return data;
}

export async function createEvent(payload: CreateEventRequest) {
  const { data } = await http.post<EventRequest>("/event-requests", payload);
  return data;
}

export async function updateEvent(id: number, payload: UpdateEventRequest) {
  const { data } = await http.put<EventRequest>(`/event-requests/${id}`, payload);
  return data;
}

export async function deleteEvent(id: number) {
  await http.delete(`/event-requests/${id}`);
}
