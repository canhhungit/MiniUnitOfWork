
# Generic Repository Pattern and Unit Of Work for C# .net Core

A simple lean & clean generic repository pattern and unit of work for .net core  abstraction layer EntityFramework Core.

[![Build status](https://ci.appveyor.com/api/projects/status/kju8o0abk7yiep22/branch/master?svg=true)](https://ci.appveyor.com/project/canhhungit/miniunitofwork/branch/master)   
[![NuGet Badge](https://buildstats.info/nuget/MiniUow)](https://www.nuget.org/packages/MiniUow/)

MiniUow supports the following platforms:
.NET 4.5.2+
.NET Platform Standard 2.0
.NET Core

## Installation

The simplest method to install MiniUow into your solution/project is to use NuGet.:

```bash
    nuget Install-Package MiniUow
```

Or via the DotNet Cli

```bash
    dotnet add package MiniUow
```

Check out [Nuget package page](https://www.nuget.org/packages/MiniUow/) for more details.

## Documentation 
Startup.cs
```csharp
//Use the MiniUow Dependency Injection to set up the Unit of Work.
//using MiniUow.DependencyInjection;
public void ConfigureServices(IServiceCollection services)
{
    //---------Other Code-----------
	//.AddUnitOfWork<SampleContext>();
    //Sample for SQL Server:
    services.AddDbContext<SampleContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("SampleDatabase"))).AddUnitOfWork<SampleContext>();
	//Sample for Postgresql:
    //services.AddEntityFrameworkNpgsql().AddDbContext<SampleContext>(opt =>
    //    opt.UseNpgsql(Configuration.GetConnectionString("SampleDatabase"))).AddUnitOfWork<SampleContext>();
    //Sample for MySQL:
    //services.AddDbContext<SampleContext>(options => 
    //    options.UseMySql(Configuration.GetConnectionString("SampleDatabase"))).AddUnitOfWork<SampleContext>();
    //---------Other Code-----------
}
```

Controller or Services
```csharp
private readonly IUnitOfWork _uow;
public ClassConstructor(IUnitOfWork unit)
{
    _uow = unit;
}

public async Task ActionMethod(string value)
{
    //Exists, ExistsAsync, Count, CountAsync, Query, Find, FindAsync, Single, SingleAsync
    //GetPagedList, GetPagedListAsync, GetAll, GetAllAsync

    //Demo method
	bool isExists = _uow.GetRepository<TblUser>().Exists(p => p.Username == value);
	isExists = await _uow.GetRepository<TblUser>().ExistsAsync(p => p.Username == value);
	
	var query = _uow.GetRepository<TblUser>().GetPagedList(index: 0, size: int.MaxValue);
    var data = _uow.GetRepository<TblUser>().GetPagedList(
       predicate: p => p.Username.Contains(value) || p.Email.Contains(value) || p.Name.Contains(value),
       orderBy: p => p.OrderBy(p => p.Username),
       include: p => p.Include(x => x.TblUserGroup),
       index: 0,
       size: 20);
    var items = _uow.GetRepository<TblUser>().GetPagedList(b => new { Username = b.Username, Name = b.Name });

    var users = _uow.GetRepository<TblUser>().GetAll();
    var userGroups = _uow.GetRepository<TblUserGroup>().GetAll();

    var user = _uow.GetRepository<TblUser>().Find(value);
    user = _uow.GetRepository<TblUser>().Single(p => p.Username == value);
}

public TblUser Create()
{
    //Add, AddAsync, Delete, Update
    var user =new TblUser();
    user.Password = CreatePasswordHash(password);
    user.CreateDate = DateTime.Now;
    var data = _uow.GetRepository<TblUser>().Add(user);
    _uow.SaveChanges();
    return data;
}
```
