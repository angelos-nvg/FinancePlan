﻿using AutoMapper;
using FinanceManagementLifesaver.DTO;
using FinanceManagementLifesaver.Data;
using FinanceManagementLifesaver.Interfaces;
using FinanceManagementLifesaver.Models;
using FinanceManagementLifesaver.ServiceResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceManagementLifesaver.DTO.AccountDTO;
using FinanceManagementLifesaver.Validations;
using FinanceManagementLifesaver.DTO.ToDoDTO;
using FinanceManagementLifesaver.Migrations;

namespace FinanceManagementLifesaver.Services
{
    public class ToDoService : IToDoService 
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ToDoService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<ServiceResponse<ToDoSaveDTO>> CreateToDo(ToDoSaveDTO toDo)
        {
            ServiceResponse<ToDoSaveDTO> response = new ServiceResponse<ToDoSaveDTO>();
                ToDo _toDo = new ToDo
                {
                     StartDate = toDo.StartDate,
                     EndDate = toDo.EndDate,
                     Description = toDo.Description,
                     UserId = toDo.UserId,
                };

            await _context.ToDos.AddAsync(_toDo);
            await _context.SaveChangesAsync();
            toDo.Id = _toDo.Id;
            response.Data = toDo;
            return response;
        }
        public async Task<ServiceResponse<IEnumerable<ToDo>>> GetAllToDos()
        {
            ServiceResponse<IEnumerable<ToDo>> response = new ServiceResponse<IEnumerable<ToDo>>();
            IEnumerable<ToDo> todos = await _context.ToDos.ToListAsync();
            response.Data = todos;
            return response;
        }

        public async  Task<ServiceResponse<ToDoIDDTO>> DeleteToDos(int toDoId)
        {
            ServiceResponse<ToDoIDDTO> response = new ServiceResponse<ToDoIDDTO>();
            ToDo toDo = await _context.ToDos.FirstOrDefaultAsync(u => u.Id == toDoId);
            if (toDo == null)
            {
                response.Success = false;
                return response;
            }
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
            response.Success = true;
            return response;
        }
        public async Task<ServiceResponse<ToDoSaveDTO>> UpdateToDo(ToDoSaveDTO todo)
        {
            ServiceResponse<ToDoSaveDTO> response = new ServiceResponse<ToDoSaveDTO>();
            ToDo _todo = await _context.ToDos.FirstOrDefaultAsync(u => u.Id == todo.Id);
            _todo.Description = todo.Description;
            _todo.StartDate = todo.StartDate;
            _todo.EndDate = todo.EndDate;
            _todo.UserId = todo.UserId;
            if (_todo == null)
            {
                response.Success = false;
                response.Message = "todo cannot be edit";
                return response;
            }
            return response;
        }
        public Task<ServiceResponse<ToDo>> GetAllToDosByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<ToDo>> GetToDoByFilter(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<IEnumerable<ToDo>>> GetToDoByUserId(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
