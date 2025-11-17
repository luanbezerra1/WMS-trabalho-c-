import React, { useState, useEffect } from "react";
import Fornecedor from "../../Models/Fornecedor";
import Endereco from "../../Models/Endereco";
import axios from "axios";

interface FormFornecedorProps {
  fornecedor?: Fornecedor | null;
  onClose: () => void;
  onSuccess: () => void;
  isEdit: boolean;
}

function FormFornecedor({ fornecedor, onClose, onSuccess, isEdit }: FormFornecedorProps) {
  const [formData, setFormData] = useState<Omit<Fornecedor, "id">>({
    nome: "",
    email: "",
    telefone: "",
    cnpj: "",
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
    if (isEdit && fornecedor) {
      setFormData({
        nome: fornecedor.nome || "",
        email: fornecedor.email || "",
        telefone: fornecedor.telefone || "",
        cnpj: fornecedor.cnpj || "",
        enderecoId: fornecedor.enderecoId || 0,
      });
    }
  }, [fornecedor, isEdit]);

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
      if (isEdit && fornecedor) {
        // PUT - Atualizar
        await axios.put(`http://localhost:5209/api/PutFornecedor=${fornecedor.id}`, {
          id: fornecedor.id,
          nome: formData.nome,
          email: formData.email,
          telefone: formData.telefone,
          cnpj: formData.cnpj,
          enderecoId: formData.enderecoId,
        });
      } else {
        // POST - Criar novo
        await axios.post("http://localhost:5209/api/PostFornecedor", {
          id: 0,
          nome: formData.nome,
          email: formData.email,
          telefone: formData.telefone,
          cnpj: formData.cnpj,
          enderecoId: formData.enderecoId,
        });
      }
      onSuccess();
      onClose();
    } catch (error: any) {
      console.log("Erro ao salvar fornecedor: " + error);
      setError(error.response?.data?.message || "Erro ao salvar fornecedor. Tente novamente.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEdit ? "Editar Fornecedor" : "Adicionar Fornecedor"}</h2>
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
              <label htmlFor="cnpj">CNPJ *</label>
              <input
                type="text"
                id="cnpj"
                name="cnpj"
                value={formData.cnpj}
                onChange={handleChange}
                required
                placeholder="00.000.000/0000-00"
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

export default FormFornecedor;

