using Google.Cloud.Firestore;
using System.ComponentModel;

namespace ProjetoV.Models
{
    [FirestoreData]
    public class Produto
    {
        [DisplayName("Código do Produto")]
        public string ProdutoId { get; set; }

        [FirestoreProperty]
        public string Nome { get; set; }

        [FirestoreProperty]
        public double Preco { get; set; }

        [FirestoreProperty]
        public int Quantidade { get; set; }
    }
}
