namespace Employees.App.Commands
{
    using System;
    using System.Text;
    using Employees.App.Commands.Contracts;
    using Employees.Services;

    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeePersonalInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            var employeeId = int.Parse(data[0]);
            var epDto = this.employeeService.EpDtoById(employeeId);

            var birthDay = epDto.BirthDay == null ? "[no birthday specified]" : epDto.BirthDay.ToString();
            var address = epDto.Address ?? "[no address specified]";

            var sb = new StringBuilder();
            sb.AppendLine($"ID: {epDto.Id} - {epDto.FirstName} {epDto.LastName} - ${epDto.Salary:F2}");
            sb.AppendLine($"Birthday: {birthDay}");
            sb.AppendLine($"Address: {address}");

            return sb.ToString().Trim();
        }
    }
}
