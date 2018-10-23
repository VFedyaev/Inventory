﻿using System;
using System.Collections.Generic;
using System.Linq;
using CatalogEntities;
using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Interfaces;

namespace Inventory.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public EmployeeService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public EmployeeDTO Get(int id)
        {
            Employee employee = _unitOfWork.Employees.Get(id);

            return BLLEmployeeMapper.EntityToDto(employee);
        }

        public IEnumerable<EmployeeDTO> GetAll()
        {
            List<Employee> employees = _unitOfWork.Employees.GetAll().ToList();

            return BLLEmployeeMapper.EntityToDto(employees);
        }

        public IEnumerable<OwnerInfoDTO> GetEmployeesByName(string fname)
        {
            //IEnumerable<OwnerInfoDTO> employees = _unitOfWork
            //    .Employees
            //    .GetAll()
            //    .Where(e => e.EmployeeFullName.Contains(fname))
            //    .Join(
            //        _unitOfWork
            //            .Departments
            //            .GetAll(),
            //            e => e.DepartmentId,
            //            d => d.DepartmentId,
            //            (e, d) => new OwnerInfoDTO
            //            {
            //                EmployeeId = e.EmployeeId,
            //                FullName = e.EmployeeFullName,
            //                Room = e.EmployeeRoom,
            //                Department = d.DepartmentName
            //            }
            //    );

            IEnumerable<OwnerInfoDTO> employees = (
                from
                    emp in _unitOfWork.Employees.GetAll()
                join
                    pos in _unitOfWork.Positions.GetAll()
                on
                    emp.PositionId equals pos.PositionId
                join
                    dep in _unitOfWork.Departments.GetAll()
                on
                    emp.DepartmentId equals dep.DepartmentId
                join
                    adm in _unitOfWork.Administrations.GetAll()
                on
                    dep.AdministrationId equals adm.AdministrationId
                where
                    emp.EmployeeFullName.IndexOf(fname, StringComparison.CurrentCultureIgnoreCase) >= 0
                select new OwnerInfoDTO
                {
                    EmployeeId = emp.EmployeeId,
                    FullName = emp.EmployeeFullName,
                    Room = emp.EmployeeRoom,
                    Position = pos.PositionName,
                    Department = dep.DepartmentName,
                    Administration = adm.AdministrationName
                }).Take(10);

            return employees;
        }

        public IEnumerable<OwnerInfoDTO> GetEmployeesByName(string fname, string lname)
        {
            IEnumerable<OwnerInfoDTO> employees = (
                from
                    emp in _unitOfWork.Employees.GetAll()
                join
                    pos in _unitOfWork.Positions.GetAll()
                on
                    emp.PositionId equals pos.PositionId
                join
                    dep in _unitOfWork.Departments.GetAll()
                on
                    emp.DepartmentId equals dep.DepartmentId
                join
                    adm in _unitOfWork.Administrations.GetAll()
                on
                    dep.AdministrationId equals adm.AdministrationId
                where
                    emp.EmployeeFullName.IndexOf(fname, StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                    emp.EmployeeFullName.IndexOf(lname, StringComparison.CurrentCultureIgnoreCase) >= 0
                select new OwnerInfoDTO
                {
                    EmployeeId = emp.EmployeeId,
                    FullName = emp.EmployeeFullName,
                    Room = emp.EmployeeRoom,
                    Position = pos.PositionName,
                    Department = dep.DepartmentName,
                    Administration = adm.AdministrationName
                }).Take(10);

            return employees;
        }

        public IEnumerable<OwnerInfoDTO> GetEmployeesByName(string fname, string lname, string mname)
        {
            IEnumerable<OwnerInfoDTO> employees = (
                from
                    emp in _unitOfWork.Employees.GetAll()
                join
                    pos in _unitOfWork.Positions.GetAll()
                on
                    emp.PositionId equals pos.PositionId
                join
                    dep in _unitOfWork.Departments.GetAll()
                on
                    emp.DepartmentId equals dep.DepartmentId
                join
                    adm in _unitOfWork.Administrations.GetAll()
                on
                    dep.AdministrationId equals adm.AdministrationId
                where
                    emp.EmployeeFullName.IndexOf(fname, StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                    emp.EmployeeFullName.IndexOf(lname, StringComparison.CurrentCultureIgnoreCase) >= 0 &&
                    emp.EmployeeFullName.IndexOf(mname, StringComparison.CurrentCultureIgnoreCase) >= 0
                select new OwnerInfoDTO
                {
                    EmployeeId = emp.EmployeeId,
                    FullName = emp.EmployeeFullName,
                    Room = emp.EmployeeRoom,
                    Position = pos.PositionName,
                    Department = dep.DepartmentName,
                    Administration = adm.AdministrationName
                }).Take(10);

            return employees;
        }
    }
}
