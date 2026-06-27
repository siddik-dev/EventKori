import { create } from "zustand";
import { createJSONStorage, persist } from "zustand/middleware";
import type { AuthResponse } from "../types/api";
import { userTypeLabel } from "../lib/status";

interface AuthState {
  token: string | null;
  user: AuthResponse | null;
  setSession: (session: AuthResponse) => void;
  clearSession: () => void;
  isAuthenticated: () => boolean;
  role: () => string | null;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      token: null,
      user: null,
      setSession: (session) => set({ token: session.token, user: session }),
      clearSession: () => set({ token: null, user: null }),
      isAuthenticated: () => Boolean(get().token),
      role: () => {
        const user = get().user;
        return user?.role || (user ? userTypeLabel(user.userType) : null);
      }
    }),
    {
      name: "eventkori-session",
      storage: createJSONStorage(() => sessionStorage)
    }
  )
);
