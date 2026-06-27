interface JwtPayload {
  domain_user_id?: string;
  exp?: number;
  [key: string]: unknown;
}

export function decodeToken(token: string): JwtPayload {
  try {
    const [, payload] = token.split(".");
    if (!payload) return {};
    const normalized = payload.replace(/-/g, "+").replace(/_/g, "/");
    const json = decodeURIComponent(
      atob(normalized)
        .split("")
        .map((char) => `%${`00${char.charCodeAt(0).toString(16)}`.slice(-2)}`)
        .join("")
    );
    return JSON.parse(json) as JwtPayload;
  } catch {
    return {};
  }
}

export function domainUserIdFromToken(token?: string | null) {
  if (!token) return null;
  const value = decodeToken(token).domain_user_id;
  const id = value ? Number(value) : Number.NaN;
  return Number.isFinite(id) ? id : null;
}
