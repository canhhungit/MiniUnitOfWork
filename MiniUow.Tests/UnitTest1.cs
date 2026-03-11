using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUow.Tests;

public class UnitOfWorkTests
{
    private static TestDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestDbContext(options);
    }

    [Fact]
    public void GetRepository_ReturnsSameInstance()
    {
        using var context = CreateContext();
        using var uow = new UnitOfWork<TestDbContext>(context);

        var repo1 = uow.GetRepository<User>();
        var repo2 = uow.GetRepository<User>();

        Assert.Same(repo1, repo2);
    }

    [Fact]
    public void Add_And_SaveChanges_Persists()
    {
        using var context = CreateContext();
        using var uow = new UnitOfWork<TestDbContext>(context);
        var repo = uow.GetRepository<User>();

        repo.Add(new User { Name = "Alice" });
        var rows = uow.SaveChanges();

        Assert.Equal(1, rows);
        Assert.Equal(1, context.Users.Count());
    }

    [Fact]
    public async Task AnyAsync_WithEmptySet_ReturnsFalse()
    {
        using var context = CreateContext();
        using var uow = new UnitOfWork<TestDbContext>(context);
        var repo = uow.GetRepository<User>();

        var exists = await repo.AnyAsync();

        Assert.False(exists);
    }

    [Fact]
    public async Task GetPagedListAsync_ReturnsExpectedPage()
    {
        using var context = CreateContext();
        context.Users.AddRange(
            new User { Name = "U1" },
            new User { Name = "U2" },
            new User { Name = "U3" },
            new User { Name = "U4" },
            new User { Name = "U5" }
        );
        await context.SaveChangesAsync();

        using var uow = new UnitOfWork<TestDbContext>(context);
        var repo = uow.GetRepository<User>();

        var page = await repo.GetPagedListAsync(predicate: _ => true, index: 0, size: 2);

        Assert.Equal(5, page.Count);
        Assert.Equal(3, page.Pages);
        Assert.Equal(2, page.Items.Count());
    }
}

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}

public class User
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
