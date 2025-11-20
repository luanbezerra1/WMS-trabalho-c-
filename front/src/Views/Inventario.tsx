import React, { useEffect, useState } from "react";
import InventarioModel from "../Models/Inventario";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/Inventario.css";
import { useRefresh } from "../Contexts/RefreshContext";

function Inventario() {
  const [inventarios, setInventarios] = useState<InventarioModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [armazemFiltro, setArmazemFiltro] = useState<number | null>(null);
  const [armazens, setArmazens] = useState<any[]>([]);
  const { setRefreshFunction } = useRefresh();

  async function loadArmazens() {
    try {
      const armazensResposta = await axios.get("http://localhost:5209/api/GetArmazem");
      setArmazens(armazensResposta.data);
    } catch (error) {
      console.log("Erro ao carregar armazéns: " + error);
    }
  }

  async function listarInventarioAPI() {
    try {
      setLoading(true);
      setError(null);
      const url = armazemFiltro 
        ? `http://localhost:5209/api/GetInventarioByArmazem=${armazemFiltro}`
        : "http://localhost:5209/api/GetInventario";
      const resposta = await axios.get<InventarioModel[]>(url);
      const dados = resposta.data;
      setInventarios(dados);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição: " + error);
      setError("Erro ao carregar posições. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  useEffect(() => {
    loadArmazens();
    listarInventarioAPI();
    setRefreshFunction(() => listarInventarioAPI);
    
    return () => {
      setRefreshFunction(null);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    listarInventarioAPI();
  }, [armazemFiltro]);

  if (loading) {
    return (
      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Posições (Inventário)</h1>
        </div>
        <div className="loading-container">
          <p>Carregando...</p>
        </div>
      </div>
    );
  }

  const inventariosFiltrados = armazemFiltro 
    ? inventarios.filter(i => i.armazemId === armazemFiltro)
    : inventarios;

  return (
    <div id="componente_listar_enderecos">
      <div className="header-container">
        <h1>Listar Posições (Inventário)</h1>
        <div style={{ display: 'flex', gap: '12px', alignItems: 'center' }}>
          <select
            value={armazemFiltro || ""}
            onChange={(e) => setArmazemFiltro(e.target.value ? parseInt(e.target.value) : null)}
            style={{
              padding: '8px 12px',
              borderRadius: '8px',
              border: '2px solid #e5e7eb',
              fontSize: '0.9rem'
            }}
          >
            <option value="">Todos os armazéns</option>
            {armazens.map((armazem) => (
              <option key={armazem.id} value={armazem.id}>
                {armazem.nomeArmazem}
              </option>
            ))}
          </select>
          <button className="btn-adicionar" onClick={listarInventarioAPI}>
            Atualizar
          </button>
        </div>
      </div>

      {error && (
        <div className="error-container">
          {error}
        </div>
      )}

      <table>
        <thead>
          <tr>
            <th>#</th>
            <th>Armazém ID</th>
            <th>Nome Posição</th>
            <th>Produto ID</th>
            <th>Nome Produto</th>
            <th>Quantidade</th>
            <th>Última Movimentação</th>
          </tr>
        </thead>
        <tbody>
          {inventariosFiltrados.length === 0 ? (
            <tr>
              <td colSpan={7} className="no-data">
                Nenhuma posição encontrada
              </td>
            </tr>
          ) : (
            inventariosFiltrados.map((inventario) => (
              <tr key={inventario.id}>
                <td>{inventario.id}</td>
                <td>{inventario.armazemId}</td>
                <td>{inventario.nomePosicao}</td>
                <td>{inventario.produtoId || "-"}</td>
                <td>{inventario.nomeProduto || "Vazio"}</td>
                <td>{inventario.quantidade}</td>
                <td>{new Date(inventario.ultimaMovimentacao).toLocaleString('pt-BR')}</td>
              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
}

export default Inventario;

