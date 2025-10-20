import { useState, useEffect } from 'react'
import Login from './components/Login'
import Register from './components/Register'
import Dashboard from './components/Dashboard'
import './App.css'

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(false)
  const [showRegister, setShowRegister] = useState(false)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    // Verificar se o usuário já está autenticado
    const token = localStorage.getItem('authToken')
    if (token) {
      setIsAuthenticated(true)
    }
    setLoading(false)
  }, [])

  const handleLoginSuccess = () => {
    setIsAuthenticated(true)
    setShowRegister(false)
  }

  const handleRegisterSuccess = () => {
    setShowRegister(false)
    // Após registrar com sucesso, mostrar login
  }

  const handleLogout = () => {
    localStorage.removeItem('authToken')
    localStorage.removeItem('user')
    setIsAuthenticated(false)
    setShowRegister(false)
  }

  if (loading) {
    return <div className="loading">Carregando...</div>
  }

  if (!isAuthenticated) {
    return showRegister ? (
      <Register
        onRegisterSuccess={handleRegisterSuccess}
        onSwitchToLogin={() => setShowRegister(false)}
      />
    ) : (
      <Login
        onLoginSuccess={handleLoginSuccess}
        onSwitchToRegister={() => setShowRegister(true)}
      />
    )
  }

  return <Dashboard onLogout={handleLogout} />
}

export default App

