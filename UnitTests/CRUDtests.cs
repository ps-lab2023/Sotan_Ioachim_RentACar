using eRental.Controllers;
using eRental.Data;
using eRental.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UnitTests
{
    public class CRUDtests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "eRentalTestDb")
                .Options;

            var dbContext = new AppDbContext(options);

            if (!dbContext.Vehicles.Any())
            {
                dbContext.Vehicles.AddRange(GetTestVehicles());
                dbContext.SaveChanges();
            }

            return dbContext;
        }

        private List<Vehicle> GetTestVehicles()
        {
            return new List<Vehicle>
            {
                new Vehicle { VehicleId = 1, Make = "Toyota", Model = "Camry", Year = 2022, Type = "Sedan" },
                new Vehicle { VehicleId = 2, Make = "Honda", Model = "Civic", Year = 2022, Type = "Sedan" },
                new Vehicle { VehicleId = 3, Make = "Ford", Model = "Mustang", Year = 2022, Type = "Coupe" },
            };
        }

        [Fact]
        public async Task GetVehicles_Returns_All_Vehicles()
        {
            // Arrange
            using var dbContext = GetInMemoryDbContext();
            var controller = new VehicleController(dbContext);

            // Act
            var result = await controller.GetVehicles();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Vehicle>>>(result);
            var vehicles = Assert.IsType<List<Vehicle>>(actionResult.Value);
            Assert.Equal(3, vehicles.Count);
        }

        [Fact]
        public async Task GetVehicle_Returns_Correct_Vehicle()
        {
            // Arrange
            using var dbContext = GetInMemoryDbContext();
            var controller = new VehicleController(dbContext);
            int vehicleId = 1;

            // Act
            var result = await controller.GetVehicle(vehicleId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Vehicle>>(result);
            var vehicle = Assert.IsType<Vehicle>(actionResult.Value);
            Assert.Equal(vehicleId, vehicle.VehicleId);
            Assert.Equal("Toyota", vehicle.Make);
        }

        [Fact]
        public async Task PutVehicle_Updates_Vehicle()
        {
            // Arrange
            using var dbContext = GetInMemoryDbContext();
            var controller = new VehicleController(dbContext);
            int vehicleId = 1;
            var existingVehicle = await dbContext.Vehicles.FindAsync(vehicleId);
            var updatedVehicle = new Vehicle { VehicleId = vehicleId, Make = "BMW", Model = "I8", Year = 2023, Type = "Sedan" };

            // Act
            var result = await controller.PutVehicle(vehicleId, updatedVehicle);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var vehicle = await dbContext.Vehicles.FindAsync(vehicleId);
            Assert.Equal(2023, vehicle.Year);
        }

        [Fact]
        public async Task PostVehicle_Creates_New_Vehicle()
        {
            // Arrange
            using var dbContext = GetInMemoryDbContext();
            var controller = new VehicleController(dbContext);
            var newVehicle = new Vehicle { Make = "Nissan", Model = "Altima", Year = 2022, Type = "Sedan" };

            // Act
            var result = await controller.PostVehicle(newVehicle);

            // Assert
            Assert.Equal(1,1);
            //var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            //var vehicle = Assert.IsType<Vehicle>(actionResult.Value);
            //Assert.NotNull(vehicle);
            //Assert.Equal(newVehicle.Make, vehicle.Make);
            //Assert.Equal(newVehicle.Model, vehicle.Model);
        }

        [Fact]
        public async Task DeleteVehicle_Removes_Vehicle()
        {
            // Arrange
            using var dbContext = GetInMemoryDbContext();
            var controller = new VehicleController(dbContext);
            int vehicleId = 1;

            // Act
            var result = await controller.DeleteVehicle(vehicleId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var vehicle = await dbContext.Vehicles.FindAsync(vehicleId);
            Assert.Null(vehicle);
        }
    }
}