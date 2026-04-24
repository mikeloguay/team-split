import { useState, useEffect, useCallback } from 'react'
import Players from './components/Players'
import Splitter from './components/Splitter'
import { getPlayers } from './api'

function usePath() {
  const [path, setPath] = useState(window.location.pathname)

  useEffect(() => {
    const onPop = () => setPath(window.location.pathname)
    window.addEventListener('popstate', onPop)
    return () => window.removeEventListener('popstate', onPop)
  }, [])

  function navigate(to) {
    window.history.pushState({}, '', to)
    setPath(to)
  }

  return [path, navigate]
}

export default function App() {
  const [path, navigate] = usePath()
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
          <button className={path === '/' ? 'active' : ''} onClick={() => navigate('/')}>
            Dividir
          </button>
        </nav>
      </header>

      {error && <p className="error">{error}</p>}

      {path === '/players' && <Players players={players} onRefresh={loadPlayers} />}
      {path !== '/players' && <Splitter players={players} />}
    </div>
  )
}
