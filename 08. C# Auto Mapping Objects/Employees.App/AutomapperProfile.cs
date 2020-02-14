namespace Employees.App
{
    using AutoMapper;
    using Employees.DtoModels;
    using Employees.Models;

    class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            this.CreateMap<Employee, EmployeeDto>();
            this.CreateMap<EmployeeDto, Employee>();
            this.CreateMap<Employee, EmployeePersonalDto>();
            this.CreateMap<Employee, ManagerDto>();
        }
    }
}
