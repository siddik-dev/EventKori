import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation } from "@tanstack/react-query";
import { UserPlus } from "lucide-react";
import { useForm } from "react-hook-form";
import { Link, Navigate, useNavigate } from "react-router-dom";
import { toast } from "sonner";
import { z } from "zod";
import { register } from "../api/auth";
import { PublicHeader } from "../components/layout/PublicHeader";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { Field, SelectField } from "../components/ui/Field";
import { apiErrorMessage } from "../lib/http";
import { USER_TYPE } from "../lib/constants";
import { useAuthStore } from "../store/authStore";

const schema = z.object({
  firstName: z.string().min(1, "First name is required.").max(50),
  lastName: z.string().min(1, "Last name is required.").max(50),
  email: z.string().email("Enter a valid email."),
  phoneNumber: z.string().min(1, "Phone number is required."),
  password: z.string().min(6, "Password must be at least 6 characters."),
  userType: z.coerce.number().pipe(z.union([z.literal(0), z.literal(1)]))
});

type RegisterInput = z.input<typeof schema>;
type RegisterForm = z.output<typeof schema>;

export function RegisterPage() {
  const token = useAuthStore((state) => state.token);
  const setSession = useAuthStore((state) => state.setSession);
  const navigate = useNavigate();
  const form = useForm<RegisterInput, unknown, RegisterForm>({
    resolver: zodResolver(schema),
    defaultValues: { firstName: "", lastName: "", email: "", phoneNumber: "", password: "", userType: USER_TYPE.Customer }
  });
  const mutation = useMutation({
    mutationFn: register,
    onSuccess: (session) => {
      setSession(session);
      toast.success("Account created.");
      navigate("/dashboard", { replace: true });
    },
    onError: (error) => toast.error(apiErrorMessage(error, "Registration failed."))
  });

  if (token) return <Navigate to="/dashboard" replace />;

  return (
    <div className="min-h-screen bg-slate-50">
      <PublicHeader />
      <main className="mx-auto grid min-h-[calc(100vh-80px)] max-w-7xl place-items-center px-4 py-10 sm:px-6 lg:px-8">
        <Card className="w-full max-w-2xl">
          <h1 className="text-2xl font-bold text-ink">Create account</h1>
          <p className="mt-2 text-sm text-muted">Choose the role that matches how you will use EventKori.</p>
          <form className="mt-6 grid gap-4 sm:grid-cols-2" onSubmit={form.handleSubmit((values) => mutation.mutate(values))}>
            <Field label="First name" error={form.formState.errors.firstName?.message} {...form.register("firstName")} />
            <Field label="Last name" error={form.formState.errors.lastName?.message} {...form.register("lastName")} />
            <Field label="Email" type="email" error={form.formState.errors.email?.message} {...form.register("email")} />
            <Field label="Phone number" error={form.formState.errors.phoneNumber?.message} {...form.register("phoneNumber")} />
            <Field label="Password" type="password" error={form.formState.errors.password?.message} {...form.register("password")} />
            <SelectField label="Role" error={form.formState.errors.userType?.message} {...form.register("userType")}>
              <option value={USER_TYPE.Customer}>Customer</option>
              <option value={USER_TYPE.ServiceProvider}>Service Provider</option>
            </SelectField>
            <div className="sm:col-span-2">
              <Button type="submit" disabled={mutation.isPending} icon={<UserPlus className="size-4" />}>
                {mutation.isPending ? "Creating..." : "Create account"}
              </Button>
            </div>
          </form>
          <p className="mt-5 text-sm text-muted">
            Already registered?{" "}
            <Link to="/login" className="font-semibold text-brand-700">
              Login
            </Link>
          </p>
        </Card>
      </main>
    </div>
  );
}
