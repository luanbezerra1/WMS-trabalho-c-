import React, { useEffect, useState } from "react";
import FornecedorModel from "../Models/Fornecedor";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/Fornecedor.css";
import FormFornecedor from "./Components/FormFornecedor";

function Fornecedor() {
  const [fornecedores, setFornecedores] = useState<FornecedorModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [fornecedorEditando, setFornecedorEditando] = useState<FornecedorModel | null>(null);
  const [isEditMode, setIsEditMode] = useState(false);

  useEffect(() => {
    let isMounted = true;
    
    async function loadData() {
      try {
        setLoading(true);
        setError(null);
        const resposta = await axios.get<FornecedorModel[]>(
          "http://localhost:5209/api/GetFornecedor"
        );
        if (isMounted) {
          const dados = resposta.data;
          setFornecedores(dados);
          setLoading(false);
        }
      } catch (error: any) {
        console.log("Erro na requisição: " + error);
        if (isMounted) {
          setError("Erro ao carregar fornecedores. Verifique se o servidor está rodando.");
          setLoading(false);
        }
      }
    }

    loadData();

    const timeout = setTimeout(() => {
      if (isMounted) {
        setLoading(false);
        setError("Timeout ao carregar fornecedores. Verifique se o servidor está rodando.");
      }
    }, 10000);

    return () => {
      isMounted = false;
      clearTimeout(timeout);
    };
  }, []);

  async function listarFornecedoresAPI() {
    try {
      setLoading(true);
      setError(null);
      const resposta = await axios.get<FornecedorModel[]>(
        "http://localhost:5209/api/GetFornecedor"
      );
      const dados = resposta.data;
      setFornecedores(dados);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição: " + error);
      setError("Erro ao carregar fornecedores. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  async function deletarFornecedor(id: number) {
    if (window.confirm("Tem certeza que deseja excluir este fornecedor?")) {
      try {
        await axios.delete(`http://localhost:5209/api/DeleteFornecedor=${id}`);
        listarFornecedoresAPI();
      } catch (error) {
        console.log("Erro ao deletar fornecedor: " + error);
        alert("Erro ao deletar fornecedor. Tente novamente.");
      }
    }
  }

  function editarFornecedor(id: number) {
    const fornecedor = fornecedores.find((f) => f.id === id);
    if (fornecedor) {
      setFornecedorEditando(fornecedor);
      setIsEditMode(true);
      setShowForm(true);
    }
  }

  function adicionarFornecedor() {
    setFornecedorEditando(null);
    setIsEditMode(false);
    setShowForm(true);
  }

  function fecharFormulario() {
    setShowForm(false);
    setFornecedorEditando(null);
    setIsEditMode(false);
  }

  function handleFormSuccess() {
    listarFornecedoresAPI();
    fecharFormulario();
  }

  if (loading) {
    return (
      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Fornecedores</h1>
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
        <FormFornecedor
          fornecedor={fornecedorEditando}
          onClose={fecharFormulario}
          onSuccess={handleFormSuccess}
          isEdit={isEditMode}
        />
      )}

      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Fornecedores</h1>
          <button className="btn-adicionar" onClick={adicionarFornecedor}>
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
            <th>Email</th>
            <th>Telefone</th>
            <th>CNPJ</th>
            <th>Endereço ID</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {fornecedores.length === 0 ? (
            <tr>
              <td colSpan={7} className="no-data">
                Nenhum fornecedor encontrado
              </td>
            </tr>
          ) : (
            fornecedores.map((fornecedor) => (
              <tr key={fornecedor.id}>
                <td>{fornecedor.id}</td>
                <td>{fornecedor.nome}</td>
                <td>{fornecedor.email}</td>
                <td>{fornecedor.telefone}</td>
                <td>{fornecedor.cnpj}</td>
                <td>{fornecedor.enderecoId}</td>
                <td className="acoes">
                  <button
                    className="btn-editar"
                    onClick={() => editarFornecedor(fornecedor.id)}
                  >
                    Editar
                  </button>
                  <button
                    className="btn-apagar"
                    onClick={() => deletarFornecedor(fornecedor.id)}
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

export default Fornecedor;

