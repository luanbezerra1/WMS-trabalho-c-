import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import Endereco from './Views/Endereco';
import Cliente from './Views/Cliente';
import Fornecedor from './Views/Fornecedor';
import Armazem from './Views/Armazem';
import Inventario from './Views/Inventario';
import SaidaProduto from './Views/SaidaProduto';
import EntradaProduto from './Views/EntradaProduto';
import Usuario from './Views/Usuario';
import Logs from './Views/Logs';
import { RefreshProvider, useRefresh } from './Contexts/RefreshContext';
import './App.css';

function NavWithRefresh() {
  const { refreshFunction } = useRefresh();

  return (
    <nav>
      <ul>
        <li>
          <Link to="/endereco">Endereços</Link>
        </li>
        <li>
          <Link to="/cliente">Clientes</Link>
        </li>
        <li>
          <Link to="/fornecedor">Fornecedores</Link>
        </li>
        <li>
          <Link to="/armazem">Armazéns</Link>
        </li>
        <li>
          <Link to="/inventario">Posições</Link>
        </li>
        <li>
          <Link to="/entrada-produto">Entrada Produto</Link>
        </li>
        <li>
          <Link to="/saida-produto">Saída Produto</Link>
        </li>
        <li>
          <Link to="/usuario">Usuários</Link>
        </li>
        <li>
          <Link to="/logs">Logs</Link>
        </li>
        <li style={{ marginLeft: 'auto' }}>
          <button 
            onClick={() => refreshFunction?.()} 
            className="btn-refresh"
            disabled={!refreshFunction}
          >
            🔄 Atualizar
          </button>
        </li>
      </ul>
    </nav>
  );
}

function App() {
  return (
    <BrowserRouter>
      <RefreshProvider>
        <div className="App" style={{ minHeight: '100vh', backgroundColor: '#f9fafb' }}>
          <NavWithRefresh />

          <Routes>
            <Route path="/" element={<Endereco />} />
            <Route path="/endereco" element={<Endereco />} />
            <Route path="/cliente" element={<Cliente />} />
            <Route path="/fornecedor" element={<Fornecedor />} />
            <Route path="/armazem" element={<Armazem />} />
            <Route path="/inventario" element={<Inventario />} />
            <Route path="/entrada-produto" element={<EntradaProduto />} />
            <Route path="/saida-produto" element={<SaidaProduto />} />
            <Route path="/usuario" element={<Usuario />} />
            <Route path="/logs" element={<Logs />} />
          </Routes>
        </div>
      </RefreshProvider>
    </BrowserRouter>
  );
}

export default App;
