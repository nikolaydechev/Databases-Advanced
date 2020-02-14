namespace Employees.DtoModels
{
    using System.Collections.Generic;
    using Employees.Models;

    public class ManagerDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<EmployeeDto> ManagerEmployees { get; set; }

        public int EmployeesCount => this.ManagerEmployees.Count;
    }
}
