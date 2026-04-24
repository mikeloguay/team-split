import { useState } from 'react'
import { createPlayer, updatePlayer, deletePlayer } from '../api'

export default function Players({ players, onRefresh }) {
  const [name, setName] = useState('')
  const [level, setLevel] = useState('')
  const [editing, setEditing] = useState(null)
  const [editLevel, setEditLevel] = useState('')
  const [error, setError] = useState(null)

  async function handleAdd(e) {
    e.preventDefault()
    try {
      await createPlayer(name.trim(), Number(level))
      setName('')
      setLevel('')
      setError(null)
      await onRefresh()
    } catch (e) {
      setError(e.message)
    }
  }

  function startEdit(player) {
    setEditing(player)
    setEditLevel(String(player.level))
  }

  async function handleUpdate(e) {
    e.preventDefault()
    try {
      await updatePlayer(editing.name, Number(editLevel))
      setEditing(null)
      setError(null)
      await onRefresh()
    } catch (e) {
      setError(e.message)
    }
  }

  async function handleDelete(playerName) {
    try {
      await deletePlayer(playerName)
      setError(null)
      await onRefresh()
    } catch (e) {
      setError(e.message)
    }
  }

  return (
    <section>
      <h2>Jugadores</h2>

      {error && <p className="error">{error}</p>}

      <form onSubmit={handleAdd} className="add-form">
        <input
          placeholder="Nombre"
          value={name}
          onChange={(e) => setName(e.target.value)}
          required
        />
        <input
          type="number"
          placeholder="Nivel (1–100)"
          min={1}
          max={100}
          value={level}
          onChange={(e) => setLevel(e.target.value)}
          required
        />
        <button type="submit">Añadir</button>
      </form>

      <table>
        <thead>
          <tr>
            <th>Nombre</th>
            <th>Nivel</th>
            <th />
          </tr>
        </thead>
        <tbody>
          {players.map((p) =>
            editing?.name === p.name ? (
              <tr key={p.name}>
                <td>{p.name}</td>
                <td>
                  <form onSubmit={handleUpdate} className="edit-form">
                    <input
                      type="number"
                      min={1}
                      max={100}
                      value={editLevel}
                      onChange={(e) => setEditLevel(e.target.value)}
                      required
                      autoFocus
                    />
                    <button type="submit">Guardar</button>
                    <button type="button" onClick={() => setEditing(null)}>Cancelar</button>
                  </form>
                </td>
                <td />
              </tr>
            ) : (
              <tr key={p.name}>
                <td>{p.name}</td>
                <td>{p.level}</td>
                <td className="row-actions">
                  <button onClick={() => startEdit(p)}>Editar</button>
                  <button className="danger" onClick={() => handleDelete(p.name)}>Eliminar</button>
                </td>
              </tr>
            ),
          )}
        </tbody>
      </table>
    </section>
  )
}
