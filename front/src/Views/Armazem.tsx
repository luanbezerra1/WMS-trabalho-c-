import React, { useEffect, useState } from "react";
import ArmazemModel from "../Models/Armazem";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/Armazem.css";
import FormArmazem from "./Components/FormArmazem";
import { useRefresh } from "../Contexts/RefreshContext";

function Armazem() {
  const [armazens, setArmazens] = useState<ArmazemModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [armazemEditando, setArmazemEditando] = useState<ArmazemModel | null>(null);
  const [isEditMode, setIsEditMode] = useState(false);
  const { setRefreshFunction } = useRefresh();

  async function listarArmazensAPI() {
    try {
      setLoading(true);
      setError(null);
      const resposta = await axios.get<ArmazemModel[]>(
        "http://localhost:5209/api/GetArmazem"
      );
      const dados = resposta.data;
      setArmazens(dados);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição: " + error);
      setError("Erro ao carregar armazéns. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  useEffect(() => {
    listarArmazensAPI();
    setRefreshFunction(() => listarArmazensAPI);
    
    return () => {
      setRefreshFunction(null);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  async function deletarArmazem(id: number) {
    if (window.confirm("Tem certeza que deseja excluir este armazém? Todas as posições serão removidas.")) {
      try {
        await axios.delete(`http://localhost:5209/api/DeleteArmazem=${id}`);
        listarArmazensAPI();
      } catch (error) {
        console.log("Erro ao deletar armazém: " + error);
        alert("Erro ao deletar armazém. Tente novamente.");
      }
    }
  }

  function editarArmazem(id: number) {
    const armazem = armazens.find((a) => a.id === id);
    if (armazem) {
      setArmazemEditando(armazem);
      setIsEditMode(true);
      setShowForm(true);
    }
  }

  function adicionarArmazem() {
    setArmazemEditando(null);
    setIsEditMode(false);
    setShowForm(true);
  }

  function fecharFormulario() {
    setShowForm(false);
    setArmazemEditando(null);
    setIsEditMode(false);
  }

  function handleFormSuccess() {
    listarArmazensAPI();
    fecharFormulario();
  }

  if (loading) {
    return (
      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Armazéns</h1>
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
        <FormArmazem
          armazem={armazemEditando}
          onClose={fecharFormulario}
          onSuccess={handleFormSuccess}
          isEdit={isEditMode}
        />
      )}

      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Armazéns</h1>
          <button className="btn-adicionar" onClick={adicionarArmazem}>
            + Adicionar
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
            <th>#</th>
            <th>Nome</th>
            <th>Status</th>
            <th>Posições</th>
            <th>Produtos/Posição</th>
            <th>Capacidade Total</th>
            <th>Endereço ID</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {armazens.length === 0 ? (
            <tr>
              <td colSpan={8} className="no-data">
                Nenhum armazém encontrado
              </td>
            </tr>
          ) : (
            armazens.map((armazem) => (
              <tr key={armazem.id}>
                <td>{armazem.id}</td>
                <td>{armazem.nomeArmazem}</td>
                <td>{armazem.status}</td>
                <td>{armazem.posicoes}</td>
                <td>{armazem.produtoPosicao}</td>
                <td>{armazem.capacidade}</td>
                <td>{armazem.enderecoId}</td>
                <td className="acoes">
                  <button
                    className="btn-editar"
                    onClick={() => editarArmazem(armazem.id)}
                  >
                    Editar
                  </button>
                  <button
                    className="btn-apagar"
                    onClick={() => deletarArmazem(armazem.id)}
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

export default Armazem;

