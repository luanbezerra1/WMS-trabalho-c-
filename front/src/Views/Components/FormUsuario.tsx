import React, { useState, useEffect } from "react";
import Usuario from "../../Models/Usuario";
import axios from "axios";
import "../../Styles/Main.css";

interface FormUsuarioProps {
  usuario?: Usuario | null;
  onClose: () => void;
  onSuccess: () => void;
  isEdit: boolean;
}

function FormUsuario({ usuario, onClose, onSuccess, isEdit }: FormUsuarioProps) {
  const [formData, setFormData] = useState<Omit<Usuario, "id">>({
    nome: "",
    login: "",
    senha: "",
    cargo: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (isEdit && usuario) {
      setFormData({
        nome: usuario.nome || "",
        login: usuario.login || "",
        senha: usuario.senha || "",
        cargo: usuario.cargo || "",
      });
    }
  }, [usuario, isEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      if (isEdit && usuario) {
        await axios.put(`http://localhost:5209/api/PutUsuario=${usuario.id}`, {
          id: usuario.id,
          nome: formData.nome,
          login: formData.login,
          senha: formData.senha,
          cargo: formData.cargo,
        });
      } else {
        await axios.post("http://localhost:5209/api/PostUsuario", {
          id: 0,
          nome: formData.nome,
          login: formData.login,
          senha: formData.senha,
          cargo: formData.cargo,
        });
      }
      onSuccess();
      onClose();
    } catch (error: any) {
      console.error("Erro ao salvar usuário:", error);
      setError(error.response?.data?.message || "Erro ao salvar usuário.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEdit ? "Editar Usuário" : "Adicionar Usuário"}</h2>
          <button className="btn-close" onClick={onClose}>×</button>
        </div>
        {error && <div className="error-message">{error}</div>}
        <form onSubmit={handleSubmit} className="form-endereco">
          <div className="form-group">
            <label htmlFor="nome">Nome:</label>
            <input
              type="text"
              id="nome"
              name="nome"
              value={formData.nome}
              onChange={handleChange}
              required
            />
          </div>
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="login">Login:</label>
              <input
                type="text"
                id="login"
                name="login"
                value={formData.login}
                onChange={handleChange}
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="senha">Senha:</label>
              <input
                type="password"
                id="senha"
                name="senha"
                value={formData.senha}
                onChange={handleChange}
                required
              />
            </div>
          </div>
          <div className="form-group">
            <label htmlFor="cargo">Cargo:</label>
            <select
              id="cargo"
              name="cargo"
              value={formData.cargo}
              onChange={handleChange}
              required
            >
              <option value="">Selecione o cargo</option>
              <option value="Administrador">Administrador</option>
              <option value="Operador">Operador</option>
              <option value="Supervisor">Supervisor</option>
              <option value="Estoquista">Estoquista</option>
            </select>
          </div>
          <div className="form-actions">
            <button type="button" className="btn-cancelar" onClick={onClose} disabled={loading}>
              Cancelar
            </button>
            <button type="submit" className="btn-salvar" disabled={loading}>
              {loading ? "Salvando..." : "Salvar"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default FormUsuario;

