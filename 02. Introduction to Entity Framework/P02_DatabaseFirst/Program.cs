namespace P02_DatabaseFirst
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;

    public class Program
    {
        public static void Main()
        {
            var dbContext = new SoftUniContext();

            using (dbContext)
            {
                //P03_EmployeesFullInformation(dbContext);
                //P04_EmployeesWithSalaryOver50000(dbContext);
                //P05_EmployeesFromResearchAndDevelopment(dbContext);
                //P06_AddingANewAddressAndUpdatingEmployee(dbContext);
                //P07_EmployeesAndProjects(dbContext);
                //P08_AddressesByTown(dbContext);
                //P09_Employee147(dbContext);
                //P10_DepartmentsWithMoreThanFiveEmployees(dbContext);
                //P11_FindLatest10Projects(dbContext);
                //P12_IncreaseSalaries(dbContext);
                //P13_FindEmployeesByFirstNameStartingWithSa(dbContext);
                //P14_DeleteProjectById(dbContext);
                //P15_RemoveTowns(dbContext);
            }

        }

        private static void P15_RemoveTowns(SoftUniContext dbContext)
        {
            string townInput = Console.ReadLine();
            Town town = dbContext.Towns.Include(x => x.Addresses).FirstOrDefault(x => x.Name == townInput);
            int addressesCount = town.Addresses.Count;

            foreach (var e in dbContext.Employees)
            {
                e.AddressId = null;
            }

            town.Addresses.Clear();

            dbContext.SaveChanges();

            Console.WriteLine($"{addressesCount} address in {townInput} was deleted");
        }

        private static void P14_DeleteProjectById(SoftUniContext dbContext)
        {
            var projects = dbContext.EmployeesProjects
                .Where(ep => ep.ProjectId == 2);

            foreach (var employeeProject in projects)
            {
                dbContext.EmployeesProjects.Remove(employeeProject);
            }

            var project = dbContext.Projects.Find(2);

            dbContext.Projects.Remove(project);

            dbContext.SaveChanges();

            foreach (var p in dbContext.Projects.Take(10))
            {
                Console.WriteLine($"{p.Name}");
            }
        }

        private static void P13_FindEmployeesByFirstNameStartingWithSa(SoftUniContext dbContext)
        {
            var employees = dbContext.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
            }
        }

        private static void P12_IncreaseSalaries(SoftUniContext dbContext)
        {

            string[] departmentNames = new[] { "Engineering", "Tool Design", "Marketing", "Information Services" };

            var employees = dbContext.Employees
                .Where(e => departmentNames.Contains(e.Department.Name))
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToList();

            foreach (var e in employees)
            {
                //e.Salary *= 1.12m;
                var increasedSalary = e.Salary;

                Console.WriteLine($"{e.FirstName} {e.LastName} (${increasedSalary:F2})");
            }
            //dbContext.SaveChanges();
        }

        private static void P11_FindLatest10Projects(SoftUniContext dbContext)
        {
            var projects = dbContext.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(x => x.Name)
                .ToList();

            foreach (var project in projects)
            {
                Console.WriteLine($"{project.Name}");
                Console.WriteLine($"{project.Description}");
                Console.WriteLine($"{project.StartDate:M/d/yyyy h:mm:ss tt}");
            }
        }

        private static void P10_DepartmentsWithMoreThanFiveEmployees(SoftUniContext dbContext)
        {
            var departments = dbContext.Departments
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    d.Name,
                    d.Manager,
                    d.Employees
                })
                .ToList();

            foreach (var d in departments.OrderBy(x => x.Employees.Count).ThenBy(x => x.Name))
            {
                Console.WriteLine($"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}");

                foreach (var e in d.Employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }

                Console.WriteLine($"----------");
            }
        }

        private static void P09_Employee147(SoftUniContext dbContext)
        {
            var employee = dbContext.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    EmployeeName = e.FirstName + " " + e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeeProjects.Select(p => p.Project.Name).OrderBy(p => p),
                })
                .FirstOrDefault();

            Console.WriteLine($"{employee.EmployeeName} - {employee.JobTitle}");

            foreach (var project in employee.Projects)
            {
                Console.WriteLine(project);
            }
        }

        private static void P08_AddressesByTown(SoftUniContext dbContext)
        {
            var addresses = dbContext.Addresses
                .OrderByDescending(x => x.Employees.Count)
                .ThenBy(x => x.Town.Name)
                .ThenBy(x => x.AddressText)
                .Select(a => new
                {
                    a.AddressText,
                    a.Town.Name,
                    a.Employees.Count
                })
                .Take(10)
                .ToList();

            foreach (var address in addresses)
            {
                Console.WriteLine($"{address.AddressText}, {address.Name} - {address.Count} employees");
            }
        }

        private static void P07_EmployeesAndProjects(SoftUniContext dbContext)
        {
            var projects = dbContext
                .Employees
                .Where(x => x.EmployeeProjects.Any(y =>
                    y.Project.StartDate.Year >= 2001 && y.Project.StartDate.Year <= 2003))
                .Take(30)
                .Select(x => new
                {
                    EmployeeName = x.FirstName + " " + x.LastName,
                    ManagerName = x.Manager.FirstName + " " + x.Manager.LastName,
                    Projects = x.EmployeeProjects.Select(ep => new
                    {
                        ep.Project.Name,
                        ep.Project.StartDate,
                        ep.Project.EndDate
                    })
                });

            foreach (var e in projects)
            {
                Console.WriteLine($"{e.EmployeeName} - Manager: {e.ManagerName}");

                foreach (var p in e.Projects)
                {
                    var endDate = p.EndDate == null ? "not finished" : p.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt");

                    Console.WriteLine($"--{p.Name} - {p.StartDate} - {endDate}");
                }
            }
        }

        private static void P06_AddingANewAddressAndUpdatingEmployee(SoftUniContext dbContext)
        {
            var address = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            Employee employee = dbContext.Employees.SingleOrDefault(e => e.LastName == "Nakov");
            if (employee != null)
            {
                employee.Address = address;
                dbContext.SaveChanges();
            }

            var employees = dbContext.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select(e => new
                {
                    e.Address.AddressText
                })
                .ToList();

            foreach (var e in employees)
            {
                Console.WriteLine(e.AddressText);
            }
        }

        private static void P05_EmployeesFromResearchAndDevelopment(SoftUniContext dbContext)
        {
            var employees = dbContext.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Department.Name,
                    e.Salary
                })
                .ToList();

            foreach (var e in employees)
            {
                Console.WriteLine($"{e.FirstName} {e.LastName} from {e.Name} - ${e.Salary:F2}");
            }

        }

        private static void P04_EmployeesWithSalaryOver50000(SoftUniContext dbContext)
        {
            var employees = dbContext.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName
                })
                .OrderBy(e => e.FirstName);

            foreach (var employee in employees)
            {
                Console.WriteLine(employee.FirstName);
            }
        }

        private static void P03_EmployeesFullInformation(SoftUniContext dbContext)
        {
            var employees = dbContext.Employees
                                .OrderBy(x => x.EmployeeId)
                                .Select(e => new
                                {
                                    e.FirstName,
                                    e.LastName,
                                    e.MiddleName,
                                    e.JobTitle,
                                    e.Salary
                                });

            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }
        }
    }
}
