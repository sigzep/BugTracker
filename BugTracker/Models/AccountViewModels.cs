using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }
    public class ExternalLoginListViewModel
    {
        public string Action { get; set; }
        public string ReturnUrl { get; set; }
    }
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name or Email")]
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public string UserId {get; set; }

        [Required]
        [Display(Name = "User name")]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        //New fields added to extend Application User Class:
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //Return a pre-populated instance of ApplicationUser:
        public ApplicationUser GetUser()
        {
            var user = new ApplicationUser()
            {
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
            };
            this.UserId = user.Id;
            return user;
        }
    }

   // uses views ListRoles and CreateRoles

    public class RolesViewModel
    {
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string RoleId { get; set; }

        public RolesViewModel() {}

        public RolesViewModel(IdentityRole role) : this()
        {
            this.RoleName = role.Name;
            this.RoleId = role.Id;
        }

    }

    public class EditRolesViewModel
    {
        public string RoleName { get; set; }
        public string RoleId { get; set; }

        public EditRolesViewModel() { }

        public EditRolesViewModel(IdentityRole role) : this()
        {
            this.RoleName = role.Name;
        }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }

    }
    public class EditUserViewModel
    {
        public EditUserViewModel() { }

        //Allow Initialization with an instance of ApplicationUser:
        public EditUserViewModel(ApplicationUser user)
        {
            this.UserId = user.Id;                /* this is always local to the class, in this case EditUserViewModel */
            this.UserName = user.UserName;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Email = user.Email;

        }
        public string UserId;  /* get set not needed because not being inputed, don't need to get or set */

        [Required]
        [Display (Name="User Name")]
        public string UserName{get; set;}

        [Required]
        [Display (Name="First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display (Name="Last Name")]
        public string LastName { get; set; }
        
        [Required]
        [Display (Name="Email")]
        public string Email { get; set; }

    }
}
