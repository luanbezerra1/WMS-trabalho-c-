import React, { useState, useEffect } from "react";
import Armazem from "../../Models/Armazem";
import Endereco from "../../Models/Endereco";
import axios from "axios";

interface FormArmazemProps {
  armazem?: Armazem | null;
  onClose: () => void;
  onSuccess: () => void;
  isEdit: boolean;
}

function FormArmazem({ armazem, onClose, onSuccess, isEdit }: FormArmazemProps) {
  const [formData, setFormData] = useState<Omit<Armazem, "id" | "capacidade">>({
    nomeArmazem: "",
    status: "",
    posicoes: 0,
    produtoPosicao: 0,
    enderecoId: 0,
  });
  const [enderecos, setEnderecos] = useState<Endereco[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
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
    if (isEdit && armazem) {
      setFormData({
        nomeArmazem: armazem.nomeArmazem || "",
        status: armazem.status || "",
        posicoes: armazem.posicoes || 0,
        produtoPosicao: armazem.produtoPosicao || 0,
        enderecoId: armazem.enderecoId || 0,
      });
    }
  }, [armazem, isEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "enderecoId" || name === "posicoes" || name === "produtoPosicao" 
        ? parseInt(value) || 0 
        : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      if (isEdit && armazem) {
        await axios.put(`http://localhost:5209/api/PutArmazem=${armazem.id}`, {
          id: armazem.id,
          nomeArmazem: formData.nomeArmazem,
          status: formData.status,
          posicoes: formData.posicoes,
          produtoPosicao: formData.produtoPosicao,
          enderecoId: formData.enderecoId,
        });
      } else {
        await axios.post("http://localhost:5209/api/PostArmazem", {
          id: 0,
          nomeArmazem: formData.nomeArmazem,
          status: formData.status,
          posicoes: formData.posicoes,
          produtoPosicao: formData.produtoPosicao,
          enderecoId: formData.enderecoId,
        });
      }
      onSuccess();
      onClose();
    } catch (error: any) {
      console.log("Erro ao salvar armazém: " + error);
      setError(error.response?.data?.message || "Erro ao salvar armazém. Tente novamente.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEdit ? "Editar Armazém" : "Adicionar Armazém"}</h2>
          <button className="btn-close" onClick={onClose}>×</button>
        </div>

        {error && (
          <div className="error-message">{error}</div>
        )}

        <form onSubmit={handleSubmit} className="form-endereco">
          <div className="form-group">
            <label htmlFor="nomeArmazem">Nome do Armazém *</label>
            <input
              type="text"
              id="nomeArmazem"
              name="nomeArmazem"
              value={formData.nomeArmazem}
              onChange={handleChange}
              required
              placeholder="Digite o nome do armazém"
            />
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="status">Status *</label>
              <select
                id="status"
                name="status"
                value={formData.status}
                onChange={handleChange}
                required
              >
                <option value="">Selecione o status</option>
                <option value="Ativo">Ativo</option>
                <option value="Inativo">Inativo</option>
                <option value="Manutenção">Manutenção</option>
              </select>
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

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="posicoes">Número de Posições *</label>
              <input
                type="number"
                id="posicoes"
                name="posicoes"
                value={formData.posicoes}
                onChange={handleChange}
                required
                min="1"
                placeholder="Ex: 10"
              />
            </div>

            <div className="form-group">
              <label htmlFor="produtoPosicao">Produtos por Posição *</label>
              <input
                type="number"
                id="produtoPosicao"
                name="produtoPosicao"
                value={formData.produtoPosicao}
                onChange={handleChange}
                required
                min="1"
                placeholder="Ex: 100"
              />
            </div>
          </div>

          {formData.posicoes > 0 && formData.produtoPosicao > 0 && (
            <div className="form-group">
              <p style={{ color: '#1e3a8a', fontWeight: 'bold', marginTop: '10px' }}>
                Capacidade Total: {formData.posicoes * formData.produtoPosicao} produtos
              </p>
            </div>
          )}

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

export default FormArmazem;

