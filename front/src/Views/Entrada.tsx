import React, { useEffect, useState } from "react";
import EntradaModel from "../Models/Entrada";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/Entrada.css";
import FormEntrada from "./Components/FormEntrada";

function Entrada() {
  const [entradas, setEntradas] = useState<EntradaModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [entradaEditando, setEntradaEditando] = useState<EntradaModel | null>(null);
  const [isEditMode, setIsEditMode] = useState(false);

  // Carrega as entradas ao abrir a tela
  useEffect(() => {
    let isMounted = true;

    async function loadData() {
      try {
        setLoading(true);
        setError(null);

        const resposta = await axios.get<EntradaModel[]>(
          "http://localhost:5209/api/GetEntradaProduto"
        );

        if (isMounted) {
          setEntradas(resposta.data);
          setLoading(false);
        }
      } catch (error: any) {
        console.log("Erro na requisição de entradas: " + error);
        if (isMounted) {
          setError("Erro ao carregar entradas. Verifique se o servidor está rodando.");
          setLoading(false);
        }
      }
    }

    loadData();

    // timeout de segurança
    const timeout = setTimeout(() => {
      if (isMounted && loading) {
        setLoading(false);
        setError("Timeout ao carregar entradas. Verifique se o servidor está rodando.");
      }
    }, 10000);

    return () => {
      isMounted = false;
      clearTimeout(timeout);
    };
  }, []);

  // Recarregar as entradas (usado após criar/editar/deletar)
  async function listarEntradasAPI() {
    try {
      setLoading(true);
      setError(null);
      const resposta = await axios.get<EntradaModel[]>(
        "http://localhost:5209/api/GetEntradaProduto"
      );
      setEntradas(resposta.data);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição de entradas: " + error);
      setError("Erro ao carregar entradas. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  // Deletar entrada (usa DELETE /api/DeleteEntradaProduto=entradaId&produtoId)
  async function deletarEntrada(entradaId: number, produtoId: number) {
    if (window.confirm("Tem certeza que deseja excluir esta entrada?")) {
      try {
        await axios.delete(
          `http://localhost:5209/api/DeleteEntradaProduto=${entradaId}&${produtoId}`
        );
        listarEntradasAPI();
      } catch (error: any) {
        console.log("Erro ao deletar entrada: " + error);
        alert("Erro ao deletar entrada. Tente novamente.");
      }
    }
  }

  // Editar (se você quiser permitir edição de entrada)
  function editarEntrada(id: number, produtoId: number) {
    const entrada = entradas.find(
      (e) => e.entradaId === id && e.produtoId === produtoId
    );
    if (entrada) {
      setEntradaEditando(entrada);
      setIsEditMode(true);
      setShowForm(true);
    }
  }

  // Abrir formulário para nova entrada
  function adicionarEntrada() {
    setEntradaEditando(null);
    setIsEditMode(false);
    setShowForm(true);
  }

  function fecharFormulario() {
    setShowForm(false);
    setEntradaEditando(null);
    setIsEditMode(false);
  }

  function handleFormSuccess() {
    listarEntradasAPI();
    fecharFormulario();
  }

  if (loading) {
    return (
      <div id="componente_listar_entradas">
        <div className="header-container">
          <h1>Listar Entradas</h1>
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
        <FormEntrada
          entrada={entradaEditando}
          onClose={fecharFormulario}
          onSuccess={handleFormSuccess}
          isEdit={isEditMode}
        />
      )}

      <div id="componente_listar_entradas">
        <div className="header-container">
          <h1>Listar Entradas de Produto</h1>
          <button className="btn-adicionar" onClick={adicionarEntrada}>
            + Adicionar Entrada
          </button>
        </div>

        {error && <div className="error-container">{error}</div>}

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
              entradas.map((entrada) => (
                <tr key={`${entrada.entradaId}-${entrada.produtoId}`}>
                  <td>{entrada.entradaId}</td>
                  <td>{entrada.fornecedorId}</td>
                  <td>{entrada.produtoId}</td>
                  <td>{entrada.quantidadeRecebida}</td>
                  <td>
                    {entrada.dataentrada
                      ? new Date(entrada.dataentrada).toLocaleString("pt-BR")
                      : "-"}
                  </td>
                  <td className="acoes">
                    {/* Se não quiser editar, pode comentar esse botão */}
                    <button
                      className="btn-editar"
                      onClick={() =>
                        editarEntrada(entrada.entradaId, entrada.produtoId)
                      }
                    >
                      Editar
                    </button>
                    <button
                      className="btn-apagar"
                      onClick={() =>
                        deletarEntrada(entrada.entradaId, entrada.produtoId)
                      }
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

export default Entrada;