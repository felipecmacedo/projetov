using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoV.Models;

namespace ProjetoV.Controllers
{
    public class ProdutosController : Controller
    {
        private string diretorio = "C:\\dev\\ProjetoV\\projetov-e92f8-85e16f327b43.json";
        private string projetoId;
        private FirestoreDb _firestoreDb;

        public ProdutosController()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATON_CREDENTIALS", diretorio);
            projetoId = "projetov-e92f8";
            _firestoreDb = FirestoreDb.Create(projetoId);
        }

        public async Task<IActionResult> Index()
        {
            Query produtosQuery = _firestoreDb.Collection("produtos");
            QuerySnapshot produtosQuerySnapshot = await produtosQuery.GetSnapshotAsync();
            List<Produtos> listaProdutos = new List<Produtos>();

            foreach(DocumentSnapshot documentSnapshot in produtosQuerySnapshot.Documents)
            {
                if(documentSnapshot.Exists)
                {
                    Dictionary<string, object> produto = documentSnapshot.ToDictionary();

                    string json = JsonConvert.SerializeObject(produto);
                    Produtos novoProduto = JsonConvert.DeserializeObject<Produtos>(json);
                    novoProduto.ProdutoId = documentSnapshot.Id;

                    listaProdutos.Add(novoProduto);
                }
            }

            return View(listaProdutos);
        }
    }
}
