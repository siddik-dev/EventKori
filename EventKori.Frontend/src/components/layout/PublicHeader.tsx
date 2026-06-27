import { CalendarCheck, LogIn, UserPlus } from "lucide-react";
import { Link, NavLink } from "react-router-dom";
import { Button } from "../ui/Button";

export function PublicHeader() {
  return (
    <header className="border-b border-line bg-white">
      <div className="mx-auto flex max-w-7xl items-center justify-between px-4 py-4 sm:px-6 lg:px-8">
        <Link to="/" className="flex items-center gap-2 text-lg font-bold text-ink">
          <span className="grid size-9 place-items-center rounded-lg bg-brand-600 text-white">
            <CalendarCheck className="size-5" />
          </span>
          EventKori
        </Link>
        <nav className="hidden items-center gap-6 text-sm font-medium text-slate-600 sm:flex">
          <NavLink to="/" className={({ isActive }) => (isActive ? "text-brand-700" : "hover:text-ink")}>
            Home
          </NavLink>
          <NavLink to="/about" className={({ isActive }) => (isActive ? "text-brand-700" : "hover:text-ink")}>
            About
          </NavLink>
        </nav>
        <div className="flex gap-2">
          <Link to="/login">
            <Button variant="secondary" icon={<LogIn className="size-4" />}>
              Login
            </Button>
          </Link>
          <Link to="/register" className="hidden sm:block">
            <Button icon={<UserPlus className="size-4" />}>Register</Button>
          </Link>
        </div>
      </div>
    </header>
  );
}
