using Ardalis.Result;
using BugTracker.Core.Requests;
using BugTracker.Data.Database;
using BugTracker.Data.Model;
using MediatR;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;

namespace BugTrackerTest
{

    public class BugServiceTest : TestBed<ServicesFixture>
    {
        public BugServiceTest(ITestOutputHelper testOutputHelper, ServicesFixture fixture)
            : base(testOutputHelper, fixture)
        {
            var options = _fixture.GetService<IOptions<Options>>(_testOutputHelper)!.Value;
        }


        [Fact]
        public async Task GetBugByIdTest()
        {

            //arrange
            var name = "BugTest1";
            await using var databaseContext = _fixture.GetService<DatabaseContext>(_testOutputHelper);
            await databaseContext.Database.EnsureDeletedAsync();
            await databaseContext.Database.EnsureCreatedAsync();
            databaseContext.Bugs.Add(new Bug()
            {
                Name = "BugTest1"
            });
            await databaseContext.SaveChangesAsync();

            //act
            var getBugByIdRequestHandler =
                _fixture.GetService<IRequestHandler<GetBugByIdRequest, Result<Bug?>>>(_testOutputHelper);
            var result = await getBugByIdRequestHandler.Handle(new GetBugByIdRequest(1), CancellationToken.None);

            //assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(name, result.Value.Name);
        }


        [Fact]
        public async Task GetBugsTest()
        {
            //arrange
            var name = "BugTest1";
            await using var databaseContext = _fixture.GetService<DatabaseContext>(_testOutputHelper);
            await databaseContext.Database.EnsureDeletedAsync();
            await databaseContext.Database.EnsureCreatedAsync();
            databaseContext.Bugs.Add(new Bug() {Name = "BugTest2"});
            databaseContext.Bugs.Add(new Bug() {Name = "Bug_Test3"});
            await databaseContext.SaveChangesAsync();

            //act
            var requestHandler =
                _fixture.GetService<IRequestHandler<GetBugsRequest, Result<BugHeader[]>>>(_testOutputHelper);
#pragma warning disable CS8602
            var result = await requestHandler.Handle(new GetBugsRequest("BugTest"), CancellationToken.None);
#pragma warning restore CS8602

            //assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value);
            Assert.Equal("BugTest2", result.Value.First().Name);
        }

        [Fact]
        public async Task UpdateBugTest()
        {
            //TODO
        }

        [Fact]
        public async Task UpdateBugStatusTest()
        {
            //TODO
        }

        [Fact]
        public async Task DeleteBugTest()
        {
            //TODO
        }
    }
}