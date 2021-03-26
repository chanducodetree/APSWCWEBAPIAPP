using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ModelService
{
    public class ApplicationUser: IdentityUser
    {
        public string Notes { get; set; }
        public string DisplayName { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public string ProfilePic { get; set; }
        public string Birthday { get; set; }
        public bool IsProfileComplete { get; set; }
        public bool Terms { get; set; }
        public bool IsEmployee { get; set; }
        public string UserRole { get; set; }
        public DateTime AccountCreatedOn { get; set; }
        public bool RememberMe { get; set; }
        public bool IsActive { get; set; }
        public ICollection<AddressModel> UserAddresses { get; set; }
    }
    public class AddressModel
    {
        [Key]
        public int AddressId { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Unit { get; set; }
        [Required]
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Type { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
