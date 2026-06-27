import { zodResolver } from "@hookform/resolvers/zod";
import { Save, User } from "lucide-react";
import { useForm } from "react-hook-form";
import { toast } from "sonner";
import { z } from "zod";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { Field, TextArea } from "../components/ui/Field";
import { PageHeader } from "../components/ui/PageHeader";
import { useCurrentProvider } from "../hooks/useCurrentProvider";
import { apiErrorMessage } from "../lib/http";
import { formatCurrency } from "../lib/format";
import { userTypeLabel } from "../lib/status";
import { useAuthStore } from "../store/authStore";

const providerSchema = z.object({
  companyName: z.string().min(1, "Company name is required."),
  description: z.string().min(1, "Description is required."),
  servicesOffered: z.string().min(1, "Services offered are required."),
  startingPrice: z.coerce.number().min(1, "Starting price must be greater than zero."),
  location: z.string().min(1, "Location is required."),
  yearsOfExperience: z.coerce.number().min(0, "Years cannot be negative."),
  businessLicense: z.string().default(""),
  taxId: z.string().default("")
});

type ProviderInput = z.input<typeof providerSchema>;
type ProviderForm = z.output<typeof providerSchema>;

export function ProfilePage() {
  const user = useAuthStore((state) => state.user);
  const role = user ? userTypeLabel(user.userType) : "Customer";
  const { domainUserId, currentProvider, providersQuery, createProviderMutation } = useCurrentProvider();
  const form = useForm<ProviderInput, unknown, ProviderForm>({
    resolver: zodResolver(providerSchema),
    defaultValues: {
      companyName: "",
      description: "",
      servicesOffered: "",
      startingPrice: 1,
      location: "",
      yearsOfExperience: 0,
      businessLicense: "",
      taxId: ""
    }
  });

  const submitProvider = (values: ProviderForm) => {
    createProviderMutation.mutate(values, {
      onSuccess: () => {
        toast.success("Provider profile created.");
        form.reset();
      },
      onError: (error) => toast.error(apiErrorMessage(error, "Could not create provider profile."))
    });
  };

  return (
    <div className="grid gap-6">
      <PageHeader title="Profile" description="Review your account and provider workspace settings." />
      <div className="grid gap-6 lg:grid-cols-[360px_1fr]">
        <Card>
          <div className="grid size-14 place-items-center rounded-full bg-brand-50 text-brand-700">
            <User className="size-7" />
          </div>
          <h2 className="mt-4 text-xl font-semibold text-ink">
            {user?.firstName} {user?.lastName}
          </h2>
          <p className="text-sm text-muted">{user?.email}</p>
          <div className="mt-5 grid gap-3 text-sm">
            <Info label="Role" value={role} />
            <Info label="Domain user id" value={domainUserId ? `${domainUserId}` : "Unavailable"} />
          </div>
        </Card>
        {role === "ServiceProvider" ? (
          currentProvider ? (
            <Card>
              <h2 className="text-lg font-semibold text-ink">{currentProvider.companyName}</h2>
              <p className="mt-2 text-sm text-muted">{currentProvider.description}</p>
              <div className="mt-5 grid gap-4 sm:grid-cols-2">
                <Info label="Services" value={currentProvider.servicesOffered} />
                <Info label="Location" value={currentProvider.location} />
                <Info label="Starting price" value={formatCurrency(currentProvider.startingPrice)} />
                <Info label="Verified" value={currentProvider.isVerified ? "Yes" : "Pending"} />
              </div>
            </Card>
          ) : (
            <Card>
              <h2 className="text-lg font-semibold text-ink">Create provider profile</h2>
              <p className="mt-2 text-sm text-muted">Provider accounts need a company profile before sending proposals.</p>
              <form className="mt-5 grid gap-4 sm:grid-cols-2" onSubmit={form.handleSubmit(submitProvider)}>
                <Field label="Company name" error={form.formState.errors.companyName?.message} {...form.register("companyName")} />
                <Field label="Location" error={form.formState.errors.location?.message} {...form.register("location")} />
                <Field label="Services offered" error={form.formState.errors.servicesOffered?.message} {...form.register("servicesOffered")} />
                <Field label="Starting price" type="number" error={form.formState.errors.startingPrice?.message} {...form.register("startingPrice", { valueAsNumber: true })} />
                <Field label="Years of experience" type="number" error={form.formState.errors.yearsOfExperience?.message} {...form.register("yearsOfExperience", { valueAsNumber: true })} />
                <Field label="Business license" error={form.formState.errors.businessLicense?.message} {...form.register("businessLicense")} />
                <Field label="Tax ID" error={form.formState.errors.taxId?.message} {...form.register("taxId")} />
                <div className="sm:col-span-2">
                  <TextArea label="Description" error={form.formState.errors.description?.message} {...form.register("description")} />
                </div>
                <div className="sm:col-span-2">
                  <Button type="submit" disabled={providersQuery.isLoading || createProviderMutation.isPending} icon={<Save className="size-4" />}>
                    {createProviderMutation.isPending ? "Creating..." : "Create provider profile"}
                  </Button>
                </div>
              </form>
            </Card>
          )
        ) : (
          <Card>
            <h2 className="text-lg font-semibold text-ink">Customer account</h2>
            <p className="mt-2 text-sm text-muted">Use the events area to create requests, browse providers, and manage bookings.</p>
          </Card>
        )}
      </div>
    </div>
  );
}

function Info({ label, value }: { label: string; value: string }) {
  return (
    <div className="rounded-md border border-line p-3">
      <p className="text-xs font-semibold uppercase text-muted">{label}</p>
      <p className="mt-1 font-semibold text-ink">{value}</p>
    </div>
  );
}
