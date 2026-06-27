import type { ReactNode } from "react";
import { Inbox } from "lucide-react";

export function EmptyState({ title, description, action }: { title: string; description: string; action?: ReactNode }) {
  return (
    <div className="grid place-items-center rounded-lg border border-dashed border-line bg-white px-6 py-12 text-center">
      <div className="grid max-w-md justify-items-center gap-3">
        <div className="grid size-12 place-items-center rounded-full bg-slate-100 text-slate-500">
          <Inbox className="size-6" />
        </div>
        <h3 className="text-lg font-semibold text-ink">{title}</h3>
        <p className="text-sm text-muted">{description}</p>
        {action}
      </div>
    </div>
  );
}
