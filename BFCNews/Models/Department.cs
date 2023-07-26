using System.Collections;

namespace BinhdienNews.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public List<DepartmentUser> DepartmentUsers{ get; set; }
    }
}