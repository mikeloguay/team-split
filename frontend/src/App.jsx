import { useState, useEffect, useCallback } from 'react'
import Players from './components/Players'
import Splitter from './components/Splitter'
import Login from './components/Login'
import { getPlayers, setAuthToken } from './api'

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

function parseJwt(token) {
  const payload = token.split('.')[1]
  return JSON.parse(atob(payload.replace(/-/g, '+').replace(/_/g, '/')))
}

export default function App() {
  const [path, navigate] = usePath()
  const [players, setPlayers] = useState([])
  const [playersLoading, setPlayersLoading] = useState(false)
  const [error, setError] = useState(null)
  const [user, setUser] = useState(null)

  const logout = useCallback(() => {
    setUser(null)
    setAuthToken(null)
    setPlayers([])
  }, [])

  const loadPlayers = useCallback(async () => {
    setPlayersLoading(true)
    try {
      const data = await getPlayers()
      setPlayers(data)
      setError(null)
    } catch (e) {
      if (e.message === 'UNAUTHORIZED') logout()
      else setError(e.message)
    } finally {
      setPlayersLoading(false)
    }
  }, [logout])

  useEffect(() => {
    if (user) loadPlayers()
  }, [user, loadPlayers])

  function handleLogin({ credential }) {
    const payload = parseJwt(credential)
    setUser({ name: payload.name, picture: payload.picture })
    setAuthToken(credential)
  }

  if (!user) {
    return <Login onLogin={handleLogin} />
  }

  return (
    <div className="app">
      <header>
        <h1>Team split</h1>
        <nav>
          <button className={path === '/' ? 'active' : ''} onClick={() => navigate('/')}>
            Dividir
          </button>
          <button className={path === '/players' ? 'active' : ''} onClick={() => navigate('/players')}>
            Jugadores
          </button>
        </nav>
        <div className="user-info">
          {user.picture && <img src={user.picture} alt={user.name} className="avatar" />}
          <button onClick={logout}>Salir</button>
        </div>
      </header>

      {error && <p className="error">{error}</p>}

      {playersLoading && players.length === 0 ? (
        <div className="loading-state">
          <div className="spinner" />
          Cargando jugadores…
        </div>
      ) : (
        <>
          {path === '/players' && <Players players={players} onRefresh={loadPlayers} />}
          {path !== '/players' && <Splitter players={players} />}
        </>
      )}
    </div>
  )
}
