import { http } from "../lib/http";
import type { DashboardSummary } from "../types/api";

export async function getDashboard() {
  const { data } = await http.get<DashboardSummary>("dashboard");
  return data;
}
