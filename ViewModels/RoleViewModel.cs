using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityProject.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public RoleViewModel() { }

        public RoleViewModel(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
