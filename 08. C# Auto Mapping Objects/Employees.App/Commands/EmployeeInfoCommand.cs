namespace Employees.App.Commands
{
    using System.Text;
    using Employees.App.Commands.Contracts;
    using Employees.Services;

    public class EmployeeInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeeInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            var employeeId = int.Parse(data[0]);

            var employeeDto = this.employeeService.ById(employeeId);

            var sb = new StringBuilder();
            sb.AppendLine(
                $"ID: {employeeDto.Id} - {employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:f2}");

            return sb.ToString().Trim();
        }
    }
}
