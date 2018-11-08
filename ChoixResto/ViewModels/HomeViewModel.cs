using ChoixResto.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ChoixResto.ViewModels
{
    public class HomeViewModel
    {
        [Display(Name = "Le message")]
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public Resto Resto { get; set; }
        public string Login { get; set; }
    }
}