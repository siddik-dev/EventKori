import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Save, Trash2 } from "lucide-react";
import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { toast } from "sonner";
import { deleteBooking, getBooking, updateBookingStatus } from "../api/bookings";
import { Badge } from "../components/ui/Badge";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { ConfirmDialog } from "../components/ui/ConfirmDialog";
import { SelectField, TextArea } from "../components/ui/Field";
import { PageHeader } from "../components/ui/PageHeader";
import { CardSkeleton } from "../components/ui/Skeleton";
import { Timeline } from "../components/ui/Timeline";
import { BOOKING_STATUSES } from "../lib/constants";
import { formatCurrency, formatDateTime } from "../lib/format";
import { apiErrorMessage } from "../lib/http";
import { badgeTone, bookingStatusLabel, userTypeLabel } from "../lib/status";
import { useAuthStore } from "../store/authStore";
import type { BookingStatus } from "../types/api";

export function BookingDetailPage() {
  const { id } = useParams();
  const bookingId = Number(id);
  const user = useAuthStore((state) => state.user);
  const role = user ? userTypeLabel(user.userType) : "Customer";
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const [deleteOpen, setDeleteOpen] = useState(false);
  const bookingQuery = useQuery({ queryKey: ["bookings", bookingId], queryFn: () => getBooking(bookingId), enabled: Number.isFinite(bookingId) });
  const [status, setStatus] = useState<BookingStatus | "">("");
  const [paymentStatus, setPaymentStatus] = useState("");
  const [notes, setNotes] = useState("");

  const updateMutation = useMutation({
    mutationFn: () => updateBookingStatus(bookingId, { status: status || bookingQuery.data?.status || "Pending", paymentStatus, notes }),
    onSuccess: (booking) => {
      toast.success("Booking updated.");
      setStatus(bookingStatusLabel(booking.status));
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
    },
    onError: (error) => toast.error(apiErrorMessage(error, "Could not update booking."))
  });
  const deleteMutation = useMutation({
    mutationFn: () => deleteBooking(bookingId),
    onSuccess: () => {
      toast.success(role === "Customer" ? "Booking cancelled." : "Booking removed.");
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
      navigate("/bookings");
    },
    onError: (error) => toast.error(apiErrorMessage(error, "Could not cancel booking."))
  });

  const booking = bookingQuery.data;
  if (bookingQuery.isLoading || !booking) return <CardSkeleton />;
  const effectiveStatus = status || bookingStatusLabel(booking.status);

  return (
    <div className="grid gap-6">
      <PageHeader
        title={booking.eventTitle}
        description={`${booking.providerName} - ${formatCurrency(booking.totalAmount)}`}
        action={
          <Button variant="danger" onClick={() => setDeleteOpen(true)} icon={<Trash2 className="size-4" />}>
            {role === "Customer" ? "Cancel booking" : "Delete"}
          </Button>
        }
      />
      <div className="grid gap-6 lg:grid-cols-[1fr_360px]">
        <Card>
          <div className="mb-4 flex flex-wrap gap-3">
            <Badge tone={badgeTone(bookingStatusLabel(booking.status))}>{bookingStatusLabel(booking.status)}</Badge>
            <Badge tone="neutral">{booking.paymentStatus || "Payment pending"}</Badge>
          </div>
          <div className="grid gap-4 sm:grid-cols-2">
            <Info label="Provider" value={booking.providerName} />
            <Info label="Package" value={booking.packageName || "Custom quotation"} />
            <Info label="Event date" value={formatDateTime(booking.eventDate)} />
            <Info label="Booking date" value={formatDateTime(booking.bookingDate)} />
            <Info label="Amount" value={formatCurrency(booking.totalAmount)} />
            <Info label="Payment" value={booking.paymentStatus || "Pending"} />
          </div>
          {booking.specialRequirements && <p className="mt-5 rounded-md bg-slate-50 p-4 text-sm text-slate-700">{booking.specialRequirements}</p>}
          {booking.notes && <p className="mt-3 rounded-md bg-slate-50 p-4 text-sm text-slate-700">{booking.notes}</p>}
        </Card>
        <div className="grid h-fit gap-6">
          <Card>
            <h2 className="mb-4 font-semibold text-ink">Status timeline</h2>
            <Timeline
              items={[
                { label: "Booking created", detail: formatDateTime(booking.bookingDate) },
                { label: bookingStatusLabel(booking.status), detail: booking.notes || "Current booking status" }
              ]}
            />
          </Card>
          <Card>
            <h2 className="font-semibold text-ink">Update status</h2>
            <div className="mt-4 grid gap-4">
              <SelectField label="Status" value={effectiveStatus} onChange={(event) => setStatus(event.target.value as BookingStatus)}>
                {BOOKING_STATUSES.map((item) => (
                  <option key={item} value={item}>
                    {item}
                  </option>
                ))}
              </SelectField>
              <SelectField label="Payment status" value={paymentStatus || booking.paymentStatus || "Pending"} onChange={(event) => setPaymentStatus(event.target.value)}>
                <option value="Pending">Pending</option>
                <option value="Paid">Paid</option>
                <option value="Refunded">Refunded</option>
              </SelectField>
              <TextArea label="Notes" value={notes || booking.notes} onChange={(event) => setNotes(event.target.value)} />
              <Button onClick={() => updateMutation.mutate()} disabled={updateMutation.isPending} icon={<Save className="size-4" />}>
                {updateMutation.isPending ? "Updating..." : "Save status"}
              </Button>
            </div>
          </Card>
        </div>
      </div>
      <ConfirmDialog
        open={deleteOpen}
        title={role === "Customer" ? "Cancel booking?" : "Delete booking?"}
        description="This action cannot be undone."
        confirmLabel={role === "Customer" ? "Cancel booking" : "Delete booking"}
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
