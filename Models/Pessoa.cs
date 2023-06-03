using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;

namespace ProjetoV.Models
{
    [FirestoreData]
    public class Pessoa
    {
        [Display(Name = "Código Pessoa")]
        public string PessoaId { get; set; } = "";

        [FirestoreProperty]
        public string Nome { get; set; } = "";

        [Display(Name = "CPF")]
        [FirestoreProperty]
        public string Cpf { get; set; } = "";

        [Display(Name = "E-Mail")]
        [FirestoreProperty]
        public string Email { get; set; } = "";

    }
}
