using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using VehicleWebAPI.Controllers;
using VehicleWebAPI.Models;
using Moq;
using Microsoft.AspNet.Mvc;

namespace VehicleWebAPITests
{
    // For some reason, certain tests are not run as part of "Run All" through Test Explorer.
    // Will pass if run by themselves.
    public class VehiclesControllersTest : IDisposable
    {
        VehiclesController controller;
        Mock<IVehicleRepository> mockVehicleRepo;

        public VehiclesControllersTest()
        {
            mockVehicleRepo = new Mock<IVehicleRepository>();
            //mockVehicleRepo = MockRepository.GenerateMock<IVehicleRepository>();
        }

        [Fact]
        public void Get_NoId_ReturnsVehicles()
        {
            mockVehicleRepo.Setup(x => x.GetAllVehicles()).Returns(new List<Vehicle>() { GenerateTestVehicle(1), GenerateTestVehicle(2) });
            //mockVehicleRepo.Stub(x => x.GetAllVehicles()).Return(new List<Vehicle>() { GenerateTestCar() });
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.Get();
            
            Assert.Equal(2, result.Count());
            var indexable = result.ToList();

            Assert.True(indexable[0].Equals(GenerateTestVehicle(1)));
            Assert.True(indexable[1].Equals(GenerateTestVehicle(2)));
        }

        [Fact]
        public void GetById_NoVehicleFound_Returns404()
        {
            mockVehicleRepo.Setup(x => x.Read(It.IsAny<int>())).Returns((Vehicle)null);
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.GetById(1);

            Assert.IsType(typeof(HttpNotFoundResult), result);
        }

        [Fact]
        public void GetById_VehicleFound_Returns200()
        {
            mockVehicleRepo.Setup(x => x.Read(It.IsAny<int>())).Returns(GenerateTestVehicle(1));
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.GetById(1);
            var okObject = result as HttpOkObjectResult;

            Assert.NotNull(okObject);
            var v = okObject.Value as Vehicle;
            Assert.NotNull(v);
            Assert.True(v.Equals(GenerateTestVehicle(1)));
        }

        [Fact]
        public void Post_NullObject_Returns400()
        {
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.Post(null);

            Assert.IsType(typeof(BadRequestResult), result);
        }

        [Fact]
        public void Post_CreateUnsuccessful_Returns409()
        {
            mockVehicleRepo.Setup(x => x.Create(It.IsAny<Vehicle>())).Returns(false);
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.Post(GenerateTestVehicle(1));
            var statusObject = result as HttpStatusCodeResult;

            // No elegant way to produce or check a Conflict status code, so do this instead
            Assert.NotNull(statusObject);
            Assert.Equal(409, statusObject.StatusCode);
        }

        [Fact]
        public void Post_CreateSuccessful_Returns201()
        {
            mockVehicleRepo.Setup(x => x.Create(It.IsAny<Vehicle>())).Returns(true);
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.Post(GenerateTestVehicle(1));

            Assert.IsType(typeof(CreatedAtRouteResult), result);
        }

        [Fact]
        public void Put_NullObject_Returns400()
        {
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.Put(null);

            Assert.IsType(typeof(BadRequestResult), result);
        }

        [Fact]
        public void Put_UpdateUnsuccessful_Returns404()
        {
            mockVehicleRepo.Setup(x => x.Update(It.IsAny<Vehicle>())).Returns(false);
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.Put(GenerateTestVehicle(1));

            Assert.IsType(typeof(HttpNotFoundResult), result);
        }

        [Fact]
        public void Put_UpdateSuccessful_Returns204()
        {
            mockVehicleRepo.Setup(x => x.Update(It.IsAny<Vehicle>())).Returns(true);
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.Put(GenerateTestVehicle(1));

            Assert.IsType(typeof(NoContentResult), result);
        }

        [Fact]
        public void Delete_DeleteUnsuccessful_Returns404()
        {
            mockVehicleRepo.Setup(x => x.Delete(It.IsAny<int>())).Returns((Vehicle)null);
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.Delete(1);

            Assert.IsType(typeof(HttpNotFoundResult), result);
        }

        [Fact]
        public void Delete_DeleteSuccessful_Returns204()
        {
            mockVehicleRepo.Setup(x => x.Delete(It.IsAny<int>())).Returns(GenerateTestVehicle(1));
            controller = new VehiclesController(mockVehicleRepo.Object);

            var result = controller.Delete(1);

            Assert.IsType(typeof(NoContentResult), result);
        }

        public void Dispose()
        {
            controller.Dispose();
        }

        #region Helper methods

        private Vehicle GenerateTestVehicle(int id, string make = "Test", string model = "Car", int year = 1999)
        {
            return new Vehicle() { Id = id, Make = make, Model = model, Year = year };
        }

        #endregion
    }
}
