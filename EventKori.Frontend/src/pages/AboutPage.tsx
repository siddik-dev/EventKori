import { Mail, MapPin, Phone } from "lucide-react";
import { PublicHeader } from "../components/layout/PublicHeader";
import { Card } from "../components/ui/Card";

export function AboutPage() {
  return (
    <div className="min-h-screen bg-slate-50">
      <PublicHeader />
      <main className="mx-auto max-w-5xl px-4 py-12 sm:px-6 lg:px-8">
        <div className="mb-8">
          <h1 className="text-3xl font-bold text-ink">About EventKori</h1>
          <p className="mt-3 text-muted">
            EventKori brings event customers and service providers into one clean workflow for discovery, planning, quotations, and bookings.
          </p>
        </div>
        <div className="grid gap-4 sm:grid-cols-3">
          <Card>
            <MapPin className="mb-3 size-5 text-brand-700" />
            <p className="font-semibold text-ink">Dhaka, Bangladesh</p>
            <p className="text-sm text-muted">Serving event teams across major cities.</p>
          </Card>
          <Card>
            <Mail className="mb-3 size-5 text-brand-700" />
            <p className="font-semibold text-ink">hello@eventkori.com</p>
            <p className="text-sm text-muted">Support for customers and providers.</p>
          </Card>
          <Card>
            <Phone className="mb-3 size-5 text-brand-700" />
            <p className="font-semibold text-ink">+880 1700 000000</p>
            <p className="text-sm text-muted">Available during business hours.</p>
          </Card>
        </div>
      </main>
    </div>
  );
}
