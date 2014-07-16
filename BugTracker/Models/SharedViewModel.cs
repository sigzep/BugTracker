using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BugTracker.Models
{
    //used to display a single role or user with with a checkbox, within a list structure 
    public class SelectUserViewModel
    {
        public SelectUserViewModel() { }

        public SelectUserViewModel(IdentityRole role)
        {
            this.Name = role.Name;
            this.Id = role.Id;
        }

        public SelectUserViewModel(ApplicationUser user)
        {
            this.Name = user.UserName;
            this.Id = user.Id;
        }
          
        public bool Selected { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
    }

    public class AssignUserViewModel
    {
        [Required]
        public string Id { get; set; }
        public string Name { get; set; }
        public List<SelectUserViewModel> ItemList { get; set; }

        public AssignUserViewModel()
        {
            this.ItemList = new List<SelectUserViewModel>();
        }

        //Enable initialization with an instance of ApplicationUser;
        public AssignUserViewModel (ApplicationUser user) : this()
        {
            var Db = new ApplicationDbContext();

            this.Id = user.Id;
            this.Name = user.UserName;
            var dataset = Db.Roles;
            foreach (var item in dataset)
            {
                // A SelectUserViewModel instance will be used by Editor Template
                var uvm = new SelectUserViewModel(item);
                this.ItemList.Add(uvm);
            }
            //Set the Selected property to true for those roles for
            //which the current user is a member;
            foreach (var item in user.Roles)
            {
                var checkitem = this.ItemList.FirstOrDefault(r => r.Id == item.RoleId);
                if (checkitem != null)
                    checkitem.Selected = true;
            }

        }
    }
}