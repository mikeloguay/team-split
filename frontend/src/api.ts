const BASE_URL =
  window.location.hostname === 'localhost'
    ? 'http://localhost:8080'
    : 'https://teamsplit-api.onrender.com'

let authToken: string | null = null

export function setAuthToken(token: string | null) {
  authToken = token
}

async function request<T>(path: string, options: RequestInit = {}): Promise<T> {
  const headers: Record<string, string> = { 'Content-Type': 'application/json' }
  if (authToken) headers['Authorization'] = `Bearer ${authToken}`

  const res = await fetch(`${BASE_URL}${path}`, { headers, ...options })

  if (res.status === 401) throw new Error('UNAUTHORIZED')
  if (!res.ok) {
    const body = await res.json().catch(() => ({}))
    throw new Error((body as { title?: string }).title || `Error ${res.status}`)
  }
  if (res.status === 204) return null as unknown as T
  return res.json() as Promise<T>
}

export interface Player {
  name: string
  level: number
}

export interface SplitResult {
  team1: { players: string[] }
  team2: { players: string[] }
}

export const getPlayers = () => request<Player[]>('/players')
export const createPlayer = (name: string, level: number) =>
  request<Player>('/players', { method: 'POST', body: JSON.stringify({ name, level }) })
export const updatePlayer = (name: string, level: number) =>
  request<Player>(`/players/${encodeURIComponent(name)}`, {
    method: 'PUT',
    body: JSON.stringify({ level }),
  })
export const deletePlayer = (name: string) =>
  request<null>(`/players/${encodeURIComponent(name)}`, { method: 'DELETE' })
export const splitTeams = (players: string[]) =>
  request<SplitResult>('/players/split', { method: 'POST', body: JSON.stringify({ players }) })
