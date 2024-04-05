using System.ComponentModel.DataAnnotations;

namespace azure.Model
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int JobTitleID { get; set; }

    }
}
