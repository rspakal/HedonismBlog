﻿using System.ComponentModel.DataAnnotations;

namespace ServicesLibrary.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Role name must be between 3 and 20 characters long.", MinimumLength = 3)]
        //[LatinCharactersOnly(ErrorMessage = "Only latin symbols allowed")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}