import { useState, useEffect, useCallback } from 'react'
import Players from './components/Players'
import Splitter from './components/Splitter'
import { getPlayers } from './api'

export default function App() {
  const [tab, setTab] = useState('split')
  const [players, setPlayers] = useState([])
  const [error, setError] = useState(null)

  const loadPlayers = useCallback(async () => {
    try {
      const data = await getPlayers()
      setPlayers(data)
      setError(null)
    } catch (e) {
      setError(e.message)
    }
  }, [])

  useEffect(() => {
    loadPlayers()
  }, [loadPlayers])

  return (
    <div className="app">
      <header>
        <h1>Team Split</h1>
        <nav>
          <button className={tab === 'split' ? 'active' : ''} onClick={() => setTab('split')}>
            Split
          </button>
          <button className={tab === 'players' ? 'active' : ''} onClick={() => setTab('players')}>
            Players
          </button>
        </nav>
      </header>

      {error && <p className="error">{error}</p>}

      {tab === 'split' && <Splitter players={players} />}
      {tab === 'players' && <Players players={players} onRefresh={loadPlayers} />}
    </div>
  )
}
