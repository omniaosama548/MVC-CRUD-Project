using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }//pk
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "MaxLength should be 50 Characters")]
        [MinLength(3, ErrorMessage = "MinLength Should Be 3 characters")]
        public string Name { get; set; }
        [Range(22, 35, ErrorMessage = "Age Must be from 20 to 35")]
        public int? Age { get; set; }
        [RegularExpression("^\\d{1,5}-[A-Za-z0-9\\s-]+-[A-Za-z0-9\\s-]+-[A-Za-z0-9\\s-]+$",
            ErrorMessage = "Address Must be like:123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        //optional =>ondelete Restrict
        //requried=> ondelete Cascade
        [InverseProperty("Employees")]
        public Department Department { get; set; }
    }
}
