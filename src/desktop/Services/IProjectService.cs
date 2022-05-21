using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using Transleet.Desktop.Models;

namespace Transleet.Desktop.Services
{
    public interface IProjectService
    {
        [Get("/projects/{id}")]
        Task<Project> GetProjectByIdAsync(Guid id);

        [Get("/projects")]
        Task<List<Project>> GetAllProjectsAsync();
    }
}
