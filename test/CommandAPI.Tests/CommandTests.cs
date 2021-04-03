using System;
using CommandAPI.Models;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        private Command testCommand;
        public CommandTests()
        {
            testCommand = new Command 
            {
                HowTo = "Do something",
                Platform = "XInit",
                CommandLine = "dotnet test"
            };
        }
        [Fact]
        public void CanChangeHowTo()
        {
            // Arrange

            // Act
            testCommand.HowTo = "Execute Unit Tests";

            //Assert
            Assert.Equal("Execute Unit Tests", testCommand.HowTo);
        }

        public void Dispose()
        {
            testCommand = null;
        }
    }
}