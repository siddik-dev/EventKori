import { zodResolver } from "@hookform/resolvers/zod";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { Save } from "lucide-react";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useNavigate, useParams } from "react-router-dom";
import { toast } from "sonner";
import { z } from "zod";
import { createEvent, getEvent, updateEvent } from "../api/events";
import { Button } from "../components/ui/Button";
import { Card } from "../components/ui/Card";
import { Field, SelectField, TextArea } from "../components/ui/Field";
import { PageHeader } from "../components/ui/PageHeader";
import { EVENT_STATUSES, EVENT_TYPES } from "../lib/constants";
import { apiErrorMessage } from "../lib/http";
import { eventStatusLabel } from "../lib/status";
import type { EventStatus } from "../types/api";

const schema = z.object({
  title: z.string().min(1, "Title is required.").max(100),
  eventType: z.string().min(1, "Event type is required."),
  eventDate: z.string().min(1, "Event date is required."),
  location: z.string().min(1, "Location is required."),
  attendeesCount: z.coerce.number().min(1, "Guest count must be greater than zero."),
  budget: z.coerce.number().min(1, "Budget must be greater than zero."),
  description: z.string().default(""),
  specialRequirements: z.string().default(""),
  status: z.string().default("Planning")
});

type EventFormInput = z.input<typeof schema>;
type EventForm = z.output<typeof schema>;

function inputDateTime(value: string) {
  if (!value) return "";
  const date = new Date(value);
  const offset = date.getTimezoneOffset();
  return new Date(date.getTime() - offset * 60_000).toISOString().slice(0, 16);
}

export function EventFormPage() {
  const { id } = useParams();
  const eventId = Number(id);
  const isEdit = Number.isFinite(eventId);
  const navigate = useNavigate();
  const queryClient = useQueryClient();
  const eventQuery = useQuery({ queryKey: ["events", eventId], queryFn: () => getEvent(eventId), enabled: isEdit });
  const form = useForm<EventFormInput, unknown, EventForm>({
    resolver: zodResolver(schema),
    defaultValues: {
      title: "",
      eventType: "Wedding",
      eventDate: "",
      location: "",
      attendeesCount: 1,
      budget: 1,
      description: "",
      specialRequirements: "",
      status: "Planning"
    }
  });

  useEffect(() => {
    if (eventQuery.data) {
      form.reset({
        ...eventQuery.data,
        eventDate: inputDateTime(eventQuery.data.eventDate),
        status: eventStatusLabel(eventQuery.data.status)
      });
    }
  }, [eventQuery.data, form]);

  const mutation = useMutation({
    mutationFn: (values: EventForm) => {
      const payload = {
        title: values.title,
        eventType: values.eventType,
        eventDate: new Date(values.eventDate).toISOString(),
        location: values.location,
        attendeesCount: values.attendeesCount,
        budget: values.budget,
        description: values.description,
        specialRequirements: values.specialRequirements
      };
      return isEdit ? updateEvent(eventId, { ...payload, status: values.status as EventStatus }) : createEvent(payload);
    },
    onSuccess: (event) => {
      toast.success(isEdit ? "Event updated." : "Event created.");
      queryClient.invalidateQueries({ queryKey: ["events"] });
      navigate(`/events/${event.id}`);
    },
    onError: (error) => toast.error(apiErrorMessage(error, "Could not save event."))
  });

  return (
    <div className="grid gap-6">
      <PageHeader title={isEdit ? "Edit event request" : "Create event request"} description="Share enough detail for providers to prepare useful quotations." />
      <Card>
        <form className="grid gap-4 sm:grid-cols-2" onSubmit={form.handleSubmit((values) => mutation.mutate(values))}>
          <Field label="Title" error={form.formState.errors.title?.message} {...form.register("title")} />
          <SelectField label="Event type" error={form.formState.errors.eventType?.message} {...form.register("eventType")}>
            {EVENT_TYPES.map((type) => (
              <option key={type} value={type}>
                {type}
              </option>
            ))}
          </SelectField>
          <Field label="Event date and time" type="datetime-local" error={form.formState.errors.eventDate?.message} {...form.register("eventDate")} />
          <Field label="Location" error={form.formState.errors.location?.message} {...form.register("location")} />
          <Field label="Guest count" type="number" error={form.formState.errors.attendeesCount?.message} {...form.register("attendeesCount", { valueAsNumber: true })} />
          <Field label="Budget" type="number" error={form.formState.errors.budget?.message} {...form.register("budget", { valueAsNumber: true })} />
          {isEdit && (
            <SelectField label="Status" error={form.formState.errors.status?.message} {...form.register("status")}>
              {EVENT_STATUSES.map((status) => (
                <option key={status} value={status}>
                  {status}
                </option>
              ))}
            </SelectField>
          )}
          <div className="sm:col-span-2">
            <TextArea label="Description" error={form.formState.errors.description?.message} {...form.register("description")} />
          </div>
          <div className="sm:col-span-2">
            <TextArea label="Special requirements" error={form.formState.errors.specialRequirements?.message} {...form.register("specialRequirements")} />
          </div>
          <div className="sm:col-span-2">
            <Button type="submit" disabled={mutation.isPending} icon={<Save className="size-4" />}>
              {mutation.isPending ? "Saving..." : "Save event"}
            </Button>
          </div>
        </form>
      </Card>
    </div>
  );
}
