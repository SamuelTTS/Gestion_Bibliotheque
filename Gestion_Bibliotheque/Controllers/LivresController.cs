using Gestion_Bibliotheque.Models;
using Microsoft.AspNetCore.Mvc; 

namespace Gestion_Bibliotheque.Controllers
{
    public class LivresController : Controller
    {
        private readonly ApplicationDbContext context;

        public LivresController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index(string searchString)
        {
            var livres = from l in context.Livres
                         select l;

            // 2. Si l'utilisateur a écrit quelque chose dans la barre de recherche
            if (!string.IsNullOrEmpty(searchString))
            {
                // On filtre par Titre ou par Auteur (insensible à la casse)
                livres = livres.Where(s => s.Titre.Contains(searchString) || s.Auteur.Contains(searchString));
            }
            return View("alllivres", livres.ToList());
        }
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View("addlivres");
        }

        [HttpPost]
        public IActionResult Create(Livres livre)
        {
            if (ModelState.IsValid)
            {
                context.Livres.Add(livre);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("addlivres", livre);
        }
        
        // 1. Affiche le formulaire avec les données actuelles
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var livre = context.Livres.Find(id);
            if (livre == null) return NotFound();

            return View("editelivre", livre); // Assure-toi d'avoir une vue editlivres.cshtml
        }

        // 2. Traite la modification après le clic sur "Enregistrer"
        [HttpPost]
        public IActionResult Edit(Livres livre)
        {
            if (ModelState.IsValid)
            {
                context.Livres.Update(livre); // Met à jour l'objet dans le contexte
                context.SaveChanges();        // Envoie l'UPDATE à MySQL
                return RedirectToAction("Index");
            }
            return View("editlivres", livre);
        }

        public IActionResult Delete(int id)
        {
            var livre = context.Livres.Find(id);
            if (livre != null)
            {
                context.Livres.Remove(livre); // Prépare la suppression
                context.SaveChanges();        // Envoie le DELETE à MySQL
            }
            return RedirectToAction("Index");
        }
        // Action pour basculer l'état du livre (Disponible <-> Emprunté)
        public IActionResult ToggleEmprunt(int id)
        {
            // 1. Rechercher le livre dans la base MySQL (via Docker)
            var livre = context.Livres.Find(id);

            if (livre == null)
            {
                return NotFound();
            }

            // 2. Logique de bascule (Toggle)
            // Si EstEmprunte est vrai, il devient faux. Si faux, il devient vrai.
            livre.EstEmprunte = !livre.EstEmprunte;

            // 3. Mise à jour et sauvegarde
            context.Livres.Update(livre);
            context.SaveChanges();

            // 4. Redirection vers la liste pour voir le changement de couleur du badge
            return RedirectToAction("Index");
        }
        public IActionResult Emprunter(int id)
        {
            // 1. Récupération de l'utilisateur en session
            string? nomConnecte = HttpContext.Session.GetString("UserName");

            if (string.IsNullOrEmpty(nomConnecte))
            {
                return RedirectToAction("Login", "Account");
            }

            var livre = context.Livres.Find(id);

            if (livre != null)
            {
                // CAS A : Le livre est libre -> On l'emprunte
                if (!livre.EstEmprunte)
                {
                    livre.EstEmprunte = true;
                    livre.Emprunteur = nomConnecte;
                }
                // CAS B : Le livre est déjà à moi -> Je le rends
                else if (livre.Emprunteur == nomConnecte)
                {
                    livre.EstEmprunte = false;
                    livre.Emprunteur = null; // On efface le nom pour libérer le livre
                }

                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
