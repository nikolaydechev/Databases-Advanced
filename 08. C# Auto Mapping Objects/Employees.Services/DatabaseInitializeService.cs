namespace Employees.Services
{
    using Employees.Data;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseInitializeService
    {
        private readonly EmployeesContext context;

        public DatabaseInitializeService(EmployeesContext context)
        {
            this.context = context;
        }

        public void InitializeDatabase()
        {
            this.context.Database.Migrate();
        }
    }
}
