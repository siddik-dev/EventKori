import { clsx } from "clsx";

interface BadgeProps {
  children: string;
  tone?: "success" | "info" | "danger" | "neutral" | "warning";
}

const tones = {
  success: "bg-emerald-50 text-emerald-700 ring-emerald-200",
  info: "bg-blue-50 text-blue-700 ring-blue-200",
  danger: "bg-rose-50 text-rose-700 ring-rose-200",
  warning: "bg-amber-50 text-amber-700 ring-amber-200",
  neutral: "bg-slate-100 text-slate-700 ring-slate-200"
};

export function Badge({ children, tone = "neutral" }: BadgeProps) {
  return (
    <span className={clsx("inline-flex w-fit items-center rounded-full px-2.5 py-1 text-xs font-semibold ring-1", tones[tone])}>
      {children}
    </span>
  );
}
