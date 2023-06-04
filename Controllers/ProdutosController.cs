using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", diretorio);
            projetoId = "projetov-e92f8";
            _firestoreDb = FirestoreDb.Create(projetoId);
        }
 
        public async Task<IActionResult> Index()
        {
            Query produtosQuery = _firestoreDb.Collection("produtos");
            QuerySnapshot produtosQuerySnapshot = await produtosQuery.GetSnapshotAsync();
            List<Produto> listaProdutos = new List<Produto>();

            foreach(DocumentSnapshot documentSnapshot in produtosQuerySnapshot.Documents)
            {
                if(documentSnapshot.Exists) 
                {
                    Dictionary<string, object> produto = documentSnapshot.ToDictionary();

                    string json = JsonConvert.SerializeObject(produto);
                    Produto novoProduto = JsonConvert.DeserializeObject<Produto>(json);
                    novoProduto.ProdutoId = documentSnapshot.Id;

                    listaProdutos.Add(novoProduto);
                }
            }
            return View(listaProdutos);
        }

        [HttpGet]
        public IActionResult NovoProduto()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NovoProduto(Produto produto)
        {
            CollectionReference collectionReference = _firestoreDb.Collection("produtos");
            await collectionReference.AddAsync(produto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AtualizarProduto(string produtoId)
        {
            DocumentReference documentReference = _firestoreDb.Collection("produtos").Document(produtoId);
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                Produto produto = documentSnapshot.ConvertTo<Produto>();
                return View(produto);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarProduto(Produto produto)
        {
            DocumentReference documentReferente = _firestoreDb.Collection("produtos").Document(produto.ProdutoId);
            await documentReferente.SetAsync(produto, SetOptions.Overwrite);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirProduto(string produtoId)
        {
            DocumentReference documentReference = _firestoreDb.Collection("produtos").Document(produtoId);
            await documentReference.DeleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}