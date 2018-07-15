using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Entities.Lookups
{
    public class ActivityCategory
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        // TODO: Navigation properties for Activity
    }
}
