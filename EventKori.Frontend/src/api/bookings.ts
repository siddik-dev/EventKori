import { http } from "../lib/http";
import type { Booking, CreateBooking, UpdateBookingStatus } from "../types/api";

export async function getBookings() {
  const { data } = await http.get<Booking[]>("bookings");
  return data;
}

export async function getBooking(id: number) {
  const { data } = await http.get<Booking>(`bookings/${id}`);
  return data;
}

export async function createBooking(payload: CreateBooking) {
  const { data } = await http.post<Booking>("bookings", payload);
  return data;
}

export async function updateBookingStatus(id: number, payload: UpdateBookingStatus) {
  const { data } = await http.patch<Booking>(`bookings/${id}/status`, payload);
  return data;
}

export async function deleteBooking(id: number) {
  await http.delete(`bookings/${id}`);
}
