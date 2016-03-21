using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleWebAPI.Models
{
    // TODO: enforce restrictions that worked in web application testbed project
    public class Vehicle
    {
        public int Id { get; set; }

        [Range(1950, 2050)]
        public int Year { get; set; }

        [Required(ErrorMessage = "Specify a manufacturer.")]
        [StringLength(100, MinimumLength = 1)]
        public string Make { get; set; }

        [Required(ErrorMessage = "Specify a model.")]
        [StringLength(100, MinimumLength = 1)]
        public string Model { get; set; }

        // Overridden to aid in testing
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast, return false.
            var v = obj as Vehicle;
            if (v == null)
            {
                return false;
            }

            // Check that fields match
            return (Id == v.Id) && (Year == v.Year) && (Make == v.Make) && (Model == v.Model);
        }

        // Overridden because VS complained about it
        public override int GetHashCode()
        {
            return Id * Year;
        }
    }
}
