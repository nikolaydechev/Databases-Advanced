namespace Employees.App.Commands
{
    using System;
    using System.Globalization;
    using Employees.App.Commands.Contracts;
    using Employees.Services;

    public class SetBirthdayCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetBirthdayCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(string[] data)
        {
            var employeeId = int.Parse(data[0]);
            var date = DateTime.ParseExact(data[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            
            string employeeName = this.employeeService.SetBirthday(employeeId, date);

            return $"{employeeName}'s birthday was set to {data[1]}";
        }
    }
}
