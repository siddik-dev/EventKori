export type UserType = "Customer" | "ServiceProvider" | "Admin" | 0 | 1 | 2;
export type EventStatus = "Planning" | "Booked" | "InProgress" | "Completed" | "Cancelled" | 0 | 1 | 2 | 3 | 4;
export type BookingStatus =
  | "Pending"
  | "Confirmed"
  | "InProgress"
  | "Completed"
  | "Cancelled"
  | "Refunded"
  | 0
  | 1
  | 2
  | 3
  | 4
  | 5;

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  userType: 0 | 1;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  userType: UserType;
}

export interface Package {
  id: number;
  serviceProviderId: number;
  packageName: string;
  description: string;
  price: number;
  features: string[];
  durationHours: number;
  isPopular: boolean;
  maxAttendees: number;
}

export interface PortfolioItem {
  id: number;
  serviceProviderId: number;
  title: string;
  description: string;
  imageUrl: string;
  category: string;
  eventDate: string;
  location: string;
  attendeesCount: number;
  budget: number;
}

export interface Review {
  id: number;
  rating: number;
  comment: string;
  reviewerName: string;
  reviewDate: string;
  response: string;
}

export interface ServiceProvider {
  id: number;
  userId: number;
  companyName: string;
  ownerName: string;
  description: string;
  servicesOffered: string;
  startingPrice: number;
  location: string;
  rating: number;
  totalReviews: number;
  yearsOfExperience: number;
  isVerified: boolean;
  coverImageUrl: string;
  packages: Package[];
  portfolioItems: PortfolioItem[];
}

export interface ServiceProviderDetail extends ServiceProvider {
  reviews: Review[];
}

export interface CreateServiceProvider {
  userId: number;
  companyName: string;
  description: string;
  servicesOffered: string;
  startingPrice: number;
  location: string;
  yearsOfExperience: number;
  isVerified: boolean;
  businessLicense: string;
  taxId: string;
}

export interface EventRequest {
  id: number;
  userId: number;
  clientName: string;
  title: string;
  description: string;
  eventDate: string;
  location: string;
  attendeesCount: number;
  budget: number;
  eventType: string;
  status: EventStatus;
  specialRequirements: string;
  quotationCount: number;
}

export interface EventRequestDetail extends EventRequest {
  quotations: Booking[];
}

export interface CreateEventRequest {
  userId?: number;
  title: string;
  description: string;
  eventDate: string;
  location: string;
  attendeesCount: number;
  budget: number;
  eventType: string;
  specialRequirements: string;
}

export interface UpdateEventRequest extends CreateEventRequest {
  status: EventStatus;
}

export interface Booking {
  id: number;
  eventId: number;
  eventTitle: string;
  serviceProviderId: number;
  providerName: string;
  packageId: number | null;
  packageName: string;
  bookingDate: string;
  eventDate: string;
  totalAmount: number;
  status: BookingStatus;
  specialRequirements: string;
  paymentStatus: string;
  notes: string;
}

export interface CreateBooking {
  eventId: number;
  serviceProviderId: number;
  packageId?: number | null;
  totalAmount: number;
  specialRequirements: string;
  notes: string;
}

export interface UpdateBookingStatus {
  status: BookingStatus;
  paymentStatus: string;
  notes: string;
}

export interface DashboardSummary {
  totalEventRequests: number;
  activeEventRequests: number;
  verifiedOrganizers: number;
  pendingQuotations: number;
  totalQuotedValue: number;
  recentRequests: EventRequest[];
  featuredOrganizers: ServiceProvider[];
  latestQuotations: Booking[];
}
