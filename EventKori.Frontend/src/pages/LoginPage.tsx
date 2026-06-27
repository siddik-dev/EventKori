import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
import { LogIn } from "lucide-react";
import { useForm } from "react-hook-form";
import { Link, Navigate, useLocation, useNavigate } from "react-router-dom";
import { toast } from "sonner";
import { z } from "zod";
import { login } from "../api/auth";
import { PublicHeader } from "../components/layout/PublicHeader";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { Field } from "../components/ui/Field";
import { apiErrorMessage } from "../lib/http";
import { useAuthStore } from "../store/authStore";

const schema = z.object({
  email: z.string().email("Enter a valid email."),
  password: z.string().min(1, "Password is required.")
});

type LoginForm = z.infer<typeof schema>;

export function LoginPage() {
  const token = useAuthStore((state) => state.token);
  const setSession = useAuthStore((state) => state.setSession);
  const navigate = useNavigate();
  const location = useLocation();
  const from = (location.state as { from?: Location })?.from?.pathname ?? "/dashboard";
  const form = useForm<LoginForm>({ resolver: zodResolver(schema), defaultValues: { email: "", password: "" } });
  const mutation = useMutation({
    mutationFn: login,
    onSuccess: (session) => {
      setSession(session);
      toast.success("Welcome back.");
      navigate(from, { replace: true });
    },
    onError: (error) => toast.error(apiErrorMessage(error, "Login failed."))
  });

  if (token) return <Navigate to="/dashboard" replace />;

  return (
    <div className="min-h-screen bg-slate-50">
      <PublicHeader />
      <main className="mx-auto grid min-h-[calc(100vh-80px)] max-w-7xl place-items-center px-4 py-10 sm:px-6 lg:px-8">
        <Card className="w-full max-w-md">
          <h1 className="text-2xl font-bold text-ink">Login</h1>
          <p className="mt-2 text-sm text-muted">Enter your credentials to access EventKori.</p>
          <form className="mt-6 grid gap-4" onSubmit={form.handleSubmit((values) => mutation.mutate(values))}>
            <Field label="Email" type="email" autoComplete="email" error={form.formState.errors.email?.message} {...form.register("email")} />
            <Field
              label="Password"
              type="password"
              autoComplete="current-password"
              error={form.formState.errors.password?.message}
              {...form.register("password")}
            />
            <Button type="submit" disabled={mutation.isPending} icon={<LogIn className="size-4" />}>
              {mutation.isPending ? "Signing in..." : "Login"}
            </Button>
          </form>
          <p className="mt-5 text-sm text-muted">
            New to EventKori?{" "}
            <Link to="/register" className="font-semibold text-brand-700">
              Create an account
            </Link>
          </p>
        </Card>
      </main>
    </div>
  );
}
