import React, { useState } from 'react';
import './home.css';

interface EntityCard {
  id: string;
  title: string;
  description: string;
  icon: string;
  color: string;
}

const HomePage: React.FC = () => {
  console.log('HomePage component rendered');
  const [selectedMenu, setSelectedMenu] = useState<string>('home');

  const entityCards: EntityCard[] = [
    {
      id: 'endereco',
      title: 'Endereço',
      description: 'Gerencie endereços de clientes, fornecedores e armazéns.',
      icon: '📍',
      color: '#1976d2'
    },
    {
      id: 'cliente',
      title: 'Cliente',
      description: 'Cadastre e gerencie informações dos seus clientes.',
      icon: '👤',
      color: '#1976d2'
    },
    {
      id: 'fornecedor',
      title: 'Fornecedor',
      description: 'Controle seus fornecedores e informações de contato.',
      icon: '🏢',
      color: '#1976d2'
    },
    {
      id: 'produto',
      title: 'Produto',
      description: 'Gerencie seu catálogo de produtos e informações detalhadas.',
      icon: '📦',
      color: '#1976d2'
    },
    {
      id: 'armazem',
      title: 'Armazém',
      description: 'Configure e gerencie seus armazéns e capacidades.',
      icon: '🏭',
      color: '#ffc107'
    },
    {
      id: 'inventario',
      title: 'Inventário',
      description: 'Controle de posições e estoque nos armazéns.',
      icon: '📊',
      color: '#ffc107'
    },
    {
      id: 'entrada',
      title: 'Entrada de Produtos',
      description: 'Registre entradas de produtos no sistema.',
      icon: '⬇️',
      color: '#4caf50'
    },
    {
      id: 'saida',
      title: 'Saída de Produtos',
      description: 'Registre saídas e retiradas de produtos.',
      icon: '⬆️',
      color: '#4caf50'
    },
    {
      id: 'usuario',
      title: 'Usuário',
      description: 'Gerencie usuários e permissões do sistema.',
      icon: '👥',
      color: '#9c27b0'
    },
    {
      id: 'logs',
      title: 'Logs',
      description: 'Visualize relatórios e logs de movimentações.',
      icon: '📋',
      color: '#9c27b0'
    }
  ];

  const handleCardClick = (entityId: string) => {
    console.log(`Clicou em ${entityId}`);
  };

  return (
    <div style={{ 
      display: 'flex', 
      minHeight: '100vh', 
      width: '100%', 
      background: '#0a2540',
      fontFamily: 'Inter, sans-serif'
    }}>
      {/* Sidebar */}
      <aside style={{
        width: '280px',
        background: '#1a1a1a',
        borderRight: '1px solid #333',
        display: 'flex',
        flexDirection: 'column',
        position: 'fixed',
        height: '100vh',
        overflowY: 'auto',
        left: 0,
        top: 0,
        zIndex: 100
      }}>
        <div style={{ padding: '1.5rem 1.25rem', borderBottom: '1px solid #333' }}>
          <h2 style={{ 
            fontSize: '1.5rem', 
            fontWeight: 700, 
            margin: 0,
            display: 'flex',
            alignItems: 'center',
            gap: '0.5rem'
          }}>
            <span style={{ color: '#ffffff' }}>WMS</span>
            <span style={{ color: '#999', fontWeight: 400 }}>System</span>
          </h2>
        </div>
        
        <nav style={{ padding: '1rem 0', display: 'flex', flexDirection: 'column' }}>
          {['Home', 'Referência da API', 'Guia', 'Logs'].map((item) => (
            <button
              key={item}
              onClick={() => setSelectedMenu(item.toLowerCase().replace(' ', '-'))}
              style={{
                background: selectedMenu === item.toLowerCase().replace(' ', '-') ? '#2a2a2a' : 'transparent',
                border: 'none',
                color: selectedMenu === item.toLowerCase().replace(' ', '-') ? '#1976d2' : '#ccc',
                padding: '0.75rem 1.25rem',
                textAlign: 'left',
                cursor: 'pointer',
                fontSize: '0.95rem',
                borderLeft: `3px solid ${selectedMenu === item.toLowerCase().replace(' ', '-') ? '#1976d2' : 'transparent'}`,
                width: '100%'
              }}
            >
              {item}
            </button>
          ))}
        </nav>
      </aside>

      {/* Main Content */}
      <main style={{
        flex: 1,
        marginLeft: '280px',
        background: '#0a2540',
        padding: '2rem 3rem',
        minHeight: '100vh',
        width: 'calc(100% - 280px)'
      }}>
        <div style={{ marginBottom: '2rem' }}>
          <h1 style={{ 
            fontSize: '3rem', 
            fontWeight: 700, 
            color: '#ffffff', 
            marginBottom: '1rem',
            marginTop: 0
          }}>
            Bem-vindo ao WMS!
          </h1>
          <p style={{ 
            fontSize: '1.125rem', 
            color: '#b0b0b0', 
            lineHeight: 1.6,
            margin: 0
          }}>
            Gerencie seu armazém de forma eficiente e organize seus produtos, clientes e fornecedores.
          </p>
        </div>

        <div style={{ marginBottom: '2.5rem' }}>
          <input
            type="text"
            placeholder="Pesquisar (Ctrl + K)"
            style={{
              width: '100%',
              maxWidth: '500px',
              padding: '0.75rem 1rem',
              background: '#1a1a1a',
              border: '1px solid #333',
              borderRadius: '6px',
              color: '#fff',
              fontSize: '0.95rem'
            }}
          />
        </div>

        <div style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
          gap: '1.5rem',
          marginTop: '1rem'
        }}>
          {entityCards.map((card) => (
            <div
              key={card.id}
              onClick={() => handleCardClick(card.id)}
              style={{
                background: '#1a1a1a',
                borderRadius: '8px',
                padding: '1.5rem',
                cursor: 'pointer',
                borderTop: `4px solid ${card.color}`,
                border: '1px solid #333',
                transition: 'all 0.3s ease'
              }}
              onMouseEnter={(e) => {
                e.currentTarget.style.transform = 'translateY(-4px)';
                e.currentTarget.style.boxShadow = '0 8px 24px rgba(0, 0, 0, 0.3)';
                e.currentTarget.style.borderColor = card.color;
                e.currentTarget.style.background = '#222';
              }}
              onMouseLeave={(e) => {
                e.currentTarget.style.transform = 'translateY(0)';
                e.currentTarget.style.boxShadow = 'none';
                e.currentTarget.style.borderColor = '#333';
                e.currentTarget.style.background = '#1a1a1a';
              }}
            >
              <div style={{ fontSize: '2.5rem', marginBottom: '1rem', color: card.color }}>
                {card.icon}
              </div>
              <h3 style={{ 
                fontSize: '1.25rem', 
                fontWeight: 600, 
                color: '#ffffff', 
                marginBottom: '0.75rem',
                marginTop: 0
              }}>
                {card.title}
              </h3>
              <p style={{ 
                fontSize: '0.95rem', 
                color: '#b0b0b0', 
                lineHeight: 1.5,
                margin: 0
              }}>
                {card.description}
              </p>
            </div>
          ))}
        </div>
      </main>
    </div>
  );
};

export default HomePage;
