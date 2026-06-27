import { clsx } from "clsx";

export function Skeleton({ className }: { className?: string }) {
  return <div className={clsx("animate-pulse rounded-md bg-slate-200", className)} />;
}

export function CardSkeleton() {
  return (
    <div className="surface rounded-lg p-5">
      <Skeleton className="mb-4 h-36" />
      <Skeleton className="mb-3 h-5 w-2/3" />
      <Skeleton className="mb-2 h-4 w-full" />
      <Skeleton className="h-4 w-1/2" />
    </div>
  );
}
