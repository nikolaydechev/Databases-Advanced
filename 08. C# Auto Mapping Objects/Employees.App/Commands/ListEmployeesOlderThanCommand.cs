namespace Employees.App.Commands
{
    using System;
    using System.Text;
    using Employees.App.Commands.Contracts;
    using Employees.Services;

    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ListEmployeesOlderThanCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] data)
        {
            var age = int.Parse(data[0]);

            var employees = this.employeeService.ListEmployeesOlderThan(age);

            if (employees == null)
            {
                throw new ArgumentException($"No employees older than {age} age.");
            }

            var sb = new StringBuilder();
            foreach (var employee in employees)
            {
                var manager = employee.Manager == null ? "[no manager]" : employee.Manager.LastName;
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - ${employee.Salary:f2} - Manager: {manager}");
            }

            return sb.ToString().Trim();
        }
    }
}
