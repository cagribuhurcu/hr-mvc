using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HRProject.Entities.Enums
{
    public enum ExpenseType
    {
        [Display(Name = "Food Expense")]
        FoodExpense = 1,
        [Display(Name = "Travel Expense")]
        TravelExpense = 2,
        [Display(Name = "Vehicle Expense")]
        VehicleExpense = 3,
        [Display(Name = "Communication Expense")]
        CommunicationExpense = 4,
        [Display(Name = "Clothing Expense")]
        ClothingExpense = 5
    }
}
