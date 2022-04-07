
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sparky.WebApp.Api.Controllers;
using Sparky.WebApp.Api.Dtos;
using Sparky.WebApp.Api.Models;
using Sparky.WebApp.Api.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SparkyTesting
{
    
    public class TestChargingStationLogic
    {
        Mock<IChargingStationService> mockCharginService;
        public TestChargingStationLogic()
        {
            mockCharginService = new Mock<IChargingStationService>();
        }
        [Fact]
        public async Task TestNoSlotsSpecified()
        {
            
            
            var chargingStation = new ChargingStation()
            {
                NumOfSlots = 0,
                Lat = 50,
                Lon = 50,
                Name = "cs10"

            };
            mockCharginService.Setup(x => x.PostAsync(chargingStation));
            var csController = new ChargingStationsController(mockCharginService.Object);
            var result = await csController.Post(chargingStation);
            var res = result.Result as BadRequestObjectResult;
            Assert.Equal("slots can not be null", res.Value);

        }
        [Fact]
        public async Task TestGetAll()
        {
            
            var stations = TestStations();
            mockCharginService.Setup(x => x.GetAllStations()).ReturnsAsync(stations);
            var csController = new ChargingStationsController(mockCharginService.Object);
            var result = await csController.GetAll();
            var res = result.Result as OkObjectResult;
            Assert.Equal(stations , res.Value);

        }
        private List<ChargingStationDto> TestStations()
        {
            var stations = new List<ChargingStationDto>();
            stations.Add(new ChargingStationDto
            {
                Name = "test1",
                Lat = 10,
                Lon = 10,
                NumOfSlots = 2
            });
            stations.Add(new ChargingStationDto
            {
                Name = "test2",
                Lat = 10,
                Lon = 10,
                NumOfSlots = 2
            });
            return stations;

        }
        [Fact]
        public async Task TestPollIsLoanRequest()
        {
            
            mockCharginService.Setup(x => x.PollIsLoanRequested(1)).ReturnsAsync(true);
            var csController = new ChargingStationsController(mockCharginService.Object);
            var result = await csController.PollIsLoanRequested(1);
            var res = result;
            Assert.True(res.Value);
        }
        [Fact]
        public async Task TestPollIsReturnRequest()
        {

            mockCharginService.Setup(x => x.PollIsReturnRequested(1)).ReturnsAsync(true);
            var csController = new ChargingStationsController(mockCharginService.Object);
            var result = await csController.PollReturnRequest(1);
            var res = result;
            Assert.True(res.Value);
        }
    }   
}
