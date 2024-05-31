﻿using CityInfoAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfoAPI.Entities
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
            
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Description { get; set; }


        public ICollection<PointOfInterest> PointOfInterests { get; set; } = new List<PointOfInterest>();

        public City(string name)
        {
            this.Name = name;
        }
    }
}
