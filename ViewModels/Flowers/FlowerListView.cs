using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flowers.api.ViewModels
{
    public class FlowerListView : BaseViewModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
    }
}