import React, { useState, useEffect } from "react";
import axios from "axios";
import Fornecedor from "../../Models/Fornecedor";
import Produto from "../../Models/Produto";
import Inventario from "../../Models/Inventario";

interface FormEntradaProdutoProps {
  onClose: () => void;
  onSuccess: () => void;
}

function FormEntradaProduto({ onClose, onSuccess }: FormEntradaProdutoProps) {
  const [formData, setFormData] = useState({
    fornecedorId: 0,
    produtoId: 0,
    quantidadeRecebida: 0,
    inventarioId: 0,
  });
  const [fornecedores, setFornecedores] = useState<Fornecedor[]>([]);
  const [produtos, setProdutos] = useState<Produto[]>([]);
  const [inventarios, setInventarios] = useState<Inventario[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadData() {
      try {
        const [fornecedoresRes, produtosRes] = await Promise.all([
          axios.get<Fornecedor[]>("http://localhost:5209/api/GetFornecedor"),
          axios.get<Produto[]>("http://localhost:5209/api/GetProduto"),
        ]);
        setFornecedores(fornecedoresRes.data);
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
          // Buscar posições vazias ou com o mesmo produto
          const todasPosicoes = await axios.get<Inventario[]>(
            "http://localhost:5209/api/GetInventario"
          );
          
          // Filtrar posições vazias ou que já têm o mesmo produto
          const posicoesDisponiveis = todasPosicoes.data.filter(
            (i) => i.produtoId === null || i.produtoId === formData.produtoId
          );
          
          setInventarios(posicoesDisponiveis);
        } catch (error) {
          console.log("Erro ao carregar inventários: " + error);
          setInventarios([]);
        }
      };
      loadInventarios();
    } else {
      setInventarios([]);
    }
  }, [formData.produtoId]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === "fornecedorId" || name === "produtoId" || name === "quantidadeRecebida" || name === "inventarioId"
        ? parseInt(value) || 0
        : value,
    }));
  };

  const inventarioSelecionado = inventarios.find(i => i.id === formData.inventarioId);
  const quantidadeAtual = inventarioSelecionado?.quantidade || 0;

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
    } catch (error: any) {
      console.log("Erro ao registrar entrada: " + error);
      setError(error.response?.data?.message || "Erro ao registrar entrada. Tente novamente.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>Registrar Entrada de Produto</h2>
          <button className="btn-close" onClick={onClose}>×</button>
        </div>

        {error && (
          <div className="error-message">{error}</div>
        )}

        <form onSubmit={handleSubmit} className="form-endereco">
          <div className="form-row">
            <div className="form-group">
              <label htmlFor="fornecedorId">Fornecedor *</label>
              <select
                id="fornecedorId"
                name="fornecedorId"
                value={formData.fornecedorId}
                onChange={handleChange}
                required
              >
                <option value="0">Selecione um fornecedor</option>
                {fornecedores.map((fornecedor) => (
                  <option key={fornecedor.id} value={fornecedor.id}>
                    {fornecedor.nome} - {fornecedor.cnpj}
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
                    {inventario.nomePosicao} - {inventario.produtoId ? `Produto atual: ${inventario.nomeProduto || inventario.produtoId}, Qtd: ${inventario.quantidade}` : "Vazio"}
                  </option>
                ))}
              </select>
            </div>
          )}

          {formData.produtoId > 0 && inventarios.length === 0 && (
            <div className="error-message">
              Nenhuma posição disponível. Todas as posições estão ocupadas com outros produtos.
            </div>
          )}

          {inventarioSelecionado && (
            <div className="form-group">
              <label htmlFor="quantidadeRecebida">Quantidade Recebida *</label>
              <input
                type="number"
                id="quantidadeRecebida"
                name="quantidadeRecebida"
                value={formData.quantidadeRecebida}
                onChange={handleChange}
                required
                min="1"
                placeholder="Digite a quantidade"
              />
              {inventarioSelecionado.produtoId && (
                <p style={{ color: '#6b7280', fontSize: '0.875rem', marginTop: '4px' }}>
                  Quantidade atual na posição: {quantidadeAtual}
                </p>
              )}
            </div>
          )}

          <div className="form-actions">
            <button type="button" className="btn-cancelar" onClick={onClose} disabled={loading}>
              Cancelar
            </button>
            <button type="submit" className="btn-salvar" disabled={loading || inventarios.length === 0}>
              {loading ? "Registrando..." : "Registrar Entrada"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

export default FormEntradaProduto;
