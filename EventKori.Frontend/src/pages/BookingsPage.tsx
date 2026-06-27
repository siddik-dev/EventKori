import { useQuery } from "@tanstack/react-query";
import { Filter } from "lucide-react";
import { useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { getBookings } from "../api/bookings";
import { Badge } from "../components/ui/Badge";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { EmptyState } from "../components/ui/EmptyState";
import { SelectField } from "../components/ui/Field";
import { PageHeader } from "../components/ui/PageHeader";
import { CardSkeleton } from "../components/ui/Skeleton";
import { BOOKING_STATUSES } from "../lib/constants";
import { formatCurrency, formatDate } from "../lib/format";
import { badgeTone, bookingStatusLabel, userTypeLabel } from "../lib/status";
import { useAuthStore } from "../store/authStore";

export function BookingsPage() {
  const user = useAuthStore((state) => state.user);
  const role = user ? userTypeLabel(user.userType) : "Customer";
  const [status, setStatus] = useState("");
  const { data, isLoading } = useQuery({ queryKey: ["bookings"], queryFn: getBookings });
  const filtered = useMemo(() => (status ? data?.filter((booking) => bookingStatusLabel(booking.status) === status) : data), [data, status]);

  return (
    <div className="grid gap-6">
      <PageHeader
        title={role === "ServiceProvider" ? "Booking requests" : "My bookings"}
        description={role === "ServiceProvider" ? "Accept, reject, complete, or cancel proposals." : "Track provider proposals and booking status."}
      />
      <Card>
        <div className="grid gap-4 sm:grid-cols-[260px_auto]">
          <SelectField label="Status" value={status} onChange={(event) => setStatus(event.target.value)}>
            <option value="">All statuses</option>
            {BOOKING_STATUSES.map((item) => (
              <option key={item} value={item}>
                {item}
              </option>
            ))}
          </SelectField>
          <div className="flex items-end">
            <Button variant="secondary" icon={<Filter className="size-4" />} onClick={() => setStatus("")}>
              Clear filter
            </Button>
          </div>
        </div>
      </Card>
      {isLoading ? (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {Array.from({ length: 6 }).map((_, index) => (
            <CardSkeleton key={index} />
          ))}
        </div>
      ) : filtered?.length ? (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {filtered.map((booking) => (
            <Link key={booking.id} to={`/bookings/${booking.id}`}>
              <Card className="h-full hover:shadow-soft">
                <div className="flex items-start justify-between gap-3">
                  <div>
                    <h2 className="font-semibold text-ink">{booking.eventTitle}</h2>
                    <p className="text-sm text-muted">{booking.providerName}</p>
                  </div>
                  <Badge tone={badgeTone(bookingStatusLabel(booking.status))}>{bookingStatusLabel(booking.status)}</Badge>
                </div>
                <div className="mt-5 grid gap-2 text-sm text-muted">
                  <span>Event: {formatDate(booking.eventDate)}</span>
                  <span>Booked: {formatDate(booking.bookingDate)}</span>
                  <span className="font-semibold text-ink">{formatCurrency(booking.totalAmount)}</span>
                </div>
              </Card>
            </Link>
          ))}
        </div>
      ) : (
        <EmptyState title="No bookings found" description="Matching bookings will appear here once requests or proposals are created." />
      )}
    </div>
  );
}
