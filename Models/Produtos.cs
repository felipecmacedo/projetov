using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjetoV.Models
{
    [FirestoreData]
    public class Produtos
    {
        [Display(Name = "Código do Produto")]
        public string ProdutoId { get; set; } = "";

        [FirestoreProperty]
        public string Nome { get; set; } = "";

        [Display(Name = "Preço")]
        [FirestoreProperty]
        public decimal Preco { get; set; }

        [Display(Name = "Estoque disponível")]
        [FirestoreProperty]
        public int EstoqueDisponivel { get; set; }
    }
}
