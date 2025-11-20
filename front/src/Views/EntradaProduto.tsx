import React, { useEffect, useState } from "react";
import EntradaProdutoModel from "../Models/EntradaProduto";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/EntradaProduto.css";
import FormEntradaProduto from "./Components/FormEntradaProduto";
import { useRefresh } from "../Contexts/RefreshContext";

function EntradaProduto() {
  const [entradas, setEntradas] = useState<EntradaProdutoModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const { setRefreshFunction } = useRefresh();

  async function listarEntradasAPI() {
    try {
      setLoading(true);
      setError(null);
      const resposta = await axios.get<EntradaProdutoModel[]>(
        "http://localhost:5209/api/GetEntradaProduto"
      );
      const dados = resposta.data;
      setEntradas(dados);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição: " + error);
      setError("Erro ao carregar entradas. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  useEffect(() => {
    listarEntradasAPI();
    setRefreshFunction(() => listarEntradasAPI);
    
    return () => {
      setRefreshFunction(null);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  async function deletarEntrada(entradaId: number, produtoId: number) {
    if (window.confirm("Tem certeza que deseja excluir este registro de entrada?")) {
      try {
        await axios.delete(`http://localhost:5209/api/DeleteEntradaProduto=${entradaId}&${produtoId}`);
        listarEntradasAPI();
      } catch (error) {
        console.log("Erro ao deletar entrada: " + error);
        alert("Erro ao deletar entrada. Tente novamente.");
      }
    }
  }

  function adicionarEntrada() {
    setShowForm(true);
  }

  function fecharFormulario() {
    setShowForm(false);
  }

  function handleFormSuccess() {
    listarEntradasAPI();
    fecharFormulario();
  }

  if (loading) {
    return (
      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Entradas de Produtos</h1>
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
        <FormEntradaProduto
          onClose={fecharFormulario}
          onSuccess={handleFormSuccess}
        />
      )}

      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Entradas de Produtos</h1>
          <button className="btn-adicionar" onClick={adicionarEntrada}>
            + Registrar Entrada
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
            <th>Entrada ID</th>
            <th>Fornecedor ID</th>
            <th>Produto ID</th>
            <th>Quantidade Recebida</th>
            <th>Data Entrada</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {entradas.length === 0 ? (
            <tr>
              <td colSpan={6} className="no-data">
                Nenhuma entrada encontrada
              </td>
            </tr>
          ) : (
            entradas.map((entrada, index) => (
              <tr key={`${entrada.entradaId}-${entrada.produtoId}-${index}`}>
                <td>{entrada.entradaId}</td>
                <td>{entrada.fornecedorId}</td>
                <td>{entrada.produtoId}</td>
                <td>{entrada.quantidadeRecebida}</td>
                <td>{new Date(entrada.dataEntrada).toLocaleString('pt-BR')}</td>
                <td className="acoes">
                  <button
                    className="btn-apagar"
                    onClick={() => deletarEntrada(entrada.entradaId, entrada.produtoId)}
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

export default EntradaProduto;
