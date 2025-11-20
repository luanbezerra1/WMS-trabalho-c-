import React, { useEffect, useState } from "react";
import SaidaProdutoModel from "../Models/SaidaProduto";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/SaidaProduto.css";
import FormSaidaProduto from "./Components/FormSaidaProduto";
import { useRefresh } from "../Contexts/RefreshContext";

function SaidaProduto() {
  const [saidas, setSaidas] = useState<SaidaProdutoModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const { setRefreshFunction } = useRefresh();

  async function listarSaidasAPI() {
    try {
      setLoading(true);
      setError(null);
      const resposta = await axios.get<SaidaProdutoModel[]>(
        "http://localhost:5209/api/GetSaidaProduto"
      );
      const dados = resposta.data;
      setSaidas(dados);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição: " + error);
      setError("Erro ao carregar saídas. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  useEffect(() => {
    listarSaidasAPI();
    setRefreshFunction(() => listarSaidasAPI);
    
    return () => {
      setRefreshFunction(null);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  async function deletarSaida(saidaId: number, produtoId: number) {
    if (window.confirm("Tem certeza que deseja excluir este registro de saída?")) {
      try {
        await axios.delete(`http://localhost:5209/api/DeleteSaidaProduto/${saidaId}/${produtoId}`);
        listarSaidasAPI();
      } catch (error) {
        console.log("Erro ao deletar saída: " + error);
        alert("Erro ao deletar saída. Tente novamente.");
      }
    }
  }

  function adicionarSaida() {
    setShowForm(true);
  }

  function fecharFormulario() {
    setShowForm(false);
  }

  function handleFormSuccess() {
    listarSaidasAPI();
    fecharFormulario();
  }

  if (loading) {
    return (
      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Saídas de Produtos</h1>
        </div>
        <div className="loading-container">
          <p>Carregando...</p>
        </div>
      </div>
    );
  }

  return (
    <>
      {showForm && (
        <FormSaidaProduto
          onClose={fecharFormulario}
          onSuccess={handleFormSuccess}
        />
      )}

      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Saídas de Produtos</h1>
          <button className="btn-adicionar" onClick={adicionarSaida}>
            + Registrar Saída
          </button>
        </div>

        {error && (
          <div className="error-container">
            {error}
          </div>
        )}

      <table>
        <thead>
          <tr>
            <th>Saída ID</th>
            <th>Cliente ID</th>
            <th>Produto ID</th>
            <th>Quantidade Retirada</th>
            <th>Data Saída</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {saidas.length === 0 ? (
            <tr>
              <td colSpan={6} className="no-data">
                Nenhuma saída encontrada
              </td>
            </tr>
          ) : (
            saidas.map((saida, index) => (
              <tr key={`${saida.saidaId}-${saida.produtoId}-${index}`}>
                <td>{saida.saidaId}</td>
                <td>{saida.clienteId}</td>
                <td>{saida.produtoId}</td>
                <td>{saida.quantidadeRetirada}</td>
                <td>{new Date(saida.dataSaida).toLocaleString('pt-BR')}</td>
                <td className="acoes">
                  <button
                    className="btn-apagar"
                    onClick={() => deletarSaida(saida.saidaId, saida.produtoId)}
                  >
                    Apagar
                  </button>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </table>
      </div>
    </>
  );
}

export default SaidaProduto;

