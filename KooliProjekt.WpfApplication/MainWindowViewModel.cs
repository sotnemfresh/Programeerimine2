using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public class MainWindowViewModel
    {
        public IList<Toode> Data 
        { 
            get
            {
                var items = new List<Toode>
                {
                    new Toode { Id = 1, Name = "Test 1" },
                    new Toode { Id = 2, Name = "Test 2" },
                    new Toode{ Id = 3, Name = "Test 3" }
                };

                return items;
            }
        }

        public object SelectedItem { get; set; }
    }
}
