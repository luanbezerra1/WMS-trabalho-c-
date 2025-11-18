import React, { useEffect, useState } from "react";
import Produto from "../Models/Produto";
import axios from "axios";
import "../Styles/Produto.css";
import FormProduto from "./Components/FormProduto";

function Produtos() {
  const [produtos, setProdutos] = useState<Produto[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [produtoEditando, setProdutoEditando] = useState<Produto | null>(null);
  const [isEditMode, setIsEditMode] = useState(false);

  useEffect(() => {
    let isMounted = true;

    async function loadData() {
      try {
        setLoading(true);
        setError(null);
        const resposta = await axios.get<Produto[]>("http://localhost:5209/api/GetProduto");
        if (isMounted) {
          setProdutos(resposta.data);
          setLoading(false);
        }
      } catch (err: any) {
        if (isMounted) {
          setError("Erro ao carregar produtos. Verifique se o servidor está rodando.");
          setLoading(false);
        }
      }
    }

    loadData();

    const timeout = setTimeout(() => {
      if (isMounted) {
        setLoading(false);
        setError("Timeout ao carregar produtos. Verifique o servidor.");
      }
    }, 10000);

    return () => {
      isMounted = false;
      clearTimeout(timeout);
    };
  }, []);

  async function listarProdutos() {
    try {
      setLoading(true);
      const resposta = await axios.get<Produto[]>("http://localhost:5209/api/GetProduto");
      setProdutos(resposta.data);
      setLoading(false);
    } catch (err) {
      setError("Erro ao recarregar produtos.");
      setLoading(false);
    }
  }

  async function deletarProduto(id: number) {
    if (!window.confirm("Deseja realmente excluir este produto?")) return;
    try {
      // ajuste a rota caso seu backend espere DeleteProduto?id=123 ou DeleteProduto/123
      await axios.delete(`http://localhost:5209/api/DeleteProduto=${id}`);
      listarProdutos();
    } catch (err) {
      console.error(err);
      alert("Erro ao excluir produto.");
    }
  }

  function editarProduto(id: number) {
    const p = produtos.find((x) => x.id === id);
    if (p) {
      setProdutoEditando(p);
      setIsEditMode(true);
      setShowForm(true);
    }
  }

  function adicionarProduto() {
    setProdutoEditando(null);
    setIsEditMode(false);
    setShowForm(true);
  }

  function fecharFormulario() {
    setShowForm(false);
    setProdutoEditando(null);
    setIsEditMode(false);
  }

  function handleFormSuccess() {
    listarProdutos();
    fecharFormulario();
  }

  if (loading) {
    return (
      <div id="componente_listar_produtos">
        <div className="header-container">
          <h1>Listar Produtos</h1>
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
        <FormProduto
          produto={produtoEditando}
          onClose={fecharFormulario}
          onSuccess={handleFormSuccess}
          isEdit={isEditMode}
        />
      )}

      <div id="componente_listar_produtos">
        <div className="header-container">
          <h1>Listar Produtos</h1>
          <button className="btn-adicionar" onClick={adicionarProduto}>
            + Adicionar
          </button>
        </div>

        {error && <div className="error-container">{error}</div>}

        <table>
          <thead>
            <tr>
              <th>#</th>
              <th>Nome</th>
              <th>Descrição</th>
              <th>Lote</th>
              <th>FornecedorId</th>
              <th>Preço</th>
              <th>Categoria</th>
              <th>Criado Em</th>
              <th>Ações</th>
            </tr>
          </thead>

          <tbody>
            {produtos.length === 0 ? (
              <tr>
                <td colSpan={9} className="no-data">
                  Nenhum produto encontrado
                </td>
              </tr>
            ) : (
              produtos.map((produto) => (
                <tr key={produto.id}>
                  <td>{produto.id}</td>
                  <td>{produto.nomeProduto}</td>
                  <td>{produto.descricao}</td>
                  <td>{produto.lote}</td>
                  <td>{produto.fornecedorId}</td>
                  <td>R$ {produto.preco.toFixed(2)}</td>
                  <td>{produto.categoria}</td>
                  <td>{produto.criadoEm}</td>
                  <td className="acoes">
                    <button className="btn-editar" onClick={() => editarProduto(produto.id)}>
                      Editar
                    </button>
                    <button className="btn-apagar" onClick={() => deletarProduto(produto.id)}>
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

export default Produtos;
