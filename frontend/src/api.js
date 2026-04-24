const BASE_URL =
  window.location.hostname === 'localhost'
    ? 'http://localhost:8080'
    : 'https://teamsplit-api.onrender.com'

async function request(path, options = {}) {
  const res = await fetch(`${BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...options,
  })
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
