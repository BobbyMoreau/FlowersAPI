using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace flowers.api.Entities
{
    public class Flower
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }   
        public int Height { get; set; }
        public int FamilyId { get; set; }

        [ForeignKey("FamilyId")]
        public Family Family { get; set; }
    }
}