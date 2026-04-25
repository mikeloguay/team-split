const BASE_URL =
  window.location.hostname === 'localhost'
    ? 'http://localhost:8080'
    : 'https://teamsplit-api.onrender.com'

let authToken = null

export function setAuthToken(token) {
  authToken = token
}

async function request(path, options = {}) {
  const headers = { 'Content-Type': 'application/json' }
  if (authToken) headers['Authorization'] = `Bearer ${authToken}`

  const res = await fetch(`${BASE_URL}${path}`, { headers, ...options })

  if (res.status === 401) throw new Error('UNAUTHORIZED')
  if (!res.ok) {
    const body = await res.json().catch(() => ({}))
    throw new Error(body.title || `Error ${res.status}`)
  }
  return res.status === 204 ? null : res.json()
}

export const getPlayers = () => request('/players')
export const createPlayer = (name, level) =>
  request('/players', { method: 'POST', body: JSON.stringify({ name, level }) })
export const updatePlayer = (name, level) =>
  request(`/players/${encodeURIComponent(name)}`, {
    method: 'PUT',
    body: JSON.stringify({ level }),
  })
export const deletePlayer = (name) =>
  request(`/players/${encodeURIComponent(name)}`, { method: 'DELETE' })
export const splitTeams = (players) =>
  request('/players/split', { method: 'POST', body: JSON.stringify({ players }) })
