import type { ReactNode } from "react";
import { Card } from "./Card";

export function StatCard({ label, value, icon }: { label: string; value: string | number; icon: ReactNode }) {
  return (
    <Card className="flex items-center justify-between gap-4">
      <div>
        <p className="text-sm font-medium text-muted">{label}</p>
        <p className="mt-2 text-2xl font-bold text-ink">{value}</p>
      </div>
      <div className="grid size-11 place-items-center rounded-lg bg-brand-50 text-brand-700">{icon}</div>
    </Card>
  );
}
