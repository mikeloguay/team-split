import { GoogleLogin } from '@react-oauth/google'

export default function Login({ onLogin }) {
  return (
    <div className="login-screen">
      <div className="login-card">
        <h1>Team split</h1>
        <p>Accede con tu cuenta de Google</p>
        <GoogleLogin onSuccess={onLogin} />
      </div>
    </div>
  )
}
