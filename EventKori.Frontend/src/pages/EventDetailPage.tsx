import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Edit, Send, Trash2 } from "lucide-react";
import { useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { toast } from "sonner";
import { createBooking } from "../api/bookings";
import { deleteEvent, getEvent } from "../api/events";
import { Badge } from "../components/ui/Badge";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { ConfirmDialog } from "../components/ui/ConfirmDialog";
import { Field, TextArea } from "../components/ui/Field";
import { PageHeader } from "../components/ui/PageHeader";
import { CardSkeleton } from "../components/ui/Skeleton";
import { Timeline } from "../components/ui/Timeline";
import { useCurrentProvider } from "../hooks/useCurrentProvider";
import { formatCurrency, formatDateTime } from "../lib/format";
import { apiErrorMessage } from "../lib/http";
import { badgeTone, bookingStatusLabel, eventStatusLabel, userTypeLabel } from "../lib/status";
import { useAuthStore } from "../store/authStore";

export function EventDetailPage() {
  const { id } = useParams();
  const eventId = Number(id);
  const user = useAuthStore((state) => state.user);
  const role = user ? userTypeLabel(user.userType) : "Customer";
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [deleteOpen, setDeleteOpen] = useState(false);
  const [amount, setAmount] = useState("");
  const [notes, setNotes] = useState("");
  const { currentProvider } = useCurrentProvider();
  const eventQuery = useQuery({ queryKey: ["events", eventId], queryFn: () => getEvent(eventId), enabled: Number.isFinite(eventId) });
  const deleteMutation = useMutation({
    mutationFn: () => deleteEvent(eventId),
    onSuccess: () => {
      toast.success("Event deleted.");
      queryClient.invalidateQueries({ queryKey: ["events"] });
      navigate("/events");
    },
    onError: (error) => toast.error(apiErrorMessage(error, "Could not delete event."))
  });
  const proposalMutation = useMutation({
    mutationFn: () =>
      createBooking({
        eventId,
        serviceProviderId: currentProvider?.id ?? 0,
        totalAmount: Number(amount),
        packageId: null,
        specialRequirements: eventQuery.data?.specialRequirements ?? "",
        notes
      }),
    onSuccess: (booking) => {
      toast.success("Proposal sent.");
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
      navigate(`/bookings/${booking.id}`);
    },
    onError: (error) => toast.error(apiErrorMessage(error, "Could not send proposal."))
  });

  if (eventQuery.isLoading || !eventQuery.data) return <CardSkeleton />;
  const event = eventQuery.data;

  return (
    <div className="grid gap-6">
      <PageHeader
        title={event.title}
        description={`${event.eventType} - ${event.location}`}
        action={
          role === "Customer" && (
            <div className="flex gap-2">
              <Link to={`/events/${event.id}/edit`}>
                <Button variant="secondary" icon={<Edit className="size-4" />}>
                  Edit
                </Button>
              </Link>
              <Button variant="danger" onClick={() => setDeleteOpen(true)} icon={<Trash2 className="size-4" />}>
                Delete
              </Button>
            </div>
          )
        }
      />
      <div className="grid gap-6 lg:grid-cols-[1fr_360px]">
        <div className="grid gap-6">
          <Card>
            <div className="mb-4 flex flex-wrap gap-3">
              <Badge tone={badgeTone(eventStatusLabel(event.status))}>{eventStatusLabel(event.status)}</Badge>
              <Badge tone="neutral">{`${event.quotationCount} quotations`}</Badge>
            </div>
            <p className="leading-7 text-slate-700">{event.description || "No description provided."}</p>
            <div className="mt-6 grid gap-4 sm:grid-cols-2">
              <Info label="Event date" value={formatDateTime(event.eventDate)} />
              <Info label="Budget" value={formatCurrency(event.budget)} />
              <Info label="Guests" value={`${event.attendeesCount}`} />
              <Info label="Client" value={event.clientName || "Customer"} />
            </div>
            {event.specialRequirements && <p className="mt-5 rounded-md bg-slate-50 p-4 text-sm text-slate-700">{event.specialRequirements}</p>}
          </Card>
          <Card>
            <h2 className="mb-4 font-semibold text-ink">Associated bookings</h2>
            <div className="grid gap-3">
              {event.quotations.map((booking) => (
                <Link key={booking.id} to={`/bookings/${booking.id}`} className="rounded-md border border-line p-3 hover:bg-slate-50">
                  <div className="flex items-center justify-between gap-3">
                    <p className="font-semibold text-ink">{booking.providerName}</p>
                    <Badge tone={badgeTone(bookingStatusLabel(booking.status))}>{bookingStatusLabel(booking.status)}</Badge>
                  </div>
                  <p className="mt-1 text-sm text-muted">{formatCurrency(booking.totalAmount)}</p>
                </Link>
              ))}
              {event.quotations.length === 0 && <p className="text-sm text-muted">No bookings or proposals yet.</p>}
            </div>
          </Card>
        </div>
        <div className="grid h-fit gap-6">
          <Card>
            <h2 className="mb-4 font-semibold text-ink">Timeline</h2>
            <Timeline
              items={[
                { label: "Request created", detail: event.clientName || "Customer submitted request" },
                { label: eventStatusLabel(event.status), detail: `${event.quotationCount} quotation updates` }
              ]}
            />
          </Card>
          {role === "ServiceProvider" && (
            <Card>
              <h2 className="font-semibold text-ink">Send proposal</h2>
              {!currentProvider ? (
                <p className="mt-3 text-sm text-muted">Create your provider profile first from the profile page.</p>
              ) : (
                <div className="mt-4 grid gap-4">
                  <Field label="Quotation amount" type="number" value={amount} onChange={(event) => setAmount(event.target.value)} />
                  <TextArea label="Notes" value={notes} onChange={(event) => setNotes(event.target.value)} />
                  <Button disabled={!amount || proposalMutation.isPending} onClick={() => proposalMutation.mutate()} icon={<Send className="size-4" />}>
                    {proposalMutation.isPending ? "Sending..." : "Send proposal"}
                  </Button>
                </div>
              )}
            </Card>
          )}
        </div>
      </div>
      <ConfirmDialog
        open={deleteOpen}
        title="Delete event request?"
        description="This removes the event request from your workspace."
        confirmLabel="Delete event"
        busy={deleteMutation.isPending}
        onClose={() => setDeleteOpen(false)}
        onConfirm={() => deleteMutation.mutate()}
      />
    </div>
  );
}

function Info({ label, value }: { label: string; value: string }) {
  return (
    <div className="rounded-md border border-line p-3">
      <p className="text-xs font-semibold uppercase text-muted">{label}</p>
      <p className="mt-1 font-semibold text-ink">{value}</p>
    </div>
  );
}
