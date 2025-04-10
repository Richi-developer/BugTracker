using Ardalis.Result;
using BugTracker.Core.Requests;
using BugTracker.Data.Database;
using BugTracker.Data.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BugTrackerTests
{
    public class BugServiceTest
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IRequestHandler<GetBugByIdRequest, Result<Bug?>> _getBugByIdRequestHandler;

        public BugServiceTest(DatabaseContext databaseContext
            , IRequestHandler<GetBugByIdRequest, Result<Bug?>> getBugByIdRequestHandler
        )
        {
            _databaseContext = databaseContext;
            _getBugByIdRequestHandler = getBugByIdRequestHandler;
            _databaseContext.Database.EnsureDeleted();
            _databaseContext.Database.EnsureCreated();
        }


        
        [Fact]
        public void TestGetById()
        {
            var name = "BugTest1";

            _databaseContext.Bugs.Add(new Bug()
            {
                Name = "BugTest1"
            });
            _databaseContext.SaveChanges();

            var result =_getBugByIdRequestHandler.Handle(new GetBugByIdRequest(1), CancellationToken.None).Result;

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(result.Value.Name, name);

        }
    }
}