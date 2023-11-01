using MyFleetManagerPro.Models;
using System.Transactions;

namespace MyFleetManagerPro.ViewModels
{
    public class AllTransactionsViewModel
    {
        public MyFleetManagerPro.Models.Transaction transaction { get; set; }
        public Vehicle vehicle { get; set; }


    }
}
