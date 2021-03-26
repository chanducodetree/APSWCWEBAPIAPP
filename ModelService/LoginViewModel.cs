using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ModelService
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class User
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string GToken{ get; set; }
    }

    public class InspectionModel
    {
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string Description { get; set; }
        public DateTime uploadeddate { get; set; }        
        public string FilePath { get; set; }
    }

    public class WareHouseMaste
        {
        public string SNo { get; set; }
    public string Region { get; set; }
    public string RegionCode { get; set; }
    public string District { get; set; }
    public string DistrictCode { get; set; }
    public string WarehouseName { get; set; }
    public string WarehouseCode { get; set; }
    public string WarehouseType { get; set; }
    public string TypeCode { get; set; }
    public string WarehouseSubType { get; set; }
}
}
