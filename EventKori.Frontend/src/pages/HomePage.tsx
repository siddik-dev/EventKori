import { CalendarPlus, Search, ShieldCheck, Sparkles } from "lucide-react";
import { Link } from "react-router-dom";
import { PublicHeader } from "../components/layout/PublicHeader";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";

export function HomePage() {
  return (
    <div className="min-h-screen bg-slate-50">
      <PublicHeader />
      <main>
        <section className="border-b border-line bg-white">
          <div className="mx-auto grid max-w-7xl gap-10 px-4 py-14 sm:px-6 lg:grid-cols-[1.05fr_0.95fr] lg:px-8">
            <div className="flex flex-col justify-center">
              <span className="mb-4 w-fit rounded-full bg-brand-50 px-3 py-1 text-sm font-semibold text-brand-700">
                Plan, compare, book, and manage events
              </span>
              <h1 className="max-w-3xl text-4xl font-bold tracking-normal text-ink sm:text-5xl">EventKori</h1>
              <p className="mt-5 max-w-2xl text-lg leading-8 text-muted">
                A focused workspace for customers and service providers to coordinate weddings, birthdays, corporate programs, and celebrations.
              </p>
              <div className="mt-8 flex flex-col gap-3 sm:flex-row">
                <Link to="/register">
                  <Button icon={<CalendarPlus className="size-4" />}>Start planning</Button>
                </Link>
                <Link to="/login">
                  <Button variant="secondary" icon={<Search className="size-4" />}>
                    Browse providers
                  </Button>
                </Link>
              </div>
            </div>
            <div className="grid gap-4 rounded-lg border border-line bg-slate-50 p-4">
              <img
                src="https://images.unsplash.com/photo-1511578314322-379afb476865?auto=format&fit=crop&w=1200&q=80"
                alt="Event venue setup"
                className="h-72 w-full rounded-md object-cover sm:h-96"
              />
              <div className="grid gap-3 sm:grid-cols-3">
                {[
                  ["Verified", "Trusted provider profiles"],
                  ["Fast", "Request quotes in minutes"],
                  ["Organized", "Track events and bookings"]
                ].map(([label, detail]) => (
                  <div key={label} className="rounded-md bg-white p-3">
                    <p className="font-semibold text-ink">{label}</p>
                    <p className="text-sm text-muted">{detail}</p>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </section>
        <section className="mx-auto grid max-w-7xl gap-4 px-4 py-10 sm:grid-cols-3 sm:px-6 lg:px-8">
          {[
            { icon: Search, title: "Find providers", body: "Search by service, location, rating, and verification." },
            { icon: CalendarPlus, title: "Create event requests", body: "Share date, budget, location, guest count, and requirements." },
            { icon: ShieldCheck, title: "Manage bookings", body: "Track proposals, confirmations, cancellations, and progress." }
          ].map((item) => (
            <Card key={item.title}>
              <item.icon className="mb-4 size-6 text-brand-700" />
              <h2 className="font-semibold text-ink">{item.title}</h2>
              <p className="mt-2 text-sm text-muted">{item.body}</p>
            </Card>
          ))}
        </section>
        <section className="mx-auto max-w-7xl px-4 pb-12 sm:px-6 lg:px-8">
          <div className="rounded-lg bg-ink p-6 text-white sm:flex sm:items-center sm:justify-between">
            <div className="flex items-center gap-3">
              <Sparkles className="size-6 text-teal-300" />
              <p className="font-semibold">Already have an account? Continue to your workspace.</p>
            </div>
            <Link to="/login" className="mt-4 inline-block sm:mt-0">
              <Button variant="secondary">Login</Button>
            </Link>
          </div>
        </section>
      </main>
    </div>
  );
}
