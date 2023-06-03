using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjetoV.Models;
using Newtonsoft.Json;

namespace ProjetoV.Controllers
{
    public class PessoasController : Controller
    {
        private string diretorio = "C:\\dev\\ProjetoV\\projetov-e92f8-85e16f327b43.json";
        private string projetoId;
        private FirestoreDb _firestoreDb;

        public PessoasController()
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", diretorio);
            projetoId = "projetov-e92f8";
            _firestoreDb = FirestoreDb.Create(projetoId);
        }

        public async Task<IActionResult> Index()
        {
            Query pessoasQuery = _firestoreDb.Collection("pessoas");
            QuerySnapshot pessoasQuerySnapshot = await pessoasQuery.GetSnapshotAsync();
            List<Pessoa> listaPessoas = new List<Pessoa>();
                
            foreach(DocumentSnapshot documentSnapshot in pessoasQuerySnapshot.Documents)
            {
                if (documentSnapshot.Exists)
                {
                    Dictionary<string, object> pessoa = documentSnapshot.ToDictionary();

                    string json = JsonConvert.SerializeObject(pessoa);
                    Pessoa novaPessoa = JsonConvert.DeserializeObject<Pessoa>(json);
                    novaPessoa.PessoaId = documentSnapshot.Id;

                    listaPessoas.Add(novaPessoa);
                }
            }

            return View(listaPessoas);
        }

        [HttpGet]
        public IActionResult NovaPessoa()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NovaPessoa(Pessoa pessoa)
        {
            CollectionReference collectionReference = _firestoreDb.Collection("pessoas");
            await collectionReference.AddAsync(pessoa);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> AtualizarPessoa(string pessoaId)
        {
            DocumentReference documentReference = _firestoreDb.Collection("pessoas").Document(pessoaId);
            DocumentSnapshot documentSnapshot = await documentReference.GetSnapshotAsync();

            if (documentSnapshot.Exists)
            {
                Pessoa pessoa = documentSnapshot.ConvertTo<Pessoa>();
                return View(pessoa);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AtualizarPessoa(Pessoa pessoa)
        {
            DocumentReference documentReferente = _firestoreDb.Collection("pessoas").Document(pessoa.PessoaId);
            await documentReferente.SetAsync(pessoa, SetOptions.Overwrite);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirPessoa(string pessoaId)
        {
            DocumentReference documentReference = _firestoreDb.Collection("pessoas").Document(pessoaId);
            await documentReference.DeleteAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
