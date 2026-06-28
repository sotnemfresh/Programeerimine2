using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WindowsForms
{
    public interface IMainView
    {
        IList<Toode> DataSource { get; set; }
        Toode SelectedItem { get; set; }
        void SetPresenter(MainViewPresenter presenter);
        void ShowError(string message, OperationResult result);
        int CurrentId { get; set; }
        string CurrentTitle { get; set; }
        string CurrentFotoUrl { get; set; }
        decimal CurrentPrice { get; set; }
        decimal CurrentStockQuantity { get; set; }
    }
}
