
namespace Application.DTO
{
    public class ProjectDTO
    {
        public string Identifier { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

        public ProjectDTO()
        {
        }

        public ProjectDTO(long id, string name, DateOnly startDate, DateOnly? endDate)
        {
            Identifier = "Project";
            Id = id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}