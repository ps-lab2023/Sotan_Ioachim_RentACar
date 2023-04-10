using System.ComponentModel.DataAnnotations;

namespace eRental.Model
{
    public class Vehicle
    {
        public int VehicleId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public virtual ICollection<Rental> Rentals { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
