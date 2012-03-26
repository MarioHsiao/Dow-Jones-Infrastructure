using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DowJones.Web.Showcase.Models
{
    public class Employee
    {
        [Required]
        [Editable(false)]
        public int ID { get; private set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be greater than 0")]
        [DataType(DataType.Currency)]
        public double Salary { get; set; }

        public Employee()
        {
        }


        private static readonly List<Employee> Employees = new List<Employee>{
                new Employee   { ID = 1, FirstName="Rahul", LastName="Kumar", Salary=45000},
                new Employee   { ID = 2, FirstName="Jose", LastName="Mathews", Salary=25000},
                new Employee   { ID = 3, FirstName="Ajith", LastName="Kumar", Salary=25000},
                new Employee   { ID = 4, FirstName="Scott", LastName="Allen", Salary=35000},
                new Employee   { ID = 5, FirstName="Abhishek", LastName="Nair", Salary=125000}
            };

        public static IEnumerable<Employee> GetAll()
        {
            return Employees;
        }

        public static Employee Get(int id)
        {
            return Employees.Single(x => x.ID == id);
        }

        public void Save()
        {
            if (Employees.Contains(this))
                return;
            
            ID = Employees.Max(x => x.ID) + 1;
            Employees.Add(this);
        }
    }
}