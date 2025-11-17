import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import Endereco from './Views/Endereco';
import Cliente from './Views/Cliente';
import Fornecedor from './Views/Fornecedor';
import './App.css';

function App() {
  return (
    <BrowserRouter>
      <div className="App" style={{ minHeight: '100vh', backgroundColor: '#f9fafb' }}>
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
          </ul>
        </nav>

        <Routes>
          <Route path="/" element={<Endereco />} />
          <Route path="/endereco" element={<Endereco />} />
          <Route path="/cliente" element={<Cliente />} />
          <Route path="/fornecedor" element={<Fornecedor />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
