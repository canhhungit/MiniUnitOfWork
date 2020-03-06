# Generic Repository Pattern for C# .net Core

A simple lean & clean generic repository pattern for .net core  abstraction layer EntityFramework Core.

[![Build status](https://ci.appveyor.com/api/projects/status/kju8o0abk7yiep22/branch/master?svg=true)](https://ci.appveyor.com/project/canhhungit/miniunitofwork/branch/master)

MiniUnitOfWork supports the following platforms:
.NET 4.0
.NET 4.5.2+
.NET Platform Standard 2.0

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
```bash
    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
    	//using MiniUnitOfWork.DependencyInjection;
        // Use the MiniUnitOfWork Dependency Injection to set up the Unit of Work
        services.AddEntityFrameworkNpgsql().AddDbContext<SampleContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("SampleDatabase"))).AddUnitOfWork<SampleContext>();
        services.AddMvc();
    }
```

HomeController.cs
```bash
    private readonly IUnitOfWork _uow;
    public HomeController(IUnitOfWork unit)
    {
        _uow = unit;
    }

    public void ActionMethod(string value)
    {
        //Demo
        var data = _uow.GetRepository<TblUser>().GetList(
           predicate: p => p.Username.Contains(value) || p.Email.Contains(value) || p.Name.Contains(value),
           orderBy: p => p.OrderBy(p => p.Username),
           include: p => p.Include(x => x.TblUserGroup),
           index: 0,
           size: 20);

        var result = _uow.GetRepository<TblUser>().GetAll();

        var user = _uow.GetRepository<TblUser>().Find(value);
        user = _uow.GetRepository<TblUser>().Single(p => p.Username == value);
    }
```

	
	
	
	
	
	
