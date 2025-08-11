using NetArchTest.Rules;
using ProductService.Abstractions;
using ProductService.Application.Abstractions.Messaging;
using Xunit;

namespace ProductService.UnitTests.ArchitectureTests
{
    public class ArchitectureUnitTests
    {
        [Fact]
        public void PersistenceLayer_ShouldNotDependOnInfrastructureLayer()
        {
            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("ApiTemplate.Persistence")
                .ShouldNot()
                .HaveDependencyOn("ApiTemplate.Infrastructure")
                .GetResult();

            Assert.True(result.IsSuccessful, "Persistence layer should not depend on infrastructure layer");
        }

        [Fact]
        public void InfrastructureLayer_ShouldNotDependOnApplicationLayer()
        {
            TestResult result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("ApiTemplate.Infrastructure")
                .ShouldNot()
                .HaveDependencyOn("ApiTemplate.Application")
                .GetResult();

            Assert.True(result.IsSuccessful, "Infrastructure layer should not depend on Application layer");
        }

        [Fact]
        public void CommandHandlerInApplicationLayer_ShouldImplementICommandHandlerInterface()
        {
            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("ApiTemplate.Application.Commands")
                .And()
                .HaveNameEndingWith("Handler")
                .Should()
                .ImplementInterface(typeof(ICommandHandler<>))
                .GetResult();

            Assert.True(result.IsSuccessful, "Command handler in application layer should implement ICommandHandler interface");
        }

        [Fact]
        public void QuerryHandlerInApplicationLayer_ShouldImplementIQueryHandlerInterface()
        {
            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("ApiTemplate.Application.Queries")
                .And()
                .HaveNameEndingWith("Handler")
                .Should()
                .ImplementInterface(typeof(IQueryHandler<,>))
                .GetResult();

            Assert.True(result.IsSuccessful, "Queries handler in application layer should implement IQueryHandler interface");
        }

        [Fact]
        public void CommandInApplicationLayer_ShouldImplementICommandInterface()
        {
            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("ApiTemplate.Application.Commands")
                .And()
                .HaveNameEndingWith("Command")
                .Should()
                .ImplementInterface(typeof(ICommand))
                .GetResult();

            Assert.True(result.IsSuccessful, "Commands in application layer should implement ICommand interface");
        }

        [Fact]
        public void QuerryInApplicationLayer_ShouldImplementIQueryInterface()
        {
            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("ApiTemplate.Application.Queries")
                .And()
                .HaveNameEndingWith("Query")
                .Should()
                .ImplementInterface(typeof(IQuery<>))
                .GetResult();

            Assert.True(result.IsSuccessful, "Queries in application layer should implement IQuery interface");
        }

        [Fact]
        public void ControllersInMainLayer_ShouldImplementApiController()
        {
            var result = Types.InCurrentDomain()
                .That()
                .ResideInNamespace("ApiTemplate.Controllers")
                .And()
                .HaveNameEndingWith("Controller")
                .Should()
                .Inherit(typeof(ApiController))
                .GetResult();

            Assert.True(result.IsSuccessful, "Controllers in main layer should inherit ApiController.");
        }
    }
}