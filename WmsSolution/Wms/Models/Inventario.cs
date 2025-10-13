namespace Wms.Models

{
    public class Inventario
{
    public int ArmazemId { get; set; }
    public Armazem? Armazem { get; set; }

    public int ProdutoId { get; set; }
    public Produto? Produto { get; set; }

    public string PosicaoCodigo { get; set; } = "";   // ex: "A1-01-02"
    public int Quantidade { get; set; }

    }
}