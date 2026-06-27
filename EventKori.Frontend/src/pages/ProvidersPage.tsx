import { useQuery } from "@tanstack/react-query";
import { CheckCircle2, Grid2X2, List, Search, Star } from "lucide-react";
import { useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { getFeaturedProviders, getProviders } from "../api/providers";
import { Badge } from "../components/ui/Badge";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { EmptyState } from "../components/ui/EmptyState";
import { Field } from "../components/ui/Field";
import { PageHeader } from "../components/ui/PageHeader";
import { CardSkeleton } from "../components/ui/Skeleton";
import { formatCurrency } from "../lib/format";

export function ProvidersPage() {
  const [search, setSearch] = useState("");
  const [location, setLocation] = useState("");
  const [verifiedOnly, setVerifiedOnly] = useState(false);
  const [view, setView] = useState<"grid" | "list">("grid");
  const filters = { search: search || undefined, location: location || undefined, verifiedOnly };
  const providersQuery = useQuery({ queryKey: ["providers", filters], queryFn: () => getProviders(filters) });
  const featuredQuery = useQuery({ queryKey: ["providers", "featured"], queryFn: () => getFeaturedProviders(6) });

  const suggestions = useMemo(() => {
    const providers = providersQuery.data ?? [];
    return Array.from(new Set(providers.flatMap((provider) => [provider.companyName, provider.location, ...provider.servicesOffered.split(",").map((s) => s.trim())]))).slice(
      0,
      8
    );
  }, [providersQuery.data]);

  return (
    <div className="grid gap-6">
      <PageHeader title="Service providers" description="Search verified organizers, venues, caterers, decorators, and production teams." />
      <Card>
        <div className="grid gap-4 lg:grid-cols-[1fr_220px_auto_auto]">
          <Field label="Search" placeholder="Company, service, description" value={search} onChange={(event) => setSearch(event.target.value)} />
          <Field label="Location" placeholder="Dhaka, Banani" value={location} onChange={(event) => setLocation(event.target.value)} />
          <label className="flex items-end gap-2 pb-2 text-sm font-semibold text-slate-700">
            <input type="checkbox" checked={verifiedOnly} onChange={(event) => setVerifiedOnly(event.target.checked)} className="size-4 rounded border-line" />
            Verified only
          </label>
          <div className="flex items-end gap-2">
            <Button variant={view === "grid" ? "primary" : "secondary"} onClick={() => setView("grid")} icon={<Grid2X2 className="size-4" />}>
              Grid
            </Button>
            <Button variant={view === "list" ? "primary" : "secondary"} onClick={() => setView("list")} icon={<List className="size-4" />}>
              List
            </Button>
          </div>
        </div>
        {suggestions.length > 0 && (
          <div className="mt-4 flex flex-wrap gap-2">
            {suggestions.map((item) => (
              <button key={item} type="button" onClick={() => setSearch(item)} className="rounded-full bg-slate-100 px-3 py-1 text-xs font-semibold text-slate-600">
                {item}
              </button>
            ))}
          </div>
        )}
      </Card>
      {featuredQuery.data && featuredQuery.data.length > 0 && (
        <section>
          <h2 className="mb-3 font-semibold text-ink">Featured providers</h2>
          <div className="flex snap-x gap-4 overflow-x-auto pb-2">
            {featuredQuery.data.map((provider) => (
              <Link key={provider.id} to={`/providers/${provider.id}`} className="min-w-72 snap-start rounded-lg border border-line bg-white p-4 hover:shadow-soft">
                <div className="flex items-center justify-between">
                  <p className="font-semibold text-ink">{provider.companyName}</p>
                  <Star className="size-4 fill-amber-400 text-amber-400" />
                </div>
                <p className="mt-1 text-sm text-muted">{provider.location}</p>
              </Link>
            ))}
          </div>
        </section>
      )}
      {providersQuery.isLoading ? (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {Array.from({ length: 6 }).map((_, index) => (
            <CardSkeleton key={index} />
          ))}
        </div>
      ) : providersQuery.data?.length ? (
        <div className={view === "grid" ? "grid gap-4 sm:grid-cols-2 lg:grid-cols-3" : "grid gap-4"}>
          {providersQuery.data.map((provider) => (
            <Link key={provider.id} to={`/providers/${provider.id}`}>
              <Card className={view === "list" ? "grid gap-4 sm:grid-cols-[220px_1fr]" : ""}>
                <img
                  src={
                    provider.coverImageUrl ||
                    provider.portfolioItems[0]?.imageUrl ||
                    "https://images.unsplash.com/photo-1464366400600-7168b8af9bc3?auto=format&fit=crop&w=800&q=80"
                  }
                  alt={provider.companyName}
                  className="h-44 w-full rounded-md object-cover"
                />
                <div>
                  <div className="mt-4 flex items-start justify-between gap-3 sm:mt-0">
                    <div>
                      <h2 className="font-semibold text-ink">{provider.companyName}</h2>
                      <p className="text-sm text-muted">{provider.servicesOffered}</p>
                    </div>
                    {provider.isVerified && <Badge tone="success">Verified</Badge>}
                  </div>
                  <div className="mt-4 grid gap-2 text-sm text-muted">
                    <span>{provider.location}</span>
                    <span>
                      {provider.rating.toFixed(1)} rating · {provider.totalReviews} reviews
                    </span>
                    <span className="font-semibold text-ink">Starts at {formatCurrency(provider.startingPrice)}</span>
                  </div>
                  <Button className="mt-4" variant="secondary" icon={<Search className="size-4" />}>
                    View details
                  </Button>
                </div>
              </Card>
            </Link>
          ))}
        </div>
      ) : (
        <EmptyState title="No providers found" description="Try a broader search or remove the verified filter." />
      )}
    </div>
  );
}
