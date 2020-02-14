namespace Employees.App
{
    using System;
    using AutoMapper;
    using Employees.Data;
    using Employees.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public class StartUp
    {
        public static void Main()
        {
            var serviceProvider = ConfigureServices();
            var commandParser = new CommandParser();
            var engine = new Engine(serviceProvider, commandParser);
            engine.Run();
        }

        public static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeesContext>(options => options.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddTransient<EmployeeService>();

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<AutomapperProfile>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
