import React, { useState, useEffect } from "react";
import Produto from "../../Models/Produto";
import Fornecedor from "../../Models/Fornecedor";
import axios from "axios";

interface FormProdutoProps {
  produto?: Produto | null;
  onClose: () => void;
  onSuccess: () => void;
  isEdit: boolean;
}

function FormProduto({ produto, onClose, onSuccess, isEdit }: FormProdutoProps) {
  const [formData, setFormData] = useState({
    nomeProduto: "",
    descricao: "",
    lote: 0,
    fornecedorId: 0,
    preco: 0,
    categoria: ""
  });

  const [fornecedores, setFornecedores] = useState<Fornecedor[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Carregar fornecedores
  useEffect(() => {
    async function loadFornecedores() {
      try {
        const resposta = await axios.get<Fornecedor[]>(
          "http://localhost:5209/api/GetFornecedor"
        );
        setFornecedores(resposta.data);
      } catch (error) {
        console.log("Erro ao carregar fornecedores: " + error);
      }
    }
    loadFornecedores();
  }, []);

  // Edição
  useEffect(() => {
    if (isEdit && produto) {
      setFormData({
        nomeProduto: produto.nomeProduto,
        descricao: produto.descricao,
        lote: produto.lote,
        fornecedorId: produto.fornecedorId,
        preco: produto.preco,
        categoria: produto.categoria
      });
    }
  }, [produto, isEdit]);

  // Atualizar campos
  const handleChange = (e: React.ChangeEvent<any>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]:
        name === "lote" || name === "fornecedorId" || name === "preco"
          ? Number(value)
          : value,
    }));
  };

  // Enviar formulário
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      if (isEdit && produto) {
        await axios.put(
          `http://localhost:5209/api/PutProduto=${produto.id}`,
          {
            id: produto.id,
            ...formData,
          }
        );
      } else {
        await axios.post("http://localhost:5209/api/PostProduto", {
          id: 0,
          ...formData,
        });
      }

      onSuccess();
      onClose();

    } catch (error: any) {
      setError(error.response?.data?.message || "Erro ao salvar produto.");
      console.log("Erro ao salvar produto: " + error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        {/* HEADER */}
        <div className="modal-header">
          <h2>{isEdit ? "Editar Produto" : "Adicionar Produto"}</h2>
          <button className="btn-close" onClick={onClose}>×</button>
        </div>

        {error && <div className="error-message">{error}</div>}

        {/* FORM */}
        <form onSubmit={handleSubmit} className="form-endereco">

          <div className="form-group">
            <label>Nome do Produto *</label>
            <input
              type="text"
              name="nomeProduto"
              value={formData.nomeProduto}
              onChange={handleChange}
              required
              placeholder="Digite o nome"
            />
          </div>

          <div className="form-group">
            <label>Descrição *</label>
            <input
              type="text"
              name="descricao"
              value={formData.descricao}
              onChange={handleChange}
              required
              placeholder="Digite a descrição"
            />
          </div>

          <div className="form-row">
            <div className="form-group">
              <label>Lote *</label>
              <input
                type="number"
                name="lote"
                value={formData.lote}
                onChange={handleChange}
                required
                placeholder="Digite o lote"
              />
            </div>

            <div className="form-group">
              <label>Preço *</label>
              <input
                type="number"
                name="preco"
                value={formData.preco}
                onChange={handleChange}
                required
                step="0.01"
                placeholder="0.00"
              />
            </div>
          </div>

          <div className="form-row">

            <div className="form-group">
              <label>Categoria *</label>
              <input
                type="text"
                name="categoria"
                value={formData.categoria}
                onChange={handleChange}
                required
                placeholder="Ex: Eletrônico"
              />
            </div>

            <div className="form-group">
              <label>Fornecedor *</label>
              <select
                name="fornecedorId"
                value={formData.fornecedorId}
                onChange={handleChange}
                required
              >
                <option value="0">Selecione um fornecedor</option>
                {fornecedores.map((f) => (
                  <option key={f.id} value={f.id}>
                    {f.nome}
                  </option>
                ))}
              </select>
            </div>

          </div>

          {/* BOTÕES */}
          <div className="form-actions">
            <button type="button" className="btn-cancelar" onClick={onClose}>
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

export default FormProduto;
