using Microsoft.AspNetCore.Http;
using System.Xml.Serialization;

namespace Application.Models.ToDos
{
    public class ToDoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }

        [XmlIgnore]
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
    }
}
