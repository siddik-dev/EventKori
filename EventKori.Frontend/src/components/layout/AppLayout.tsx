import { CalendarCheck, ChevronDown, LayoutDashboard, LogOut, Menu, Search, User, X } from "lucide-react";
import { useState } from "react";
import { Link, NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuthStore } from "../../store/authStore";
import { API_BASE_URL } from "../../lib/constants";
import { userTypeLabel } from "../../lib/status";
import { Button } from "../ui/Button";

const links = [
  { to: "/dashboard", label: "Dashboard", icon: LayoutDashboard },
  { to: "/providers", label: "Providers", icon: Search },
  { to: "/events", label: "Events", icon: CalendarCheck },
  { to: "/bookings", label: "Bookings", icon: CalendarCheck }
];

export function AppLayout() {
  const [menuOpen, setMenuOpen] = useState(false);
  const [profileOpen, setProfileOpen] = useState(false);
  const user = useAuthStore((state) => state.user);
  const clearSession = useAuthStore((state) => state.clearSession);
  const navigate = useNavigate();

  const logout = () => {
    clearSession();
    navigate("/");
  };

  const nav = (
    <nav className="flex flex-col gap-1 md:flex-row md:items-center md:gap-2">
      {links.map((item) => (
        <NavLink
          key={item.to}
          to={item.to}
          onClick={() => setMenuOpen(false)}
          className={({ isActive }) =>
            `flex items-center gap-2 rounded-md px-3 py-2 text-sm font-semibold ${
              isActive ? "bg-brand-50 text-brand-700" : "text-slate-600 hover:bg-slate-100 hover:text-ink"
            }`
          }
        >
          <item.icon className="size-4" />
          {item.label}
        </NavLink>
      ))}
    </nav>
  );

  return (
    <div className="min-h-screen bg-slate-50">
      <header className="sticky top-0 z-40 border-b border-line bg-white/95 backdrop-blur">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-4 py-3 sm:px-6 lg:px-8">
          <Link to="/dashboard" className="flex items-center gap-2 text-lg font-bold text-ink">
            <span className="grid size-9 place-items-center rounded-lg bg-brand-600 text-white">
              <CalendarCheck className="size-5" />
            </span>
            EventKori
          </Link>
          <div className="hidden md:block">{nav}</div>
          <div className="flex items-center gap-2">
            <div className="relative hidden sm:block">
              <button
                type="button"
                onClick={() => setProfileOpen((value) => !value)}
                className="focus-ring flex items-center gap-2 rounded-md px-3 py-2 text-sm font-semibold text-slate-700 hover:bg-slate-100"
              >
                <span className="grid size-8 place-items-center rounded-full bg-slate-100 text-slate-600">
                  <User className="size-4" />
                </span>
                <span className="max-w-36 truncate">{user?.firstName || "User"}</span>
                <ChevronDown className="size-4" />
              </button>
              {profileOpen && (
                <div className="absolute right-0 mt-2 w-64 rounded-lg border border-line bg-white p-2 shadow-soft">
                  <div className="border-b border-line px-3 py-2">
                    <p className="font-semibold text-ink">
                      {user?.firstName} {user?.lastName}
                    </p>
                    <p className="truncate text-sm text-muted">{user?.email}</p>
                    <p className="mt-1 text-xs font-semibold text-brand-700">{user ? userTypeLabel(user.userType) : ""}</p>
                  </div>
                  <Link to="/profile" onClick={() => setProfileOpen(false)} className="mt-2 flex rounded-md px-3 py-2 text-sm hover:bg-slate-100">
                    Profile settings
                  </Link>
                  <button onClick={logout} className="flex w-full items-center gap-2 rounded-md px-3 py-2 text-left text-sm text-rose-600 hover:bg-rose-50">
                    <LogOut className="size-4" />
                    Logout
                  </button>
                </div>
              )}
            </div>
            <button
              type="button"
              className="focus-ring grid size-10 place-items-center rounded-md border border-line bg-white md:hidden"
              onClick={() => setMenuOpen((value) => !value)}
              aria-label="Toggle menu"
            >
              {menuOpen ? <X className="size-5" /> : <Menu className="size-5" />}
            </button>
          </div>
        </div>
        {menuOpen && <div className="border-t border-line bg-white px-4 py-3 md:hidden">{nav}</div>}
      </header>
      <main className="mx-auto min-h-[calc(100vh-144px)] max-w-7xl px-4 py-8 sm:px-6 lg:px-8">
        <Outlet />
      </main>
      <footer className="border-t border-line bg-white">
        <div className="mx-auto flex max-w-7xl flex-col gap-2 px-4 py-5 text-sm text-muted sm:flex-row sm:items-center sm:justify-between sm:px-6 lg:px-8">
          <span>EventKori event planning workspace</span>
          <span>API: {API_BASE_URL}</span>
        </div>
      </footer>
    </div>
  );
}
