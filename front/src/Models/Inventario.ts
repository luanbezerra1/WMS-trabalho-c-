export default interface Inventario {
    id: number;
    armazemId: number;
    nomePosicao: string;
    produtoId: number | null;
    nomeProduto?: string | null;
    quantidade: number;
    ultimaMovimentacao: string;
}

