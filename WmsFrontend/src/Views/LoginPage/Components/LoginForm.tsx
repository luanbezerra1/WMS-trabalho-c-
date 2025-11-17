import React from 'react';
import { useNavigate } from 'react-router-dom';

const LoginForm: React.FC = () => {
  const navigate = useNavigate();

  const handleLogin = () => {
    // Simulação de login - você pode implementar a lógica real depois
    localStorage.setItem('wms-user', 'true');
    navigate('/home');
  };

  return (
    <div style={{ 
      display: 'flex', 
      justifyContent: 'center', 
      alignItems: 'center', 
      minHeight: '100vh',
      background: '#0a2540'
    }}>
      <button 
        onClick={handleLogin}
        style={{
          padding: '1rem 2rem',
          fontSize: '1.2rem',
          background: '#1976d2',
          color: 'white',
          border: 'none',
          borderRadius: '8px',
          cursor: 'pointer'
        }}
      >
        Entrar no Sistema
      </button>
    </div>
  );
};

export default LoginForm;

