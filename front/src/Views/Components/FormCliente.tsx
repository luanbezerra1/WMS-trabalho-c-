import React, { useState, useEffect } from "react";
import Cliente from "../../Models/Cliente";
import Endereco from "../../Models/Endereco";
import axios from "axios";

interface FormClienteProps {
  cliente?: Cliente | null;
  onClose: () => void;
  onSuccess: () => void;
  isEdit: boolean;
}

function FormCliente({ cliente, onClose, onSuccess, isEdit }: FormClienteProps) {
  const [formData, setFormData] = useState<Omit<Cliente, "id">>({
    nome: "",
    email: "",
    telefone: "",
    cpf: "",
    enderecoId: 0,
  });
  const [enderecos, setEnderecos] = useState<Endereco[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    // Carregar lista de endereços
    async function loadEnderecos() {
      try {
        const resposta = await axios.get<Endereco[]>(
          "http://localhost:5209/api/GetEndereco"
        );
        setEnderecos(resposta.data);
      } catch (error) {
        console.log("Erro ao carregar endereços: " + error);
      }
    }
    loadEnderecos();
  }, []);

  useEffect(() => {
    if (isEdit && cliente) {
      setFormData({
        nome: cliente.nome || "",
        email: cliente.email || "",
        telefone: cliente.telefone || "",
        cpf: cliente.cpf || "",
        enderecoId: cliente.enderecoId || 0,
      });
    }
  }, [cliente, isEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "enderecoId" ? parseInt(value) || 0 : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      if (isEdit && cliente) {
        // PUT - Atualizar
        await axios.put(`http://localhost:5209/api/PutCliente=${cliente.id}`, {
          id: cliente.id,
          nome: formData.nome,
          email: formData.email,
          telefone: formData.telefone,
          cpf: formData.cpf,
          enderecoId: formData.enderecoId,
        });
      } else {
        // POST - Criar novo
        await axios.post("http://localhost:5209/api/PostCliente", {
          id: 0,
          nome: formData.nome,
          email: formData.email,
          telefone: formData.telefone,
          cpf: formData.cpf,
          enderecoId: formData.enderecoId,
        });
      }
      onSuccess();
      onClose();
    } catch (error: any) {
      console.log("Erro ao salvar cliente: " + error);
      setError(error.response?.data?.message || "Erro ao salvar cliente. Tente novamente.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEdit ? "Editar Cliente" : "Adicionar Cliente"}</h2>
          <button className="btn-close" onClick={onClose}>×</button>
        </div>

        {error && (
          <div className="error-message">{error}</div>
        )}

        <form onSubmit={handleSubmit} className="form-endereco">
          <div className="form-group">
            <label htmlFor="nome">Nome *</label>
            <input
              type="text"
              id="nome"
              name="nome"
              value={formData.nome}
              onChange={handleChange}
              required
              placeholder="Digite o nome"
            />
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="email">Email *</label>
              <input
                type="email"
                id="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                required
                placeholder="Digite o email"
              />
            </div>

            <div className="form-group">
              <label htmlFor="telefone">Telefone *</label>
              <input
                type="text"
                id="telefone"
                name="telefone"
                value={formData.telefone}
                onChange={handleChange}
                required
                placeholder="(00) 00000-0000"
              />
            </div>
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="cpf">CPF *</label>
              <input
                type="text"
                id="cpf"
                name="cpf"
                value={formData.cpf}
                onChange={handleChange}
                required
                placeholder="000.000.000-00"
              />
            </div>

            <div className="form-group">
              <label htmlFor="enderecoId">Endereço *</label>
              <select
                id="enderecoId"
                name="enderecoId"
                value={formData.enderecoId}
                onChange={handleChange}
                required
              >
                <option value="0">Selecione um endereço</option>
                {enderecos.map((endereco) => (
                  <option key={endereco.id} value={endereco.id}>
                    {endereco.rua}, {endereco.numero} - {endereco.cidade}/{endereco.estado}
                  </option>
                ))}
              </select>
            </div>
          </div>

          <div className="form-actions">
            <button type="button" className="btn-cancelar" onClick={onClose} disabled={loading}>
              Cancelar
            </button>
            <button type="submit" className="btn-salvar" disabled={loading}>
              {loading ? "Salvando..." : isEdit ? "Atualizar" : "Salvar"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default FormCliente;

