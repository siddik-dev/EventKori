import type { BookingStatus, EventStatus } from "../types/api";

export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5000/api";

export const EVENT_TYPES = ["Wedding", "Birthday", "Corporate", "Graduation", "Engagement", "Conference", "Celebration"];

export const EVENT_STATUSES: Array<Exclude<EventStatus, number>> = [
  "Planning",
  "Booked",
  "InProgress",
  "Completed",
  "Cancelled"
];

export const BOOKING_STATUSES: Array<Exclude<BookingStatus, number>> = [
  "Pending",
  "Confirmed",
  "InProgress",
  "Completed",
  "Cancelled",
  "Refunded"
];

export const USER_TYPE = {
  Customer: 0,
  ServiceProvider: 1
} as const;
