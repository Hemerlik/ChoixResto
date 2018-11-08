using System;
using System.Collections.Generic;

namespace ChoixResto.Models
{
    public interface IDal : IDisposable
    {
        List<Resto> ObtientTousLesRestaurants();
        void CreerRestaurant(string name, string telephone);
        void ModifierRestaurant(int id, string nom, string telephone);
        bool RestaurantExiste(string nom);
        Utilisateur ObtenirUtilisateur(int id);
        Utilisateur ObtenirUtilisateur(string nom);
        int AjouterUtilisateur(string nom, string motDePasse);
        Utilisateur Authentifier(string nom, string motDePasse);
        bool ADejaVote(int id, string nom);
        int CreerUnSondage();
        void AjouterVote(int sondageId, int restaurantId, int utilisateurId);
        List<Resultats> ObtenirLesResultats(int sondageId);
    }
}
