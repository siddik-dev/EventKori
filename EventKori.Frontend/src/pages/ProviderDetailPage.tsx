import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { CalendarPlus, CheckCircle2, Star } from "lucide-react";
import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { toast } from "sonner";
import { createBooking } from "../api/bookings";
import { getEvents } from "../api/events";
import { getProvider } from "../api/providers";
import { Badge } from "../components/ui/Badge";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { SelectField, TextArea } from "../components/ui/Field";
import { PageHeader } from "../components/ui/PageHeader";
import { CardSkeleton } from "../components/ui/Skeleton";
import { apiErrorMessage } from "../lib/http";
import { formatCurrency, formatDate } from "../lib/format";
import { eventStatusLabel, userTypeLabel } from "../lib/status";
import { useAuthStore } from "../store/authStore";

export function ProviderDetailPage() {
  const { id } = useParams();
  const providerId = Number(id);
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const user = useAuthStore((state) => state.user);
  const [eventId, setEventId] = useState("");
  const [packageId, setPackageId] = useState("");
  const [notes, setNotes] = useState("");
  const providerQuery = useQuery({ queryKey: ["providers", providerId], queryFn: () => getProvider(providerId), enabled: Number.isFinite(providerId) });
  const eventsQuery = useQuery({ queryKey: ["events", "booking-options"], queryFn: () => getEvents(), enabled: user ? userTypeLabel(user.userType) === "Customer" : false });
  const bookingMutation = useMutation({
    mutationFn: () => {
      const provider = providerQuery.data;
      const selectedPackage = provider?.packages.find((item) => item.id === Number(packageId));
      return createBooking({
        eventId: Number(eventId),
        serviceProviderId: providerId,
        packageId: packageId ? Number(packageId) : null,
        totalAmount: selectedPackage?.price ?? provider?.startingPrice ?? 0,
        specialRequirements: notes,
        notes: selectedPackage ? `Requested package: ${selectedPackage.packageName}` : "Requested from provider profile"
      });
    },
    onSuccess: (booking) => {
      toast.success("Booking request created.");
      queryClient.invalidateQueries({ queryKey: ["bookings"] });
      navigate(`/bookings/${booking.id}`);
    },
    onError: (error) => toast.error(apiErrorMessage(error, "Could not create booking."))
  });

  if (providerQuery.isLoading || !providerQuery.data) return <CardSkeleton />;
  const provider = providerQuery.data;
  const isCustomer = user ? userTypeLabel(user.userType) === "Customer" : false;

  return (
    <div className="grid gap-6">
      <PageHeader title={provider.companyName} description={`${provider.servicesOffered} · ${provider.location}`} />
      <div className="grid gap-6 lg:grid-cols-[1fr_360px]">
        <div className="grid gap-6">
          <Card>
            <img
              src={provider.coverImageUrl || provider.portfolioItems[0]?.imageUrl || "https://images.unsplash.com/photo-1519225421980-715cb0215aed?auto=format&fit=crop&w=1200&q=80"}
              alt={provider.companyName}
              className="h-72 w-full rounded-md object-cover"
            />
            <div className="mt-5 flex flex-wrap items-center gap-3">
              {provider.isVerified && <Badge tone="success">Verified</Badge>}
              <span className="flex items-center gap-1 text-sm font-semibold text-ink">
                <Star className="size-4 fill-amber-400 text-amber-400" />
                {provider.rating.toFixed(1)} · {provider.totalReviews} reviews
              </span>
              <span className="text-sm text-muted">{provider.yearsOfExperience} years experience</span>
            </div>
            <p className="mt-4 leading-7 text-slate-700">{provider.description}</p>
          </Card>
          <section>
            <h2 className="mb-3 font-semibold text-ink">Packages</h2>
            <div className="grid gap-4 sm:grid-cols-2">
              {provider.packages.map((pkg) => (
                <Card key={pkg.id}>
                  <div className="flex items-center justify-between gap-3">
                    <h3 className="font-semibold text-ink">{pkg.packageName}</h3>
                    {pkg.isPopular && <Badge tone="info">Popular</Badge>}
                  </div>
                  <p className="mt-2 text-sm text-muted">{pkg.description}</p>
                  <p className="mt-4 text-xl font-bold text-ink">{formatCurrency(pkg.price)}</p>
                  <ul className="mt-4 grid gap-2 text-sm text-slate-700">
                    {pkg.features.map((feature) => (
                      <li key={feature} className="flex gap-2">
                        <CheckCircle2 className="size-4 text-accent-500" />
                        {feature}
                      </li>
                    ))}
                  </ul>
                </Card>
              ))}
            </div>
          </section>
          <section>
            <h2 className="mb-3 font-semibold text-ink">Portfolio</h2>
            <div className="grid gap-4 sm:grid-cols-2">
              {provider.portfolioItems.map((item) => (
                <Card key={item.id}>
                  <img src={item.imageUrl} alt={item.title} className="h-40 w-full rounded-md object-cover" />
                  <h3 className="mt-4 font-semibold text-ink">{item.title}</h3>
                  <p className="text-sm text-muted">
                    {item.category} · {formatDate(item.eventDate)}
                  </p>
                  <p className="mt-2 text-sm text-slate-700">{item.description}</p>
                </Card>
              ))}
            </div>
          </section>
          <section>
            <h2 className="mb-3 font-semibold text-ink">Reviews</h2>
            <div className="grid gap-3">
              {provider.reviews.map((review) => (
                <Card key={review.id}>
                  <div className="flex items-center justify-between">
                    <p className="font-semibold text-ink">{review.reviewerName}</p>
                    <span className="text-sm font-semibold text-amber-600">{review.rating}/5</span>
                  </div>
                  <p className="mt-2 text-sm text-slate-700">{review.comment}</p>
                  {review.response && <p className="mt-3 rounded-md bg-slate-50 p-3 text-sm text-muted">Provider response: {review.response}</p>}
                </Card>
              ))}
            </div>
          </section>
        </div>
        <Card className="h-fit">
          <h2 className="font-semibold text-ink">Book service</h2>
          <p className="mt-2 text-sm text-muted">Choose one of your event requests and send a booking request.</p>
          {isCustomer ? (
            <div className="mt-4 grid gap-4">
              <SelectField label="Event request" value={eventId} onChange={(event) => setEventId(event.target.value)}>
                <option value="">Select event</option>
                {eventsQuery.data?.map((event) => (
                  <option key={event.id} value={event.id}>
                    {event.title} · {eventStatusLabel(event.status)}
                  </option>
                ))}
              </SelectField>
              <SelectField label="Package" value={packageId} onChange={(event) => setPackageId(event.target.value)}>
                <option value="">Starting price</option>
                {provider.packages.map((pkg) => (
                  <option key={pkg.id} value={pkg.id}>
                    {pkg.packageName} · {formatCurrency(pkg.price)}
                  </option>
                ))}
              </SelectField>
              <TextArea label="Notes" value={notes} onChange={(event) => setNotes(event.target.value)} placeholder="Special requirements or preferred contact time" />
              <Button disabled={!eventId || bookingMutation.isPending} onClick={() => bookingMutation.mutate()} icon={<CalendarPlus className="size-4" />}>
                {bookingMutation.isPending ? "Sending..." : "Create booking"}
              </Button>
            </div>
          ) : (
            <p className="mt-4 rounded-md bg-slate-50 p-3 text-sm text-muted">Customer accounts can create bookings from provider profiles.</p>
          )}
        </Card>
      </div>
    </div>
  );
}
