import React, { useState, useEffect } from "react";
import Endereco from "../../Models/Endereco";
import axios from "axios";

interface FormEnderecoProps {
  endereco?: Endereco | null;
  onClose: () => void;
  onSuccess: () => void;
  isEdit: boolean;
}

function FormEndereco({ endereco, onClose, onSuccess, isEdit }: FormEnderecoProps) {
  const [formData, setFormData] = useState<Omit<Endereco, "id">>({
    rua: "",
    numero: "",
    complemento: "",
    bairro: "",
    cidade: "",
    estado: "",
    cep: "",
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (isEdit && endereco) {
      setFormData({
        rua: endereco.rua || "",
        numero: endereco.numero || "",
        complemento: endereco.complemento || "",
        bairro: endereco.bairro || "",
        cidade: endereco.cidade || "",
        estado: endereco.estado || "",
        cep: endereco.cep || "",
      });
    }
  }, [endereco, isEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
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
      if (isEdit && endereco) {
        // PUT - Atualizar
        await axios.put(`http://localhost:5209/api/PutEndereco=${endereco.id}`, {
          id: endereco.id,
          rua: formData.rua,
          numero: formData.numero,
          complemento: formData.complemento,
          bairro: formData.bairro,
          cidade: formData.cidade,
          estado: formData.estado,
          cep: formData.cep,
        });
      } else {
        // POST - Criar novo
        await axios.post("http://localhost:5209/api/PostEndereco", {
          id: 0,
          rua: formData.rua,
          numero: formData.numero,
          complemento: formData.complemento,
          bairro: formData.bairro,
          cidade: formData.cidade,
          estado: formData.estado,
          cep: formData.cep,
        });
      }
      onSuccess();
      onClose();
    } catch (error: any) {
      console.log("Erro ao salvar endereço: " + error);
      setError(error.response?.data?.message || "Erro ao salvar endereço. Tente novamente.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>{isEdit ? "Editar Endereço" : "Adicionar Endereço"}</h2>
          <button className="btn-close" onClick={onClose}>×</button>
        </div>

        {error && (
          <div className="error-message">{error}</div>
        )}

        <form onSubmit={handleSubmit} className="form-endereco">
          <div className="form-group">
            <label htmlFor="rua">Rua *</label>
            <input
              type="text"
              id="rua"
              name="rua"
              value={formData.rua}
              onChange={handleChange}
              required
              placeholder="Digite a rua"
            />
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="numero">Número *</label>
              <input
                type="text"
                id="numero"
                name="numero"
                value={formData.numero}
                onChange={handleChange}
                required
                placeholder="Digite o número"
              />
            </div>

            <div className="form-group">
              <label htmlFor="cep">CEP *</label>
              <input
                type="text"
                id="cep"
                name="cep"
                value={formData.cep}
                onChange={handleChange}
                required
                placeholder="00000-000"
              />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="complemento">Complemento</label>
            <input
              type="text"
              id="complemento"
              name="complemento"
              value={formData.complemento}
              onChange={handleChange}
              placeholder="Digite o complemento (opcional)"
            />
          </div>

          <div className="form-group">
            <label htmlFor="bairro">Bairro *</label>
            <input
              type="text"
              id="bairro"
              name="bairro"
              value={formData.bairro}
              onChange={handleChange}
              required
              placeholder="Digite o bairro"
            />
          </div>

          <div className="form-row">
            <div className="form-group">
              <label htmlFor="cidade">Cidade *</label>
              <input
                type="text"
                id="cidade"
                name="cidade"
                value={formData.cidade}
                onChange={handleChange}
                required
                placeholder="Digite a cidade"
              />
            </div>

            <div className="form-group">
              <label htmlFor="estado">Estado *</label>
              <input
                type="text"
                id="estado"
                name="estado"
                value={formData.estado}
                onChange={handleChange}
                required
                placeholder="Ex: SP"
                maxLength={2}
              />
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

export default FormEndereco;

