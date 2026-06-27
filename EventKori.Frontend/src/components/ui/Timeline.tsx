export function Timeline({ items }: { items: Array<{ label: string; detail?: string }> }) {
  return (
    <ol className="space-y-4">
      {items.map((item, index) => (
        <li key={`${item.label}-${index}`} className="flex gap-3">
          <span className="mt-1 size-2.5 rounded-full bg-brand-600 ring-4 ring-brand-100" />
          <div>
            <p className="text-sm font-semibold text-ink">{item.label}</p>
            {item.detail && <p className="text-sm text-muted">{item.detail}</p>}
          </div>
        </li>
      ))}
    </ol>
  );
}
