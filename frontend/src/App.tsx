import { useState, useEffect, useCallback } from 'react'
import { Menu } from 'lucide-react'
import type { CredentialResponse } from '@react-oauth/google'
import Players from './components/Players'
import Splitter from './components/Splitter'
import Login from './components/Login'
import { getPlayers, setAuthToken, type Player } from './api'

function usePath() {
  const [path, setPath] = useState(window.location.pathname)

  useEffect(() => {
    const onPop = () => setPath(window.location.pathname)
    window.addEventListener('popstate', onPop)
    return () => window.removeEventListener('popstate', onPop)
  }, [])

  function navigate(to: string) {
    window.history.pushState({}, '', to)
    setPath(to)
  }

  return [path, navigate] as const
}

interface User {
  name: string
  picture: string
}

interface JwtPayload {
  name: string
  picture: string
}

function parseJwt(token: string): JwtPayload {
  const payload = token.split('.')[1]
  const bytes = Uint8Array.from(
    atob(payload.replace(/-/g, '+').replace(/_/g, '/')),
    (c) => c.charCodeAt(0),
  )
  return JSON.parse(new TextDecoder().decode(bytes)) as JwtPayload
}

export default function App() {
  const [path, navigate] = usePath()
  const [players, setPlayers] = useState<Player[]>([])
  const [playersLoading, setPlayersLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [user, setUser] = useState<User | null>(null)
  const [menuOpen, setMenuOpen] = useState(false)
  const [navOpen, setNavOpen] = useState(false)

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
      if (e instanceof Error && e.message === 'UNAUTHORIZED') logout()
      else setError(e instanceof Error ? e.message : 'Error desconocido')
    } finally {
      setPlayersLoading(false)
    }
  }, [logout])

  useEffect(() => {
    if (user) loadPlayers()
  }, [user, loadPlayers])

  useEffect(() => {
    if (!menuOpen) return
    const close = () => setMenuOpen(false)
    document.addEventListener('click', close)
    return () => document.removeEventListener('click', close)
  }, [menuOpen])

  useEffect(() => {
    if (!navOpen) return
    const close = () => setNavOpen(false)
    document.addEventListener('click', close)
    return () => document.removeEventListener('click', close)
  }, [navOpen])

  function goTo(to: string) {
    navigate(to)
    setNavOpen(false)
  }

  function handleLogin({ credential }: CredentialResponse) {
    if (!credential) return
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
        <h1 className="site-title" onClick={() => navigate('/')}>Team split</h1>
        <div className="header-right">
          <div className="nav-menu">
            <button
              className="nav-menu-trigger"
              onClick={(e) => { e.stopPropagation(); setNavOpen((v) => !v) }}
              aria-label="Navegación"
            >
              <Menu size={24} />
            </button>
            {navOpen && (
              <div className="nav-menu-dropdown">
                <button className={path === '/' ? 'active' : ''} onClick={() => goTo('/')}>
                  Home
                </button>
                <button className={path === '/players' ? 'active' : ''} onClick={() => goTo('/players')}>
                  Mis jugadores
                </button>
              </div>
            )}
          </div>
          <div className="user-menu">
            <button
              className="user-menu-trigger"
              onClick={(e) => { e.stopPropagation(); setMenuOpen((v) => !v) }}
              aria-label="Menú de usuario"
            >
              {user.picture && <img src={user.picture} alt={user.name} className="avatar" />}
            </button>
            {menuOpen && (
              <div className="user-menu-dropdown">
                <span className="user-menu-name">{user.name}</span>
                <hr className="user-menu-divider" />
                <button onClick={logout}>Salir</button>
              </div>
            )}
          </div>
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
          {path === '/players' && <Players players={players} onRefresh={loadPlayers} onGoHome={() => goTo('/')} />}
          {path !== '/players' && <Splitter players={players} onGoToPlayers={() => goTo('/players')} />}
        </>
      )}
    </div>
  )
}
