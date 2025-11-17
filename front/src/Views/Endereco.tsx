import React, { useEffect, useState } from "react";
import EnderecoModel from "../Models/Endereco";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/Endereco.css";
import FormEndereco from "./Components/FormEndereco";

function Endereco() {
  const [enderecos, setEnderecos] = useState<EnderecoModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [enderecoEditando, setEnderecoEditando] = useState<EnderecoModel | null>(null);
  const [isEditMode, setIsEditMode] = useState(false);

  useEffect(() => {
    let isMounted = true;
    
    async function loadData() {
      try {
        setLoading(true);
        setError(null);
        const resposta = await axios.get<EnderecoModel[]>(
          "http://localhost:5209/api/GetEndereco"
        );
        if (isMounted) {
          const dados = resposta.data;
          setEnderecos(dados);
          setLoading(false);
        }
      } catch (error: any) {
        console.log("Erro na requisição: " + error);
        if (isMounted) {
          setError("Erro ao carregar endereços. Verifique se o servidor está rodando.");
          setLoading(false);
        }
      }
    }

    loadData();

    // Timeout de segurança
    const timeout = setTimeout(() => {
      if (isMounted) {
        setLoading(false);
        setError("Timeout ao carregar endereços. Verifique se o servidor está rodando.");
      }
    }, 10000);

    return () => {
      isMounted = false;
      clearTimeout(timeout);
    };
  }, []);

  async function listarEnderecosAPI() {
    try {
      setLoading(true);
      setError(null);
      const resposta = await axios.get<EnderecoModel[]>(
        "http://localhost:5209/api/GetEndereco"
      );
      const dados = resposta.data;
      setEnderecos(dados);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição: " + error);
      setError("Erro ao carregar endereços. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  async function deletarEndereco(id: number) {
    if (window.confirm("Tem certeza que deseja excluir este endereço?")) {
      try {
        await axios.delete(`http://localhost:5209/api/DeleteEndereco=${id}`);
        listarEnderecosAPI(); // Recarrega a lista após deletar
      } catch (error) {
        console.log("Erro ao deletar endereço: " + error);
        alert("Erro ao deletar endereço. Tente novamente.");
      }
    }
  }

  function editarEndereco(id: number) {
    const endereco = enderecos.find((e) => e.id === id);
    if (endereco) {
      setEnderecoEditando(endereco);
      setIsEditMode(true);
      setShowForm(true);
    }
  }

  function adicionarEndereco() {
    setEnderecoEditando(null);
    setIsEditMode(false);
    setShowForm(true);
  }

  function fecharFormulario() {
    setShowForm(false);
    setEnderecoEditando(null);
    setIsEditMode(false);
  }

  function handleFormSuccess() {
    listarEnderecosAPI();
    fecharFormulario();
  }

  if (loading) {
    return (
      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Endereços</h1>
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
        <FormEndereco
          endereco={enderecoEditando}
          onClose={fecharFormulario}
          onSuccess={handleFormSuccess}
          isEdit={isEditMode}
        />
      )}

      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Endereços</h1>
          <button className="btn-adicionar" onClick={adicionarEndereco}>
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
            <th>Rua</th>
            <th>Número</th>
            <th>Complemento</th>
            <th>Bairro</th>
            <th>Cidade</th>
            <th>Estado</th>
            <th>CEP</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {enderecos.length === 0 ? (
            <tr>
              <td colSpan={9} className="no-data">
                Nenhum endereço encontrado
              </td>
            </tr>
          ) : (
            enderecos.map((endereco) => (
              <tr key={endereco.id}>
                <td>{endereco.id}</td>
                <td>{endereco.rua}</td>
                <td>{endereco.numero}</td>
                <td>{endereco.complemento}</td>
                <td>{endereco.bairro}</td>
                <td>{endereco.cidade}</td>
                <td>{endereco.estado}</td>
                <td>{endereco.cep}</td>
                <td className="acoes">
                  <button
                    className="btn-editar"
                    onClick={() => editarEndereco(endereco.id)}
                  >
                    Editar
                  </button>
                  <button
                    className="btn-apagar"
                    onClick={() => deletarEndereco(endereco.id)}
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

export default Endereco;

