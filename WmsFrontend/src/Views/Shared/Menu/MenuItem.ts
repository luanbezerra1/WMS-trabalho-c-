export type MenuItem = {
  label: string;
  path: string;
};

export const defaultMenu: MenuItem[] = [
  { label: "Ordens", path: "/ordens" },
  { label: "Entradas", path: "/entradas" },
  { label: "Saídas", path: "/saidas" },
  { label: "Produtos", path: "/produtos" },
  { label: "Clientes", path: "/clientes" },
  { label: "Fornecedores", path: "/fornecedores" },
  { label: "Inventário", path: "/inventario" },
  { label: "Endereços", path: "/enderecos" },
  { label: "Armazéns", path: "/armazens" },
  { label: "Logs", path: "/logs" }
];

