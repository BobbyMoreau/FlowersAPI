using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flowers.api.Entities
{
    public class Family : BaseEntity
    {   
        public ICollection<Flower> Flowers {get; set;}
    }
}