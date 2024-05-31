using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ST10058357_PROG7311_POE2.Models
{
    public class User : IdentityUser<int>
    {
        
       /* public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }*/

        public string Role { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Full Name")]
        public string FullName => FirstName + " " + LastName;

       /* public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }*/

        public int Age { get; set; }

        /*
        [Display(Name = "Phone Number")]
        
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed {  get; set; }*/

        [Display(Name = "Registration Date")]
        [DataType(DataType.Date)]
        public DateOnly RegistrationDate { get; set; }

        /*
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp {  get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime LockoutEnd {  get; set; }
        public bool LockoutEnabled {  get; set; }
        public int AccessFailedCount {  get; set; }*/




        // If farmer
        public string? Address { get; set; }

        public int? RegEmployeeUserID { get; set; }
        public string? RegEmployeeName { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

        [Display(Name = "Number of Products")]
        public int ProductCount { get; set; }

        [Column("ImagePath")]
        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        


        public User()
        {
            
        }


        public User(int id, string userName, string role, string firstName, string lastName,
            string email, string passwordHash, int age, string phoneNumber,
            DateOnly registrationDate, string? address, int? regEmployeeUserID, string? regEmployeeName, string? imagePath)
        {
            Id = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Age = age;
            PhoneNumber = phoneNumber;
            RegistrationDate = registrationDate;
            Address = address;
            RegEmployeeUserID = regEmployeeUserID;
            RegEmployeeName = regEmployeeName;
            ImagePath = imagePath;
            //Todo add method to get employee
            //TODO add method to get products
            //RegEmployee = GetEmployee(userId);
        }


        /// <summary>
        /// Method to get the employee associated with the farmer
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        /*
       public User GetEmployee(int userID)
       {

       }*/



    }
}

