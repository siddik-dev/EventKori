import { useQuery } from "@tanstack/react-query";
import { CalendarDays, CircleDollarSign, Clock, ShieldCheck } from "lucide-react";
import { Link } from "react-router-dom";
import { getDashboard } from "../api/dashboard";
import { Badge } from "../components/ui/Badge";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { CardSkeleton } from "../components/ui/Skeleton";
import { PageHeader } from "../components/ui/PageHeader";
import { StatCard } from "../components/ui/StatCard";
import { formatCurrency, formatDate } from "../lib/format";
import { badgeTone, bookingStatusLabel, eventStatusLabel, userTypeLabel } from "../lib/status";
import { useAuthStore } from "../store/authStore";

export function DashboardPage() {
  const user = useAuthStore((state) => state.user);
  const role = user ? userTypeLabel(user.userType) : "Customer";
  const { data, isLoading } = useQuery({ queryKey: ["dashboard"], queryFn: getDashboard });

  return (
    <div className="grid gap-6">
      <PageHeader
        title={`Welcome, ${user?.firstName || "there"}`}
        description={role === "ServiceProvider" ? "Review event demand and manage incoming quotations." : "Track your events, quotations, and upcoming bookings."}
        action={
          <div className="flex gap-2">
            {role === "Customer" && (
              <Link to="/events/new">
                <Button>Create event</Button>
              </Link>
            )}
            <Link to="/providers">
              <Button variant="secondary">Browse providers</Button>
            </Link>
          </div>
        }
      />
      {isLoading || !data ? (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
          {Array.from({ length: 4 }).map((_, index) => (
            <CardSkeleton key={index} />
          ))}
        </div>
      ) : (
        <>
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-4">
            <StatCard label="Total events" value={data.totalEventRequests} icon={<CalendarDays className="size-5" />} />
            <StatCard label="Active events" value={data.activeEventRequests} icon={<Clock className="size-5" />} />
            <StatCard label="Verified providers" value={data.verifiedOrganizers} icon={<ShieldCheck className="size-5" />} />
            <StatCard label="Quoted value" value={formatCurrency(data.totalQuotedValue)} icon={<CircleDollarSign className="size-5" />} />
          </div>
          <div className="grid gap-4 lg:grid-cols-[1fr_0.85fr]">
            <Card>
              <div className="mb-4 flex items-center justify-between">
                <h2 className="font-semibold text-ink">Recent event requests</h2>
                <Link to="/events" className="text-sm font-semibold text-brand-700">
                  View all
                </Link>
              </div>
              <div className="grid gap-3">
                {data.recentRequests.map((event) => (
                  <Link key={event.id} to={`/events/${event.id}`} className="rounded-md border border-line p-3 hover:bg-slate-50">
                    <div className="flex items-center justify-between gap-3">
                      <p className="font-semibold text-ink">{event.title}</p>
                      <Badge tone={badgeTone(eventStatusLabel(event.status))}>{eventStatusLabel(event.status)}</Badge>
                    </div>
                    <p className="mt-1 text-sm text-muted">
                      {event.eventType} in {event.location} on {formatDate(event.eventDate)}
                    </p>
                  </Link>
                ))}
              </div>
            </Card>
            <Card>
              <div className="mb-4 flex items-center justify-between">
                <h2 className="font-semibold text-ink">Latest bookings</h2>
                <Link to="/bookings" className="text-sm font-semibold text-brand-700">
                  View all
                </Link>
              </div>
              <div className="grid gap-3">
                {data.latestQuotations.map((booking) => (
                  <Link key={booking.id} to={`/bookings/${booking.id}`} className="rounded-md border border-line p-3 hover:bg-slate-50">
                    <div className="flex items-center justify-between gap-3">
                      <p className="font-semibold text-ink">{booking.providerName || booking.eventTitle}</p>
                      <Badge tone={badgeTone(bookingStatusLabel(booking.status))}>{bookingStatusLabel(booking.status)}</Badge>
                    </div>
                    <p className="mt-1 text-sm text-muted">{formatCurrency(booking.totalAmount)}</p>
                  </Link>
                ))}
              </div>
            </Card>
          </div>
        </>
      )}
    </div>
  );
}
