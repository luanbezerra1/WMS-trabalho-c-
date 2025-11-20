import React, { useState, useEffect } from "react";
import axios from "axios";
import Cliente from "../../Models/Cliente";
import Produto from "../../Models/Produto";
import Inventario from "../../Models/Inventario";

interface FormSaidaProdutoProps {
  onClose: () => void;
  onSuccess: () => void;
}

function FormSaidaProduto({ onClose, onSuccess }: FormSaidaProdutoProps) {
  const [formData, setFormData] = useState({
    clienteId: 0,
    produtoId: 0,
    quantidadeRetirada: 0,
    inventarioId: 0,
  });
  const [clientes, setClientes] = useState<Cliente[]>([]);
  const [produtos, setProdutos] = useState<Produto[]>([]);
  const [inventarios, setInventarios] = useState<Inventario[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadData() {
      try {
        const [clientesRes, produtosRes] = await Promise.all([
          axios.get<Cliente[]>("http://localhost:5209/api/GetCliente"),
          axios.get<Produto[]>("http://localhost:5209/api/GetProduto"),
        ]);
        setClientes(clientesRes.data);
        setProdutos(produtosRes.data);
      } catch (error) {
        console.log("Erro ao carregar dados: " + error);
      }
    }
    loadData();
  }, []);

  useEffect(() => {
    if (formData.produtoId > 0) {
      const loadInventarios = async () => {
        try {
          const resposta = await axios.get<Inventario[]>(
            `http://localhost:5209/api/GetInventarioByProduto=${formData.produtoId}`
          );
          setInventarios(resposta.data.filter(i => i.produtoId === formData.produtoId && i.quantidade > 0));
        } catch (error) {
          console.log("Erro ao carregar inventários: " + error);
          setInventarios([]);
        }
      }
      loadInventarios();
    } else {
      setInventarios([]);
    }
  }, [formData.produtoId]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "clienteId" || name === "produtoId" || name === "quantidadeRetirada" || name === "inventarioId"
        ? parseInt(value) || 0
        : value,
    }));
  };

  const inventarioSelecionado = inventarios.find(i => i.id === formData.inventarioId);
  const quantidadeMaxima = inventarioSelecionado?.quantidade || 0;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      await axios.post("http://localhost:5209/api/PostSaidaProduto", {
        clienteId: formData.clienteId,
        produtoId: formData.produtoId,
        quantidadeRetirada: formData.quantidadeRetirada,
        inventarioId: formData.inventarioId,
      });
      onSuccess();
      onClose();
    } catch (error: any) {
      console.log("Erro ao registrar saída: " + error);
      setError(error.response?.data?.message || "Erro ao registrar saída. Tente novamente.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>Registrar Saída de Produto</h2>
          <button className="btn-close" onClick={onClose}>×</button>
        </div>

        {error && (
          <div className="error-message">{error}</div>
        )}

        <form onSubmit={handleSubmit} className="form-endereco">
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="clienteId">Cliente *</label>
              <select
                id="clienteId"
                name="clienteId"
                value={formData.clienteId}
                onChange={handleChange}
                required
              >
                <option value="0">Selecione um cliente</option>
                {clientes.map((cliente) => (
                  <option key={cliente.id} value={cliente.id}>
                    {cliente.nome} - {cliente.cpf}
                  </option>
                ))}
              </select>
            </div>

            <div className="form-group">
              <label htmlFor="produtoId">Produto *</label>
              <select
                id="produtoId"
                name="produtoId"
                value={formData.produtoId}
                onChange={handleChange}
                required
              >
                <option value="0">Selecione um produto</option>
                {produtos.map((produto) => (
                  <option key={produto.id} value={produto.id}>
                    {produto.nomeProduto}
                  </option>
                ))}
              </select>
            </div>
          </div>

          {formData.produtoId > 0 && inventarios.length > 0 && (
            <div className="form-group">
              <label htmlFor="inventarioId">Posição (Inventário) *</label>
              <select
                id="inventarioId"
                name="inventarioId"
                value={formData.inventarioId}
                onChange={handleChange}
                required
              >
                <option value="0">Selecione uma posição</option>
                {inventarios.map((inventario) => (
                  <option key={inventario.id} value={inventario.id}>
                    {inventario.nomePosicao} - Quantidade disponível: {inventario.quantidade}
                  </option>
                ))}
              </select>
            </div>
          )}

          {formData.produtoId > 0 && inventarios.length === 0 && (
            <div className="error-message">
              Nenhuma posição disponível com este produto em estoque.
            </div>
          )}

          {inventarioSelecionado && (
            <div className="form-group">
              <label htmlFor="quantidadeRetirada">Quantidade a Retirar *</label>
              <input
                type="number"
                id="quantidadeRetirada"
                name="quantidadeRetirada"
                value={formData.quantidadeRetirada}
                onChange={handleChange}
                required
                min="1"
                max={quantidadeMaxima}
                placeholder={`Máximo: ${quantidadeMaxima}`}
              />
              <p style={{ color: '#6b7280', fontSize: '0.875rem', marginTop: '4px' }}>
                Quantidade disponível na posição: {quantidadeMaxima}
              </p>
            </div>
          )}

          <div className="form-actions">
            <button type="button" className="btn-cancelar" onClick={onClose} disabled={loading}>
              Cancelar
            </button>
            <button type="submit" className="btn-salvar" disabled={loading || inventarios.length === 0}>
              {loading ? "Registrando..." : "Registrar Saída"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default FormSaidaProduto;

