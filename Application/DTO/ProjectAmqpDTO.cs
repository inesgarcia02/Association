using System;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.DTO
{
    public class ProjectGatewayDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public ProjectGatewayDTO()
        {
        }

        public ProjectGatewayDTO(long id, string name, DateOnly startDate, DateOnly? endDate)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        public static string Serialize(ProjectDTO projectDTO)
        {
            var jsonMessage = JsonSerializer.Serialize(projectDTO);
            return jsonMessage;
        }

        public static ProjectGatewayDTO Deserialize(string projectDTOString)
        {
            return JsonSerializer.Deserialize<ProjectGatewayDTO>(projectDTOString);
        }

        public static ProjectDTO ToDTO(string projectDTOString)
        {
            ProjectGatewayDTO projectGatewayDTO = Deserialize(projectDTOString);
            ProjectDTO projectDTO = new ProjectDTO(projectGatewayDTO.Id, projectGatewayDTO.Name, projectGatewayDTO.StartDate,
                projectGatewayDTO.EndDate);
            return projectDTO;
        }
    }
}