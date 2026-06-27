import { useQuery } from "@tanstack/react-query";
import { CalendarPlus, Filter } from "lucide-react";
import { useState } from "react";
import { Link } from "react-router-dom";
import { getEvents } from "../api/events";
import { Badge } from "../components/ui/Badge";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { EmptyState } from "../components/ui/EmptyState";
import { SelectField } from "../components/ui/Field";
import { PageHeader } from "../components/ui/PageHeader";
import { CardSkeleton } from "../components/ui/Skeleton";
import { EVENT_STATUSES, EVENT_TYPES } from "../lib/constants";
import { formatCurrency, formatDate } from "../lib/format";
import { badgeTone, eventStatusLabel, userTypeLabel } from "../lib/status";
import { useAuthStore } from "../store/authStore";
import type { EventStatus } from "../types/api";

export function EventsPage() {
  const user = useAuthStore((state) => state.user);
  const role = user ? userTypeLabel(user.userType) : "Customer";
  const [eventType, setEventType] = useState("");
  const [status, setStatus] = useState<EventStatus | "">("");
  const { data, isLoading } = useQuery({
    queryKey: ["events", { eventType, status }],
    queryFn: () => getEvents({ eventType: eventType || undefined, status: status || undefined })
  });

  return (
    <div className="grid gap-6">
      <PageHeader
        title={role === "ServiceProvider" ? "Available event requests" : "My event requests"}
        description={role === "ServiceProvider" ? "Browse customer requests and send proposals." : "Create and manage your planned events."}
        action={
          role === "Customer" && (
            <Link to="/events/new">
              <Button icon={<CalendarPlus className="size-4" />}>Create event</Button>
            </Link>
          )
        }
      />
      <Card>
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-[260px_260px_auto]">
          <SelectField label="Event type" value={eventType} onChange={(event) => setEventType(event.target.value)}>
            <option value="">All types</option>
            {EVENT_TYPES.map((type) => (
              <option key={type} value={type}>
                {type}
              </option>
            ))}
          </SelectField>
          <SelectField label="Status" value={status} onChange={(event) => setStatus(event.target.value as EventStatus | "")}>
            <option value="">All statuses</option>
            {EVENT_STATUSES.map((item) => (
              <option key={item} value={item}>
                {item}
              </option>
            ))}
          </SelectField>
          <div className="flex items-end">
            <Button variant="secondary" icon={<Filter className="size-4" />} onClick={() => (setEventType(""), setStatus(""))}>
              Clear filters
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
      ) : data?.length ? (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {data.map((event) => (
            <Link key={event.id} to={`/events/${event.id}`}>
              <Card className="h-full hover:shadow-soft">
                <div className="flex items-start justify-between gap-3">
                  <div>
                    <h2 className="font-semibold text-ink">{event.title}</h2>
                    <p className="text-sm text-muted">{event.eventType}</p>
                  </div>
                  <Badge tone={badgeTone(eventStatusLabel(event.status))}>{eventStatusLabel(event.status)}</Badge>
                </div>
                <p className="mt-4 line-clamp-3 text-sm text-slate-700">{event.description}</p>
                <div className="mt-5 grid gap-2 text-sm text-muted">
                  <span>{formatDate(event.eventDate)}</span>
                  <span>
                    {event.location} · {event.attendeesCount} guests
                  </span>
                  <span className="font-semibold text-ink">{formatCurrency(event.budget)}</span>
                </div>
              </Card>
            </Link>
          ))}
        </div>
      ) : (
        <EmptyState
          title="No event requests"
          description={role === "Customer" ? "Create your first event request to start receiving proposals." : "No matching customer requests are available right now."}
          action={
            role === "Customer" && (
              <Link to="/events/new">
                <Button>Create event</Button>
              </Link>
            )
          }
        />
      )}
    </div>
  );
}
