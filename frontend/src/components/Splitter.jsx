import { useState } from 'react'
import { splitTeams } from '../api'

export default function Splitter({ players }) {
  const [selected, setSelected] = useState(new Set())
  const [result, setResult] = useState(null)
  const [error, setError] = useState(null)
  const [loading, setLoading] = useState(false)

  function toggle(name) {
    setSelected((prev) => {
      const next = new Set(prev)
      next.has(name) ? next.delete(name) : next.add(name)
      return next
    })
  }

  function toggleAll(checked) {
    setSelected(checked ? new Set(players.map((p) => p.name)) : new Set())
  }

  const allSelected = players.length > 0 && selected.size === players.length

  async function handleSplit() {
    setError(null)
    setLoading(true)
    try {
      const data = await splitTeams([...selected])
      setResult(data)
    } catch (e) {
      setError(e.message)
    } finally {
      setLoading(false)
    }
  }

  return (
    <section>
      <h2>Select players</h2>

      {error && <p className="error">{error}</p>}

      <label className="select-all-label">
        <input
          type="checkbox"
          checked={allSelected}
          onChange={(e) => toggleAll(e.target.checked)}
        />
        Select all ({players.length})
      </label>

      <div className="player-grid">
        {players.map((p) => (
          <label key={p.name}>
            <input
              type="checkbox"
              checked={selected.has(p.name)}
              onChange={() => toggle(p.name)}
            />
            {p.name}
          </label>
        ))}
      </div>

      <button
        className="split-btn"
        onClick={handleSplit}
        disabled={selected.size < 2 || loading}
      >
        {loading ? 'Splitting…' : `Split ${selected.size} players`}
      </button>

      {result && (
        <div className="teams">
          <div className="team">
            <h3>Team 1</h3>
            <ul>
              {result.team1.players.map((name) => (
                <li key={name}>{name}</li>
              ))}
            </ul>
          </div>
          <div className="team">
            <h3>Team 2</h3>
            <ul>
              {result.team2.players.map((name) => (
                <li key={name}>{name}</li>
              ))}
            </ul>
          </div>
        </div>
      )}
    </section>
  )
}
