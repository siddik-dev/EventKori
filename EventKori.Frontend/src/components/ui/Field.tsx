import type { InputHTMLAttributes, ReactNode, SelectHTMLAttributes, TextareaHTMLAttributes } from "react";
import { clsx } from "clsx";

interface BaseProps {
  label: string;
  error?: string;
  hint?: string;
}

export function Field({
  label,
  error,
  hint,
  className,
  ...props
}: BaseProps & InputHTMLAttributes<HTMLInputElement>) {
  return (
    <label className="grid gap-1.5 text-sm font-medium text-slate-700">
      <span>{label}</span>
      <input
        className={clsx(
          "focus-ring min-h-11 rounded-md border border-line bg-white px-3 text-ink placeholder:text-slate-400",
          error && "border-rose-300",
          className
        )}
        {...props}
      />
      {hint && <span className="text-xs font-normal text-muted">{hint}</span>}
      {error && <span className="text-xs font-semibold text-rose-600">{error}</span>}
    </label>
  );
}

export function TextArea({
  label,
  error,
  hint,
  className,
  ...props
}: BaseProps & TextareaHTMLAttributes<HTMLTextAreaElement>) {
  return (
    <label className="grid gap-1.5 text-sm font-medium text-slate-700">
      <span>{label}</span>
      <textarea
        className={clsx(
          "focus-ring min-h-28 rounded-md border border-line bg-white px-3 py-2 text-ink placeholder:text-slate-400",
          error && "border-rose-300",
          className
        )}
        {...props}
      />
      {hint && <span className="text-xs font-normal text-muted">{hint}</span>}
      {error && <span className="text-xs font-semibold text-rose-600">{error}</span>}
    </label>
  );
}

export function SelectField({
  label,
  error,
  hint,
  children,
  className,
  ...props
}: BaseProps & SelectHTMLAttributes<HTMLSelectElement> & { children: ReactNode }) {
  return (
    <label className="grid gap-1.5 text-sm font-medium text-slate-700">
      <span>{label}</span>
      <select
        className={clsx(
          "focus-ring min-h-11 rounded-md border border-line bg-white px-3 text-ink",
          error && "border-rose-300",
          className
        )}
        {...props}
      >
        {children}
      </select>
      {hint && <span className="text-xs font-normal text-muted">{hint}</span>}
      {error && <span className="text-xs font-semibold text-rose-600">{error}</span>}
    </label>
  );
}
