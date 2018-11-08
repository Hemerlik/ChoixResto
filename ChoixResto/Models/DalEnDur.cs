using System;
using System.Collections.Generic;
using System.Linq;

namespace ChoixResto.Models
{
    public class DalEnDur : IDal
    {
        private List<Resto> _restos;
        private List<Utilisateur> _users;
        private List<Sondage> _sondages;

        public DalEnDur()
        {
            _restos = new List<Resto>
            {
                new Resto{Id = 1, Nom = "Resto Pinambour", Telephone = "0123456789"},
                new Resto{Id = 2, Nom = "Resto Pinière", Telephone = "0132547698"},
                new Resto{Id = 3, Nom = "Resto toro", Telephone = "0987654321"}
            };
            _users = new List<Utilisateur>();
            _sondages = new List<Sondage>();
        }

        public bool ADejaVote(int id, string nom)
        {
            return _sondages.Any(s => s.Id == id && s.Votes.Any(v => v.Utilisateur.Prenom == nom));
        }

        public int AjouterUtilisateur(string nom, string motDePasse)
        {
            var id = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
            _users.Add(new Utilisateur { Id = id, Prenom = nom, MotDePasse = motDePasse });
            return id;
        }

        public void AjouterVote(int sondageId, int restaurantId, int utilisateurId)
        {
            Vote vote = new Vote
            {
                Resto = _restos.FirstOrDefault(r => r.Id == restaurantId),
                Utilisateur = ObtenirUtilisateur(utilisateurId)
            };

            var sondage = _sondages.FirstOrDefault(s => s.Id == sondageId);
            if (sondage != null) sondage.Votes.Add(vote);
        }

        public Utilisateur Authentifier(string nom, string motDePasse)
        {
            return _users.FirstOrDefault(u => u.Prenom == nom && u.MotDePasse == motDePasse);
        }

        public void CreerRestaurant(string name, string telephone)
        {
            var id = _restos.Count == 0 ? 1 : _restos.Max(r => r.Id) + 1;
            _restos.Add(new Resto { Id = id, Nom = name, Telephone = telephone });
        }

        public int CreerUnSondage()
        {
            var id = _sondages.Count == 0 ? 1 : _sondages.Max(s => s.Id) + 1;
            _sondages.Add(new Sondage { Id = id, Date = DateTime.Now, Votes = new List<Vote>() });
            return id;
        }

        public void Dispose()
        {
            _restos = new List<Resto>();
            _users = new List<Utilisateur>();
            _sondages = new List<Sondage>();
        }

        public void ModifierRestaurant(int id, string nom, string telephone)
        {
            var resto = _restos.FirstOrDefault(r => r.Id == id);
            if(resto != null)
            {
                resto.Nom = nom;
                resto.Telephone = telephone;
            }
        }

        public List<Resultats> ObtenirLesResultats(int sondageId)
        {
            List<Resto> restaurants = ObtientTousLesRestaurants();
            List<Resultats> resultats = new List<Resultats>();
            Sondage sondage = _sondages.First(s => s.Id == sondageId);

            foreach (IGrouping<int, Vote> grouping in sondage.Votes.GroupBy(v => v.Resto.Id))
            {
                int idRestaurant = grouping.Key;
                Resto resto = restaurants.First(r => r.Id == idRestaurant);
                int nombreDeVotes = grouping.Count();
                resultats.Add(new Resultats { Nom = resto.Nom, Telephone = resto.Telephone, NombreDeVotes = nombreDeVotes });
            }

            return resultats;
        }

        public Utilisateur ObtenirUtilisateur(int id)
        {
            return _users.FirstOrDefault(u => u.Id == id);
        }

        public Utilisateur ObtenirUtilisateur(string nom)
        {
            return _users.FirstOrDefault(u => u.Prenom == nom);
        }

        public List<Resto> ObtientTousLesRestaurants()
        {
            return _restos;
        }

        public bool RestaurantExiste(string nom)
        {
            return _restos.Any(r => r.Nom == nom);
        }
    }
}
