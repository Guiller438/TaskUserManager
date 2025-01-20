﻿using TaskUserManager.DTOs;
using TaskUserManager.Models;
using TaskUserManager.Repositories;

namespace TaskUserManager.Service
{
    public class TaskUsersService : ITaskUserService
    {
        private readonly ITaskUserRepository _repository;

        public TaskUsersService(ITaskUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TfaTask>> GetTasksByCategoryIdAsync(int categoryId)
        {
            return await _repository.GetTasksByCategoryIdAsync(categoryId);
        }

        public async Task<IEnumerable<TfaUsersTask>> GetTasksByUserIdAsync(int userId)
        {
            return await _repository.GetTasksByUserIdAsync(userId);
        }

        public async Task AddAsync(int TaskId)
        {
            var AssignToTask = await _repository.GetTaskAsync(TaskId);

            // Verificar si existe una categoría
            if (AssignToTask == null)
            {
                throw new InvalidOperationException("No se encontró ninguna categoría.");
            }

            // Obtener el ID de la categoría
            var categoryId = AssignToTask.CategoryId;

            if (categoryId == null)
            {
                return; // O maneja el caso según la lógica de negocio
            }

            // Obtener los colaboradores asociados a la categoría
            var collaboratorIds = await _repository.GetUserIdsByCategoryIdAsync(categoryId.Value);

            // Si no hay colaboradores asociados, simplemente retorna
            if (!collaboratorIds.Any())
            {
                return;
            }

            // Crea registros en TfaUsersTask para cada colaborador
            var taskUsers = collaboratorIds.Select(userId => new TfaUsersTask
            {
                UserTaskId = AssignToTask.TaskId, // ID de la tarea recién creada
                UserId = userId,
                StatusTask = false, // Estado pendiente
                EvidencePath = null,
                EvidenceUploadDate = null
            });

            // Inserta los registros en la tabla TfaUsersTask
            await _repository.AddRangeAsync(taskUsers);
        }


        public async Task<IEnumerable<int>?> GetUserIdsByCategoryIdAsync(int categoryId)
        {
            return await _repository.GetUserIdsByCategoryIdAsync(categoryId);
        }

        public async Task<TfaCategory?> GetLastCategoryAsync()
        {
            return await _repository.GetLastCategoryAsync();
        }

        public async Task AprobacionAsync(int userId, int userTaskId)
        {
            await _repository.AprobacionAsync(userId, userTaskId);
        }

        public async Task SubirImagenTarea(UpdateEvidence updateEvidence)
        {
             await _repository.SubirImagenTarea(updateEvidence);
        }

        public async Task DeleteTaskAsync(int taskId)
        {
            await _repository.DeleteTaskAsync(taskId);
        }
    }
}
