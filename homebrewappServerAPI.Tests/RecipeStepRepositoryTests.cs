using homebrewAppServerAPI.Persistence.Contexts;
using homebrewAppServerAPI.Persistence.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace homebrewappServerAPI.Tests
{
    public class RecipeStepRepositoryTests
    {
        // Prefer this to throw an exception
        [Test]
        public void AddAsync_Null_ReturnsNull()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            var connection = new SqliteConnection(connectionStringBuilder.ToString());

            var options = new DbContextOptionsBuilder<SqliteDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new SqliteDbContext(options))
            {
                var recipeStepRepository = new RecipeStepRepository(context);

                var result = recipeStepRepository.AddAsync(null);

                Assert.IsNull(result.Result);
            }
        }
    }
}
