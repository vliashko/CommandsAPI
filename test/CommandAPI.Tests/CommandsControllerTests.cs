using System;
using CommandAPI.Data;
using Xunit;
using Moq;
using CommandAPI.Controllers;
using System.Collections.Generic;
using CommandAPI.Models;
using CommandAPI.Profiles;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Dtos;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }

        [Fact]
        public void GetAllComands_ReturnsZeroItems_WhenDBIsEmpty()
        {
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetAllCommands();
            Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact]
        public void GetAllCommands_ReturnsOneiTem_WhenDBHasOneResource()
        {
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetAllCommands();
            
            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<CommandReadDto>;

            Assert.Single(commands);
        }
        [Fact]
        public void GetAllCommands_Returns200OK_WhenDBHasOneResource()
        {
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetAllCommands();

            Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact]
        public void GetAllCommands_ReturnsCorrectType_WhenDBHasOneResource()
        {
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetAllCommands();
            
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }
        [Fact]
        public void GetCommandById_Returns404NotFound_WhenNonExistentIDProvided()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetCommandById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public void GetCommandById_Returns200OK_WhenValidIDProvided()
        {
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(
                    new Command 
                    { 
                        Id = 1,
                        HowTo = "mock",
                        Platform = "Mock",
                        CommandLine = "Mock" 
                    });
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetCommandById(1);

            Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact]
        public void GetCommandById_ReturnsCorrectType_WhenValidIDProvided()
        {
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(
                    new Command 
                    { 
                        Id = 1,
                        HowTo = "mock",
                        Platform = "Mock",
                        CommandLine = "Mock" 
                    });
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.GetCommandById(1);

            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }
        [Fact]
        public void CreateCommand_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
        {
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(
                    new Command 
                    { 
                        Id = 1,
                        HowTo = "mock",
                        Platform = "Mock",
                        CommandLine = "Mock" 
                    });
            
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.CreateCommand(new CommandCreateDto {  });

            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }
        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectSubmitted()
        {
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(
                    new Command 
                    { 
                        Id = 1,
                        HowTo = "mock",
                        Platform = "Mock",
                        CommandLine = "Mock" 
                    });
            
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.CreateCommand(new CommandCreateDto {  });

            Assert.IsType<CreatedAtRouteResult>(result.Result);
        }
        [Fact]
        public void UpdateCommand_Returns204NoContent_WhenValidObjectSubmitted()
        {
            mockRepo.Setup(repo =>
                repo.GetCommandById(1)).Returns(
                    new Command 
                    { 
                        Id = 1,
                        HowTo = "mock",
                        Platform = "Mock",
                        CommandLine = "Mock" 
                    });
            
            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.UpdateCommand(1, new CommandUpdateDto {  });

            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public void UpdateCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.UpdateCommand(0, new CommandUpdateDto {  });

            Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public void PartialCommandUpdate_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.PartialCommandUpdate(0, new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto> {  });

            Assert.IsType<NotFoundResult>(result);
        }
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if(num > 0)
            {
                commands.Add(
                    new Command 
                    {
                        Id = 0,
                        HowTo = "How to generate a migration",
                        CommandLine = "dotnet ef migrations add <Name of Migration>",
                        Platform = ".Net Core EF"
                    });
            }
            return commands;
        }
    }
}