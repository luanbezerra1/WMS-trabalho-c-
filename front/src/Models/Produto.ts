export default interface Produto {
    id: number;
    nomeProduto: string;
    descricao: string;
    lote: number;
    fornecedorId: number;
    preco: number;
    categoria: string;
    criadoEm: string;
}