using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using VehicleWebAPI.Controllers;
using VehicleWebAPI.Models;

namespace VehicleWebAPITests
{
    // For some reason, certain tests are not run as part of "Run All" through Test Explorer.
    // Will pass if run by themselves.
    public class VehicleRepositoryTests
    {
        VehicleRepository vehicleRepo;

        public VehicleRepositoryTests()
        {
            vehicleRepo = new VehicleRepository();
            VehicleRepository.Vehicles.Clear();
        }

        [Fact]
        public void Constructor_Default_InitializesKeyToOne()
        {
            Assert.Equal(1, vehicleRepo.Key);
        }

        [Fact]
        public void Create_DuplicateKey_ReturnsFalse()
        {
            VehicleRepository.Vehicles[1] = GenerateTestVehicle(2);

            var result = vehicleRepo.Create(GenerateTestVehicle(1));

            Assert.False(result);
            Assert.Equal(1, VehicleRepository.Vehicles.Count);
            Assert.Equal(1, vehicleRepo.Key);
        }

        [Fact]
        public void Create_NotDuplicateKey_ReturnsTrue()
        {
            var result = vehicleRepo.Create(GenerateTestVehicle(1));

            Assert.True(result);
            Assert.Equal(1, VehicleRepository.Vehicles.Count);
            Assert.Equal(2, vehicleRepo.Key);
        }

        [Fact]
        public void Read_KeyNotFound_ReturnsNull()
        {
            vehicleRepo.Create(GenerateTestVehicle(1));

            var result = vehicleRepo.Read(vehicleRepo.Key);

            Assert.Null(result);
        }

        [Fact]
        public void Read_KeyFound_ReturnsVehicle()
        {
            vehicleRepo.Create(GenerateTestVehicle(1));

            var result = vehicleRepo.Read(1);

            Assert.NotNull(result);
            Assert.Equal(true, result.Equals(GenerateTestVehicle(1)));
        }
        
        [Fact]
        public void Update_KeyNotFound_ReturnsFalse()
        {
            vehicleRepo.Create(GenerateTestVehicle(1));

            var result = vehicleRepo.Update(GenerateTestVehicle(2));

            Assert.False(result);
        }

        [Fact]
        public void Update_KeyFound_ReturnsTrue()
        {
            vehicleRepo.Create(GenerateTestVehicle(1));

            var result = vehicleRepo.Update(GenerateTestVehicle(1, "foo", "bar", 1950));
            var updatedVehicle = vehicleRepo.Read(1);

            Assert.True(result);
            Assert.Equal(true, updatedVehicle.Equals(GenerateTestVehicle(1, "foo", "bar", 1950)));
        }

        [Fact]
        public void Delete_KeyNotFound_ReturnsNull()
        {
            vehicleRepo.Create(GenerateTestVehicle(1));

            var result = vehicleRepo.Delete(vehicleRepo.Key);

            Assert.Null(result);
            Assert.Equal(1, VehicleRepository.Vehicles.Count());
        }

        [Fact]
        public void Delete_KeyFound_ReturnsVehicle()
        {
            vehicleRepo.Create(GenerateTestVehicle(1));

            var result = vehicleRepo.Delete(1);

            Assert.NotNull(result);
            Assert.Equal(0, VehicleRepository.Vehicles.Count());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="make"></param>
        /// <param name="model"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        /// <remarks>Setting the id is largely just an artifact of the controller tests, as the id
        /// will be overwritten on any Create</remarks>
        private Vehicle GenerateTestVehicle(int id, string make = "Test", string model = "Car", int year = 1999)
        {
            return new Vehicle() { Id = id, Make = make, Model = model, Year = year };
        }
    }
}
