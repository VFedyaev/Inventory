﻿using Inventory.BLL.DTO;
using Inventory.BLL.Infrastructure;
using Inventory.BLL.Interfaces;
using Inventory.DAL.Entities;
using Inventory.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Inventory.BLL.Services
{
    public class HistoryService : IHistoryService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        public HistoryService(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public HistoryDTO Get(Guid id)
        {
            History history = _unitOfWork.History.Get(id);

            return Mapper.Map<HistoryDTO>(history);
        }

        public HistoryDTO Get(Guid? id)
        {
            if (id == null)
                throw new ArgumentNullException();

            History history = _unitOfWork.History.Get(id);
            if (history == null)
                throw new NotFoundException();

            return Mapper.Map<HistoryDTO>(history);
        }

        public IEnumerable<HistoryDTO> GetAll()
        {
            List<History> histories = _unitOfWork.History.GetAll().ToList();

            return Mapper.Map<IEnumerable<HistoryDTO>>(histories);
        }

        public void Add(HistoryDTO historyDTO)
        {
            History history = Mapper.Map<History>(historyDTO);
            history.Id = Guid.NewGuid();
            history.ChangeDate = DateTime.Now;
            _unitOfWork.History.Create(history);
            _unitOfWork.Save();
        }

        public void Update(HistoryDTO historyDTO)
        {
            History history = Mapper.Map<History>(historyDTO);
            history.ChangeDate = DateTime.Now;
            _unitOfWork.History.Update(history);
            _unitOfWork.Save();
        }

        public void Delete(Guid id)
        {
            History history = _unitOfWork.History.Get(id);

            if (history == null)
                throw new NotFoundException();

            _unitOfWork.History.Delete(id);
            _unitOfWork.Save();
        }

        public IEnumerable<HistoryDTO> GetFilteredList(FilterParamsDTO parameters)
        {
            IEnumerable<HistoryDTO> filteredList = GetAll();

            if (!string.IsNullOrEmpty(parameters.EquipmentId))
            {
                Guid guidEquipmentId = Guid.Parse(parameters.EquipmentId);
                filteredList = filteredList.Where(h => h.EquipmentId == guidEquipmentId);
            }

            if (!string.IsNullOrEmpty(parameters.EmployeeId))
            {
                int intEmployeeId = int.Parse(parameters.EmployeeId);
                filteredList = filteredList.Where(h => h.EmployeeId == intEmployeeId);
            }

            if (!string.IsNullOrEmpty(parameters.RepairPlaceId))
            {
                Guid guidRepairPlaceId = Guid.Parse(parameters.RepairPlaceId);
                filteredList = filteredList.Where(h => h.RepairPlaceId == guidRepairPlaceId);
            }

            if (!string.IsNullOrEmpty(parameters.StatusTypeId))
            {
                Guid guidStatusTypeId = Guid.Parse(parameters.StatusTypeId);
                filteredList = filteredList.Where(h => h.StatusTypeId == guidStatusTypeId);
            }

            return filteredList.OrderBy(h => h.Employee.EmployeeFullName);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
