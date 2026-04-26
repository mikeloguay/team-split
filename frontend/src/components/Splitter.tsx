import { useState } from 'react'
import { Shuffle, Users } from 'lucide-react'
import { splitTeams, type Player, type SplitResult } from '../api'

interface SplitterProps {
  players: Player[]
  onGoToPlayers: () => void
}

export default function Splitter({ players, onGoToPlayers }: SplitterProps) {
  const [selected, setSelected] = useState<Set<string>>(new Set())
  const [result, setResult] = useState<SplitResult | null>(null)
  const [error, setError] = useState<string | null>(null)
  const [loading, setLoading] = useState(false)

  function toggle(name: string) {
    setSelected((prev) => {
      const next = new Set(prev)
      next.has(name) ? next.delete(name) : next.add(name)
      return next
    })
  }

  function toggleAll(checked: boolean) {
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
      setError(e instanceof Error ? e.message : 'Error desconocido')
    } finally {
      setLoading(false)
    }
  }

  if (players.length === 0) {
    return (
      <section className="empty-state">
        <Users size={48} strokeWidth={1.5} />
        <h2>No hay jugadores definidos</h2>
        <p>Añade jugadores para poder hacer los equipos.</p>
        <button className="split-btn" onClick={onGoToPlayers}>
          Ir a Mis jugadores
        </button>
      </section>
    )
  }

  return (
    <section>
      <h2>Seleccionar jugadores</h2>

      {error && <p className="error">{error}</p>}

      <label className="select-all-label">
        <input
          type="checkbox"
          checked={allSelected}
          onChange={(e) => toggleAll(e.target.checked)}
        />
        Seleccionar todos ({players.length})
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
        {loading ? <div className="spinner spinner-sm" /> : <Shuffle size={18} />}
        {loading ? 'Haciendo los equipos…' : `Hacer los equpos (${selected.size} jugadores)`}
      </button>

      {result && (
        <div className="teams">
          <div className="team team1">
            <div className="team-header">Equipo 1</div>
            <ul>
              {result.team1.players.map((name) => (
                <li key={name}>{name}</li>
              ))}
            </ul>
          </div>
          <div className="team team2">
            <div className="team-header">Equipo 2</div>
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
