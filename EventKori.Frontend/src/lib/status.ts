import type { BookingStatus, EventStatus, UserType } from "../types/api";

const eventStatusMap = ["Planning", "Booked", "InProgress", "Completed", "Cancelled"] as const;
const bookingStatusMap = ["Pending", "Confirmed", "InProgress", "Completed", "Cancelled", "Refunded"] as const;
const userTypeMap = ["Customer", "ServiceProvider", "Admin"] as const;

export function eventStatusLabel(status: EventStatus) {
  return typeof status === "number" ? eventStatusMap[status] ?? "Planning" : status;
}

export function bookingStatusLabel(status: BookingStatus) {
  return typeof status === "number" ? bookingStatusMap[status] ?? "Pending" : status;
}

export function userTypeLabel(type: UserType) {
  return typeof type === "number" ? userTypeMap[type] ?? "Customer" : type;
}

export function eventStatusValue(status: EventStatus) {
  const label = eventStatusLabel(status);
  return eventStatusMap.indexOf(label as (typeof eventStatusMap)[number]);
}

export function bookingStatusValue(status: BookingStatus) {
  const label = bookingStatusLabel(status);
  return bookingStatusMap.indexOf(label as (typeof bookingStatusMap)[number]);
}

export function badgeTone(status: string) {
  if (["Confirmed", "Booked", "Completed"].includes(status)) return "success";
  if (["InProgress", "Planning", "Pending"].includes(status)) return "info";
  if (["Cancelled", "Refunded"].includes(status)) return "danger";
  return "neutral";
}
