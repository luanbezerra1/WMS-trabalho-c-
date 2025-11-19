import React, { useEffect, useState } from "react";
import Entrada from "../../Models/Entrada";
import axios from "axios";

interface FormEntradaProps {
  entrada?: Entrada | null;
  onClose: () => void;
  onSuccess: () => void;
  isEdit: boolean;
}

function FormEntrada({ entrada, onClose, onSuccess, isEdit }: FormEntradaProps) {
    const [formData, setFormData] = useState<{
        fornecedorId: number;
        produtoId: number;
        quantidadeRecebida: number;
        inventarioId: number;
      }>({
        fornecedorId: 0,
        produtoId: 0,
        quantidadeRecebida: 0,
        inventarioId: 0,      
      });

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (isEdit && entrada) {
      setFormData({
        fornecedorId: entrada.fornecedorId,
        produtoId: entrada.produtoId,
        quantidadeRecebida: entrada.quantidadeRecebida,
        inventarioId: entrada.inventarioId ?? 0,
      });
    }
  }, [entrada, isEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: Number(value),
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      await axios.post("http://localhost:5209/api/PostEntradaProduto", {
        fornecedorId: formData.fornecedorId,
        produtoId: formData.produtoId,
        quantidadeRecebida: formData.quantidadeRecebida,
        inventarioId: formData.inventarioId,   
      });

      onSuccess(); 
      onClose();
    } catch (err: any) {
      console.log("Erro ao salvar entrada: ", err);
      setError(
        err.response?.data?.message ||
          "Erro ao salvar entrada. Verifique os IDs informados."
      );
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEdit ? "Editar Entrada" : "Adicionar Entrada"}</h2>
          <button className="btn-close" onClick={onClose}>
            ×
          </button>
        </div>

        {error && <div className="error-message">{error}</div>}

        <form onSubmit={handleSubmit} className="form-endereco">
          <div className="form-group">
            <label>Fornecedor ID *</label>
            <input
              type="number"
              name="fornecedorId"
              value={formData.fornecedorId}
              onChange={handleChange}
              required
              placeholder="ID do fornecedor"
            />
          </div>

          <div className="form-group">
            <label>Produto ID *</label>
            <input
              type="number"
              name="produtoId"
              value={formData.produtoId}
              onChange={handleChange}
              required
              placeholder="ID do produto"
            />
          </div>

          <div className="form-group">
            <label>Quantidade Recebida *</label>
            <input
              type="number"
              name="quantidadeRecebida"
              value={formData.quantidadeRecebida}
              onChange={handleChange}
              required
              placeholder="Quantidade"
            />
          </div>

        <div className="form-group">
        <label>Inventário ID *</label>
        <input
        type="number"
        name="inventarioId"
        value={formData.inventarioId}
        onChange={handleChange}
        required
        placeholder="ID da posição (Inventário)"
        />
        </div>

          <div className="form-actions">
            <button
              type="button"
              className="btn-cancelar"
              onClick={onClose}
              disabled={loading}
            >
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

export default FormEntrada;