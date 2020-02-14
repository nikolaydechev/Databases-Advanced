namespace Employees.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Employees.Data;
    using Employees.DtoModels;
    using Employees.Models;
    using Microsoft.EntityFrameworkCore;

    public class EmployeeService
    {
        private readonly EmployeesContext context;

        public EmployeeService(EmployeesContext context)
        {
            this.context = context;
        }

        public EmployeeDto ById(int employeeId)
        {
            var employee = this.context.Employees.Find(employeeId);

            var employeeDto = Mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public void AddEmployee(EmployeeDto dto)
        {
            var employee = Mapper.Map<Employee>(dto);

            this.context.Employees.Add(employee);

            this.context.SaveChanges();
        }

        public string SetBirthday(int employeeId, DateTime birthDay)
        {
            var employee = this.context.Employees.Find(employeeId);

            employee.BirthDay = birthDay;

            this.context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public string SetAddress(int employeeId, string address)
        {
            var employee = this.context.Employees.Find(employeeId);

            employee.Address = address;

            this.context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public EmployeePersonalDto EpDtoById(int employeeId)
        {
            var employee = this.context.Employees.Find(employeeId);

            var epDto = Mapper.Map<EmployeePersonalDto>(employee);

            return epDto;
        }

        public string SetManager(int employeeId, int managerId)
        {
            var employee = this.context.Employees.Find(employeeId);
            var manager = this.context.Employees.Find(managerId);

            employee.Manager = manager;

            this.context.SaveChanges();

            return
                $"{manager.FirstName} {manager.LastName} was set as a manager to {employee.FirstName} {employee.LastName}";
        }

        public ManagerDto GetManager(int managerId)
        {
            var manager = this.context.Employees
                .Include(m => m.ManagerEmployees)
                .SingleOrDefault(m => m.Id == managerId);

            var managerDto = Mapper.Map<ManagerDto>(manager);

            return managerDto;
        }

        public EmployeeManagerDto[] ListEmployeesOlderThan(int age)
        {
            var currentDate = DateTime.Now;

            var employees = this.context.Employees
                .Where(e => e.BirthDay != null)
                .Where(e => (currentDate.Year - e.BirthDay.Value.Year) > age)
                .Include(e => e.Manager)
                .OrderByDescending(e => e.Salary)
                .ProjectTo<EmployeeManagerDto>()
                .ToArray();

            return employees;
        }
    }
}
