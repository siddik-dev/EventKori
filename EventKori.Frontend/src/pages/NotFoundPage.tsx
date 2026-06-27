import { Link } from "react-router-dom";
import { Button } from "../components/ui/Button";

export function NotFoundPage() {
  return (
    <div className="grid min-h-screen place-items-center bg-slate-50 p-4 text-center">
      <div>
        <h1 className="text-3xl font-bold text-ink">Page not found</h1>
        <p className="mt-2 text-muted">The page you are looking for is not available.</p>
        <Link to="/dashboard" className="mt-6 inline-block">
          <Button>Go to dashboard</Button>
        </Link>
      </div>
    </div>
  );
}
