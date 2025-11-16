using System;
using System.Reflection;

namespace Wms.Enums
{
    public static class EnumTipoException
    {
        // Usuario
        public const string MSG0001 = "Nenhum usuário encontrado!";
        public const string MSG0002 = "Usuário não encontrado!";
        public const string MSG0003 = "Esse usuário já existe!";
        public const string MSG0004 = "Esse login já está em uso!";

        // Produto
        public const string MSG0005 = "Nenhum produto encontrado!";
        public const string MSG0006 = "Produto não encontrado!";
        public const string MSG0007 = "Nenhum produto encontrado para essa categoria!";
        public const string MSG0008 = "Nenhum produto encontrado para esse fornecedor!";
        public const string MSG0009 = "Esse produto já existe!";
        public const string MSG0010 = "Nome do produto deve ser informado!";
        public const string MSG0011 = "Preço do produto deve ser maior que zero!";
        public const string MSG0012 = "Lote do produto deve ser maior que zero!";
        public const string MSG0013 = "Fornecedor com ID {0} não encontrado!";

        // Fornecedor
        public const string MSG0014 = "Nenhum fornecedor encontrado!";
        public const string MSG0015 = "Fornecedor não encontrado!";
        public const string MSG0016 = "Esse fornecedor já existe!";
        public const string MSG0017 = "Fornecedor deve ter CNPJ informado!";
        public const string MSG0018 = "Esse CNPJ já está cadastrado!";
        public const string MSG0019 = "Endereço com ID {0} não encontrado!";

        // Cliente
        public const string MSG0020 = "Nenhum cliente encontrado!";
        public const string MSG0021 = "Cliente não encontrado!";
        public const string MSG0022 = "CPF já cadastrado!";

        // Inventario
        public const string MSG0023 = "Nenhum inventário encontrado!";
        public const string MSG0024 = "Inventário não encontrado!";
        public const string MSG0025 = "Nenhum inventário encontrado para esse armazém!";
        public const string MSG0026 = "Nenhum inventário encontrado para esse produto!";
        public const string MSG0027 = "Nenhuma posição vazia encontrada!";
        public const string MSG0028 = "Posição não encontrada!";
        public const string MSG0029 = "Esta posição já está ocupada! Use o endpoint de atualizar quantidade.";
        public const string MSG0030 = "Quantidade deve ser maior que zero!";
        public const string MSG0031 = "Produto com ID {0} não encontrado!";
        public const string MSG0032 = "Posição de inventário com ID {0} não encontrada!";
        public const string MSG0033 = "Armazém com ID {0} não encontrado!";
        public const string MSG0034 = "Quantidade recebida deve ser maior que zero!";
        public const string MSG0035 = "Quantidade retirada deve ser maior que zero!";
        public const string MSG0036 = "A posição {0} não contém o produto ID {1}!";
        public const string MSG0037 = "Quantidade insuficiente! Na posição há apenas {0} unidades.";
        public const string MSG0038 = "Erro ao processar entrada: {0}";
        public const string MSG0039 = "Erro ao processar saída: {0}";

        // Logs
        public const string MSG0040 = "Nenhum log encontrado!";
        public const string MSG0041 = "Log não encontrado!";

        // Extras/Outros
        public const string MSG0042 = "Esta posição está vazia! Use o endpoint de alocar produto.";
        public const string MSG0043 = "Quantidade não pode ser negativa!";
        public const string MSG0044 = "Esta posição já está vazia!";
        public const string MSG0045 = "Nenhuma entrada encontrada!";
        public const string MSG0046 = "Entrada não encontrada!";
        public const string MSG0047 = "Nenhuma saída encontrada!";
        public const string MSG0048 = "Cliente com ID {0} não encontrado!";
        public const string MSG0049 = "Saída não encontrada!";
        public const string MSG0050 = "Esse cliente já existe!";
        public const string MSG0051 = "Cliente deve ter CPF informado!";
        public const string MSG0052 = "Nenhum endereço encontrado!";
        public const string MSG0053 = "Endereço não encontrado!";
        public const string MSG0054 = "Esse endereço já existe!";
        public const string MSG0055 = "Nenhum armazém encontrado!";
        public const string MSG0056 = "Armazém não encontrado!";
        public const string MSG0057 = "Nenhum armazém encontrado com esse status!";
        public const string MSG0058 = "Esse armazém já existe!";
        public const string MSG0059 = "Nome do armazém deve ser informado!";
        public const string MSG0060 = "Número de posições deve ser maior que zero!";
        public const string MSG0061 = "Número de produtos por posição deve ser maior que zero!";
        public const string MSG0062 = "A posição {0} suporta no máximo {1} unidades. Quantidade atual: {2}. Tentando adicionar: {3}. Total seria: {4}. Escolha outra posição ou reduza a quantidade.";
        public const string MSG0063 = "A posição {0} já contém o produto ID {1}! Uma posição só pode ter produtos de um mesmo ID. Escolha outra posição ou remova o produto atual.";

        // Generics
        public const string MSG0099 = "Parâmetros inválidos";

        // Helpers
        public static string Get(string code, params object[] formatArgs)
        {
            var field = typeof(EnumTipoException).GetField(code, BindingFlags.Public | BindingFlags.Static);
            var template = field?.GetRawConstantValue() as string ?? $"Mensagem não cadastrada: {code}";

            if (formatArgs is { Length: > 0 })
            {
                return string.Format(template, formatArgs);
            }
            return template;
        }

        public static Exception ThrowException(string code, params object[] formatArgs)
        {
            var msg = Get(code, formatArgs);
            return new Exception($"#{code} - {msg}");
        }
    }
}

