import { ChevronLeft, ChevronRight } from "lucide-react";
import { Button } from "./Button";

export function Pagination({
  page,
  totalPages,
  onPageChange
}: {
  page: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}) {
  if (totalPages <= 1) return null;
  return (
    <div className="flex items-center justify-end gap-3">
      <Button variant="secondary" disabled={page === 1} onClick={() => onPageChange(page - 1)} icon={<ChevronLeft className="size-4" />}>
        Previous
      </Button>
      <span className="text-sm font-medium text-muted">
        Page {page} of {totalPages}
      </span>
      <Button
        variant="secondary"
        disabled={page === totalPages}
        onClick={() => onPageChange(page + 1)}
        icon={<ChevronRight className="size-4" />}
      >
        Next
      </Button>
    </div>
  );
}
