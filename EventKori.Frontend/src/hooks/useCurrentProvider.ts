import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { createProvider, getProviders } from "../api/providers";
import { domainUserIdFromToken } from "../lib/jwt";
import { useAuthStore } from "../store/authStore";
import type { CreateServiceProvider } from "../types/api";

export function useCurrentProvider() {
  const token = useAuthStore((state) => state.token);
  const domainUserId = domainUserIdFromToken(token);
  const queryClient = useQueryClient();
  const providersQuery = useQuery({
    queryKey: ["providers", "current"],
    queryFn: () => getProviders(),
    enabled: Boolean(domainUserId)
  });

  const currentProvider = providersQuery.data?.find((provider) => provider.userId === domainUserId) ?? null;

  const createProviderMutation = useMutation({
    mutationFn: (payload: Omit<CreateServiceProvider, "userId" | "isVerified">) => {
      if (!domainUserId) throw new Error("Missing user id.");
      return createProvider({ ...payload, userId: domainUserId, isVerified: false });
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["providers"] });
    }
  });

  return { domainUserId, currentProvider, providersQuery, createProviderMutation };
}
