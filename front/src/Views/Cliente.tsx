import React, { useEffect, useState } from "react";
import ClienteModel from "../Models/Cliente";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/Cliente.css";
import FormCliente from "./Components/FormCliente";

function Cliente() {
  const [clientes, setClientes] = useState<ClienteModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [clienteEditando, setClienteEditando] = useState<ClienteModel | null>(null);
  const [isEditMode, setIsEditMode] = useState(false);

  useEffect(() => {
    let isMounted = true;
    
    async function loadData() {
      try {
        setLoading(true);
        setError(null);
        const resposta = await axios.get<ClienteModel[]>(
          "http://localhost:5209/api/GetCliente"
        );
        if (isMounted) {
          const dados = resposta.data;
          setClientes(dados);
          setLoading(false);
        }
      } catch (error: any) {
        console.log("Erro na requisição: " + error);
        if (isMounted) {
          setError("Erro ao carregar clientes. Verifique se o servidor está rodando.");
          setLoading(false);
        }
      }
    }

    loadData();

    const timeout = setTimeout(() => {
      if (isMounted) {
        setLoading(false);
        setError("Timeout ao carregar clientes. Verifique se o servidor está rodando.");
      }
    }, 10000);

    return () => {
      isMounted = false;
      clearTimeout(timeout);
    };
  }, []);

  async function listarClientesAPI() {
    try {
      setLoading(true);
      setError(null);
      const resposta = await axios.get<ClienteModel[]>(
        "http://localhost:5209/api/GetCliente"
      );
      const dados = resposta.data;
      setClientes(dados);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição: " + error);
      setError("Erro ao carregar clientes. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  async function deletarCliente(id: number) {
    if (window.confirm("Tem certeza que deseja excluir este cliente?")) {
      try {
        await axios.delete(`http://localhost:5209/api/DeleteCliente=${id}`);
        listarClientesAPI();
      } catch (error) {
        console.log("Erro ao deletar cliente: " + error);
        alert("Erro ao deletar cliente. Tente novamente.");
      }
    }
  }

  function editarCliente(id: number) {
    const cliente = clientes.find((c) => c.id === id);
    if (cliente) {
      setClienteEditando(cliente);
      setIsEditMode(true);
      setShowForm(true);
    }
  }

  function adicionarCliente() {
    setClienteEditando(null);
    setIsEditMode(false);
    setShowForm(true);
  }

  function fecharFormulario() {
    setShowForm(false);
    setClienteEditando(null);
    setIsEditMode(false);
  }

  function handleFormSuccess() {
    listarClientesAPI();
    fecharFormulario();
  }

  if (loading) {
    return (
      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Clientes</h1>
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
        <FormCliente
          cliente={clienteEditando}
          onClose={fecharFormulario}
          onSuccess={handleFormSuccess}
          isEdit={isEditMode}
        />
      )}

      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Clientes</h1>
          <button className="btn-adicionar" onClick={adicionarCliente}>
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
            <th>CPF</th>
            <th>Endereço ID</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {clientes.length === 0 ? (
            <tr>
              <td colSpan={7} className="no-data">
                Nenhum cliente encontrado
              </td>
            </tr>
          ) : (
            clientes.map((cliente) => (
              <tr key={cliente.id}>
                <td>{cliente.id}</td>
                <td>{cliente.nome}</td>
                <td>{cliente.email}</td>
                <td>{cliente.telefone}</td>
                <td>{cliente.cpf}</td>
                <td>{cliente.enderecoId}</td>
                <td className="acoes">
                  <button
                    className="btn-editar"
                    onClick={() => editarCliente(cliente.id)}
                  >
                    Editar
                  </button>
                  <button
                    className="btn-apagar"
                    onClick={() => deletarCliente(cliente.id)}
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

export default Cliente;

