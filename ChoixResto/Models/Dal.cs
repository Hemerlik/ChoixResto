using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChoixResto.Models
{
    public class Dal : IDal
    {
        private BddContext _context;

        public Dal()
        {
            this._context = new BddContext();
        }

        public List<Resto> ObtientTousLesRestaurants()
        {
            return this._context.Restos.ToList();
        }

        public void CreerRestaurant(string name, string telephone)
        {
            this._context.Restos.Add(new Resto { Nom = name, Telephone = telephone });
            this._context.SaveChanges();
        }

        public void ModifierRestaurant(int id, string nom, string telephone)
        {
            Resto restoTrouve = this._context.Restos.FirstOrDefault(r => r.Id == id);
            if(restoTrouve != null)
            {
                restoTrouve.Nom = nom;
                restoTrouve.Telephone = telephone;
                this._context.SaveChanges();
            }
        }

        public bool RestaurantExiste(string nom)
        {
            return this._context.Restos.Any(r => r.Nom == nom);
        }

        public Utilisateur ObtenirUtilisateur(int id)
        {
            return this._context.Utilisateurs.FirstOrDefault(u => u.Id == id);
        }

        public Utilisateur ObtenirUtilisateur(string nom)
        {
            var ret = this._context.Utilisateurs.FirstOrDefault(u => u.Prenom == nom);
            if(ret == null && int.TryParse(nom, out int index))
            {
                ret = this._context.Utilisateurs.FirstOrDefault(u => u.Id == index);
            }

            return ret;
        }

        public int AjouterUtilisateur(string nom, string motDePasse)
        {
            var mdp = this.EncodeMD5(motDePasse);
            var utilisateur = this._context.Utilisateurs.Add(new Utilisateur { Prenom = nom, MotDePasse = mdp });
            this._context.SaveChanges();
            return utilisateur.Id;
        }

        public Utilisateur Authentifier(string nom, string motDePasse)
        {
            var mdp = this.EncodeMD5(motDePasse);
            return this._context.Utilisateurs.FirstOrDefault(u => u.Prenom == nom && u.MotDePasse == mdp);
        }

        public bool ADejaVote(int id, string nom)
        {
            var sondage = this._context.Sondages.FirstOrDefault(s => s.Id == id);
            var utilisateur = this._context.Utilisateurs.FirstOrDefault(u => u.Prenom == nom);
            if(utilisateur == null && int.TryParse(nom, out int index))
            {
                utilisateur = this._context.Utilisateurs.FirstOrDefault(u => u.Id == index);
            }

            return sondage != null && utilisateur != null && sondage.Votes.Any(v => v.Utilisateur == utilisateur);
        }

        public void AjouterVote(int sondageId, int restaurantId, int utilisateurId)
        {
            var sondage = this._context.Sondages.FirstOrDefault(s => s.Id == sondageId);
            var utilisateur = this._context.Utilisateurs.FirstOrDefault(u => u.Id == utilisateurId);
            var restaurant = this._context.Restos.FirstOrDefault(r => r.Id == restaurantId);

            if(sondage != null && utilisateur != null && restaurant != null)
            {
                sondage.Votes.Add(new Vote { Resto = restaurant, Utilisateur = utilisateur });
                this._context.SaveChanges();
            }
        }

        public int CreerUnSondage()
        {
            var sondage = this._context.Sondages.Add(new Sondage { Date = DateTime.UtcNow, Votes = new List<Vote>() });
            this._context.SaveChanges();
            return sondage.Id;
        }

        public List<Resultats> ObtenirLesResultats(int sondageId)
        {
            List<Resultats> resultats = new List<Resultats>();
            var sondage = this._context.Sondages.FirstOrDefault(s => s.Id == sondageId);
            if(sondage != null)
            {
                foreach(var vote in sondage.Votes)
                {
                    var resultat = resultats.FirstOrDefault(r => r.Nom == vote.Resto.Nom);
                    if (resultat == null)
                    {
                        resultat = new Resultats { Nom = vote.Resto.Nom, Telephone = vote.Resto.Telephone };
                        resultats.Add(resultat);
                    }

                    resultat.NombreDeVotes++;
                }
            }

            return resultats;
        }

        public void Dispose()
        {
            this._context.Dispose();
        }

        private string EncodeMD5(string motDePasse)
        {
            string motDePAsseSel = "ChoixDeResto" + motDePasse + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.Default.GetBytes(motDePAsseSel)));
        }
    }
}