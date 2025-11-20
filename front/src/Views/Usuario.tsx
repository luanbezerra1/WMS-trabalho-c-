import React, { useEffect, useState } from "react";
import UsuarioModel from "../Models/Usuario";
import axios from "axios";
import "../Styles/Main.css";
import "../Styles/Usuario.css";
import FormUsuario from "./Components/FormUsuario";
import { useRefresh } from "../Contexts/RefreshContext";

function Usuario() {
  const [usuarios, setUsuarios] = useState<UsuarioModel[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [usuarioEditando, setUsuarioEditando] = useState<UsuarioModel | null>(null);
  const [isEditMode, setIsEditMode] = useState(false);
  const { setRefreshFunction } = useRefresh();

  async function listarUsuariosAPI() {
    try {
      setLoading(true);
      setError(null);
      const resposta = await axios.get<UsuarioModel[]>(
        "http://localhost:5209/api/GetUsuario"
      );
      const dados = resposta.data;
      setUsuarios(dados);
      setLoading(false);
    } catch (error: any) {
      console.log("Erro na requisição: " + error);
      setError("Erro ao carregar usuários. Verifique se o servidor está rodando.");
      setLoading(false);
    }
  }

  useEffect(() => {
    listarUsuariosAPI();
    setRefreshFunction(() => listarUsuariosAPI);
    
    return () => {
      setRefreshFunction(null);
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  async function deletarUsuario(id: number) {
    if (window.confirm("Tem certeza que deseja excluir este usuário?")) {
      try {
        await axios.delete(`http://localhost:5209/api/DeleteUsuario=${id}`);
        listarUsuariosAPI();
      } catch (error) {
        console.log("Erro ao deletar usuário: " + error);
        alert("Erro ao deletar usuário. Tente novamente.");
      }
    }
  }

  function editarUsuario(id: number) {
    const usuario = usuarios.find((u) => u.id === id);
    if (usuario) {
      setUsuarioEditando(usuario);
      setIsEditMode(true);
      setShowForm(true);
    }
  }

  function adicionarUsuario() {
    setUsuarioEditando(null);
    setIsEditMode(false);
    setShowForm(true);
  }

  function fecharFormulario() {
    setShowForm(false);
    setUsuarioEditando(null);
    setIsEditMode(false);
  }

  function handleFormSuccess() {
    listarUsuariosAPI();
    fecharFormulario();
  }

  if (loading) {
    return (
      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Usuários</h1>
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
        <FormUsuario
          usuario={usuarioEditando}
          onClose={fecharFormulario}
          onSuccess={handleFormSuccess}
          isEdit={isEditMode}
        />
      )}

      <div id="componente_listar_enderecos">
        <div className="header-container">
          <h1>Listar Usuários</h1>
          <button className="btn-adicionar" onClick={adicionarUsuario}>
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
            <th>Login</th>
            <th>Cargo</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          {usuarios.length === 0 ? (
            <tr>
              <td colSpan={5} className="no-data">
                Nenhum usuário encontrado
              </td>
            </tr>
          ) : (
            usuarios.map((usuario) => (
              <tr key={usuario.id}>
                <td>{usuario.id}</td>
                <td>{usuario.nome}</td>
                <td>{usuario.login}</td>
                <td>{usuario.cargo}</td>
                <td className="acoes">
                  <button
                    className="btn-editar"
                    onClick={() => editarUsuario(usuario.id)}
                  >
                    Editar
                  </button>
                  <button
                    className="btn-apagar"
                    onClick={() => deletarUsuario(usuario.id)}
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

export default Usuario;

