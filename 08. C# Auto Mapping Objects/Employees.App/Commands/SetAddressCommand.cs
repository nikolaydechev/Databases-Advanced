namespace Employees.App.Commands
{
    using System.Linq;
    using Employees.App.Commands.Contracts;
    using Employees.Services;

    public class SetAddressCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetAddressCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            var employeeId = int.Parse(data[0]);
            var address = string.Join(" ", data.Skip(1));

            string employeeName = this.employeeService.SetAddress(employeeId, address);

            return $"{employeeName}'s address was set to {address}";
        }
    }
}
