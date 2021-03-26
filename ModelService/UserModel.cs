using System;
using System.Collections.Generic;
using System.Text;

namespace ModelService
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfilePic { get; set; }
        public bool IsProfileComplete { get; set; }
        public bool IsActive { get; set; }
        
    }
    public class DataProtectionKeys
    {
        public string ApplicationUserKey { get; set; }
        public string SendGridProtectionKey { get; set; }
    }
    public static class ListWarehouse
    {
        public static List<WareHouseMaste> ListWareHosueMaster = new List<WareHouseMaste>
        {

          new  WareHouseMaste {
           SNo= "1",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Amdalavalasa -I (Own)",
            WarehouseCode= "VZ01101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own"

        },new  WareHouseMaste

        {
           SNo= "2",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Amdalavalasa -Ii (Own-PEG)",
            WarehouseCode= "VZ01102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own-PEG",

        },new  WareHouseMaste
        {
           SNo= "3",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Palasa (Own)/ Tharlkota",
            WarehouseCode= "VZ01103",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "4",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Sompeta (Own)",
            WarehouseCode= "VZ01104",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "5",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Ponduru (Own)",
            WarehouseCode= "VZ01105",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "6",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Narasannapeta (Own)",
            WarehouseCode= "VZ01106",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "7",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Amdalavalasa (AMC- Pvt) ",
            WarehouseCode= "VZ01207",
            WarehouseType= "AMC",
            TypeCode= "2",
            WarehouseSubType= "AMC-Pvt",

        },new  WareHouseMaste
        {
           SNo= "8",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Rajam(AMC)",
            WarehouseCode= "VZ01208",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "9",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Ponduru(AMC)",
            WarehouseCode= "VZ01209",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "10",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Ampolu (AMC)",
            WarehouseCode= "VZ01210",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "11",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Narasannapeta(AMC) ",
            WarehouseCode= "VZ01211",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "12",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Kotabommali (AMC)",
            WarehouseCode= "VZ01212",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "13",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Nivagam (Vasapa_AMC)",
            WarehouseCode= "VZ01213",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "14",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Palasa -IG",
            WarehouseCode= "VZ01414",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "15",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Palasa -Ii (Harini -IG)",
            WarehouseCode= "VZ01415",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "16",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Rajam(IG)",
            WarehouseCode= "VZ01416",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "17",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Tekkali (PEG)",
            WarehouseCode= "VZ01517",
            WarehouseType= "PEG Godowns",
            TypeCode= "5",
            WarehouseSubType= "PEG",

        },new  WareHouseMaste
        {
           SNo= "18",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Palasa (AMC-Own)",
            WarehouseCode= "VZ01118",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "AMC-Own",

        },new  WareHouseMaste
        {
           SNo= "19",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Jammu (AMC-Hired)",
            WarehouseCode= "VZ01319",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC-Hired",

        },new  WareHouseMaste
        {
           SNo= "20",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Maddilodpeta (IG - Tekkali)",
            WarehouseCode= "VZ01420",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "21",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Bhamini (Own)",
            WarehouseCode= "VZ01121",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "22",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Ichapuram (AMC)",
            WarehouseCode= "VZ01222",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "23",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Anthakapalli (hired- RAJAM)",
            WarehouseCode= "VZ01323",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "24",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "HIRED GODOWN  AMDALAVALASA",
            WarehouseCode= "VZ01324",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "25",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Sarubujili (AMC)",
            WarehouseCode= "VZ01225",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "26",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Ravipadu (IG)",
            WarehouseCode= "VZ01426",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "27",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Singannapalem (IG)",
            WarehouseCode= "VZ01427",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "28",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "KCR Warehousing corp (Hired))",
            WarehouseCode= "VZ01328",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "29",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Palakonda (AMC)",
            WarehouseCode= "VZ01229",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "30",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Tekkali (PEG- Additional) ",
            WarehouseCode= "VZ01530",
            WarehouseType= "PEG Godowns",
            TypeCode= "5",
            WarehouseSubType= "PEG",

        },new  WareHouseMaste
        {
           SNo= "31",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Kotabommali (IG)",
            WarehouseCode= "VZ01431",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "32",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Palakandhyam (IG)",
            WarehouseCode= "VZ01432",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "33",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Naira (IG)",
            WarehouseCode= "VZ01433",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "34",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Budumuru(OWN) ",
            WarehouseCode= "VZ01134",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "35",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Kanchili (OWN)",
            WarehouseCode= "VZ01135",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "36",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Narayanavalasa (Own)",
            WarehouseCode= "VZ01136",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "37",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Amdalavalasa (AMC)",
            WarehouseCode= "VZ01237",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "38",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Srikakulam",
            DistrictCode= "519",
            WarehouseName= "Avanthi Warehousing services PLTD(H)",
            WarehouseCode= "VZ01338",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "39",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Vizianagaram (Own)",
            WarehouseCode= "VZ02101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "40",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Donkinavalasa (Own)",
            WarehouseCode= "VZ02102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "41",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Gajapathinagram (AMC)",
            WarehouseCode= "VZ02203",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "42",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Boghapuram (AMC) ",
            WarehouseCode= "VZ02204",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "43",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Pachipenta (AMC)",
            WarehouseCode= "VZ02205",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "44",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Latchayyapeta (Hired)",
            WarehouseCode= "VZ02306",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "45",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Appayyapeta(Hired-Sethangrm)",
            WarehouseCode= "VZ02307",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "46",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Nsl-Bobbili (Hired)",
            WarehouseCode= "VZ02308",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "47",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Vizianagaram (IG-K.L Puram)",
            WarehouseCode= "VZ02409",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "48",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Jiyannavalasa (IG)",
            WarehouseCode= "VZ02410",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "49",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Cheepurupalli (IG)",
            WarehouseCode= "VZ02411",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "50",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Bobbili (IG)",
            WarehouseCode= "VZ02412",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "51",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Paravathipuram (IG)",
            WarehouseCode= "VZ02413",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "52",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Bobbili (PEG)",
            WarehouseCode= "VZ02514",
            WarehouseType= "PEG Godowns",
            TypeCode= "5",
            WarehouseSubType= "PEG",

        },new  WareHouseMaste
        {
           SNo= "53",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Surampeta (IG)",
            WarehouseCode= "VZ02415",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "54",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Donkinavalasa (AMC)",
            WarehouseCode= "VZ02216",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "55",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Cheepurupalli (AMC)",
            WarehouseCode= "VZ02217",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "56",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Saluru(AMC)",
            WarehouseCode= "VZ02218",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "57",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "MVR Pvt.Godowns(Saluru)",
            WarehouseCode= "VZ02319",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Pvt",

        },new  WareHouseMaste
        {
           SNo= "58",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Paravathipuram (AMC)",
            WarehouseCode= "VZ02220",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "59",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "B.P Varakattu (Cheepurupalli - IG)",
            WarehouseCode= "VZ02421",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "60",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Gajapathinagram (OWN)",
            WarehouseCode= "VZ02122",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "61",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Byripuram (OWN)",
            WarehouseCode= "VZ02123",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "62",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Vizianagaram",
            DistrictCode= "521",
            WarehouseName= "Balijipeta (OWN)",
            WarehouseCode= "VZ02124",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "63",
            Region = "Vizianagaram",
            RegionCode= "R0001",
            District= "Visakhapatnam",
            DistrictCode= "520",
            WarehouseName= "Parawada (OWN)",
            WarehouseCode= "VZ03101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "64",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Balabhadrapuram (IG)",
            WarehouseCode= "KK04418",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "65",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Biccavolu (IG)",
            WarehouseCode= "KK04419",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "66",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Biccavolu (IG) II",
            WarehouseCode= "KK04425",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "67",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Chollangi (IG)",
            WarehouseCode= "KK04421",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "68",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Dwarapudi (IG)",
            WarehouseCode= "KK04412",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "69",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Mandapeta (IG)",
            WarehouseCode= "KK04411",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "70",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Pithapuram (IG)",
            WarehouseCode= "KK04422",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "71",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Rajahmundry (IG)",
            WarehouseCode= "KK04423",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "72",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Ramchandrapuram (IG)",
            WarehouseCode= "KK04415",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "73",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Ravulapalem (IG)",
            WarehouseCode= "KK04413",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "74",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Razole (IG)",
            WarehouseCode= "KK04414",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "75",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Samalkota (IG) ",
            WarehouseCode= "KK04416",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "76",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Tuni (IG)",
            WarehouseCode= "KK04417",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "77",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Vemulapalli (IG)",
            WarehouseCode= "KK04420",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "78",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Kanedu (IG)",
            WarehouseCode= "KK04424",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "79",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Karapa (IG)",
            WarehouseCode= "KK04427",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "80",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Dwarapudi (Own)",
            WarehouseCode= "KK04103",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "81",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Dwarapudi (PEG)",
            WarehouseCode= "KK04104",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "PEG",

        },new  WareHouseMaste
        {
           SNo= "82",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Gollaprolu (Own)",
            WarehouseCode= "KK04109",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "83",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Katrenikona (Own)",
            WarehouseCode= "KK04107",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "84",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Kirlampudi (Own)",
            WarehouseCode= "KK04110",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "85",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Mandapeta (Own)",
            WarehouseCode= "KK04102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "86",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Sampara(Nadakuduru)(O)",
            WarehouseCode= "KK04106",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "O",

        },new  WareHouseMaste
        {
           SNo= "87",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Sakhinetipally (Own)",
            WarehouseCode= "KK04108",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "88",
            Region = "Kakinada",
            RegionCode= "R0002",
            District= "Eastgodavari",
            DistrictCode= "505",
            WarehouseName= "Vemulapalli (Own)",
            WarehouseCode= "KK04105",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "89",
            Region = "TP Gudem",
            RegionCode= "R0003",
            District= "Westgodavari",
            DistrictCode= "523",
            WarehouseName= "Bhimavaram (Own) ",
            WarehouseCode= "TP05101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "90",
            Region = "TP Gudem",
            RegionCode= "R0003",
            District= "Westgodavari",
            DistrictCode= "523",
            WarehouseName= "Eluru (Own) ",
            WarehouseCode= "TP05102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "91",
            Region = "TP Gudem",
            RegionCode= "R0003",
            District= "Westgodavari",
            DistrictCode= "523",
            WarehouseName= "Palakollu (Own) ",
            WarehouseCode= "TP05103",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "92",
            Region = "TP Gudem",
            RegionCode= "R0003",
            District= "Westgodavari",
            DistrictCode= "523",
            WarehouseName= "Palakol (Peg)",
            WarehouseCode= "TP05104",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "PEG",

        },new  WareHouseMaste
        {
           SNo= "93",
            Region = "TP Gudem",
            RegionCode= "R0003",
            District= "Westgodavari",
            DistrictCode= "523",
            WarehouseName= "Tanuku (Own) ",
            WarehouseCode= "TP05105",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "94",
            Region = "TP Gudem",
            RegionCode= "R0003",
            District= "Westgodavari",
            DistrictCode= "523",
            WarehouseName= "Kavitam (Hired)",
            WarehouseCode= "TP05306",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "95",
            Region = "TP Gudem",
            RegionCode= "R0003",
            District= "Westgodavari",
            DistrictCode= "523",
            WarehouseName= "Marteru (Hired) ",
            WarehouseCode= "TP05307",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "96",
            Region = "TP Gudem",
            RegionCode= "R0003",
            District= "Westgodavari",
            DistrictCode= "523",
            WarehouseName= "Nawabpalem (IG)",
            WarehouseCode= "TP05408",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "97",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Krishna",
            DistrictCode= "510",
            WarehouseName= "JAGGAIAHPETA (Own)",
            WarehouseCode= "VJ06101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "98",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Krishna",
            DistrictCode= "510",
            WarehouseName= "GUDIVADA (Own)",
            WarehouseCode= "VJ06102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "99",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Krishna",
            DistrictCode= "510",
            WarehouseName= "GUDLAVALLERU (AMC)",
            WarehouseCode= "VJ06203",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "100",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Krishna",
            DistrictCode= "510",
            WarehouseName= "JAGGAIAHPETA (IG)",
            WarehouseCode= "VJ06404",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "101",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Krishna",
            DistrictCode= "510",
            WarehouseName= "MACHILIPATNAM (IG)",
            WarehouseCode= "VJ06405",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "102",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Krishna",
            DistrictCode= "510",
            WarehouseName= "KOTHAMAJERU (OWN)",
            WarehouseCode= "VJ06106",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "103",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "BAPATLA (Own)",
            WarehouseCode= "VJ07101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "104",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "NARASARAOPET - I&II (Own)",
            WarehouseCode= "VJ07102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "105",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "PIDUGURALLA (Own)",
            WarehouseCode= "VJ07103",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "106",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "TENALI (SYG)",
            WarehouseCode= "VJ07104",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "SYG",

        },new  WareHouseMaste
        {
           SNo= "107",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "TENALI (Own)",
            WarehouseCode= "VJ07105",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "108",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "NADIKUDI (Own)",
            WarehouseCode= "VJ07106",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "109",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "REPALLE (Own)",
            WarehouseCode= "VJ07107",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "110",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "TADEPALLE (Own)",
            WarehouseCode= "VJ07108",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "111",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "TENALI (AMC)",
            WarehouseCode= "VJ07209",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "112",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "SATTENAPALLI (HIRED)",
            WarehouseCode= "VJ07310",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "113",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "NADIKUDI (IG)",
            WarehouseCode= "VJ07411",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "114",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "CHILAKALURIPETA (AMC)",
            WarehouseCode= "VJ07212",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "115",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Guntur",
            DistrictCode= "506",
            WarehouseName= "EDLAPADU (OWN)",
            WarehouseCode= "VJ07113",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "116",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "CHIRALA (Own)",
            WarehouseCode= "VJ08101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "117",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "CHIRALA (PEG - OWN)",
            WarehouseCode= "VJ08102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "118",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "KANDUKUR (OWN)",
            WarehouseCode= "VJ08103",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "119",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "PURUCHURU (OWN)",
            WarehouseCode= "VJ08104",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "120",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "KANDUKUR (AMC)",
            WarehouseCode= "VJ08205",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "121",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "PURUCHURU (AMC)",
            WarehouseCode= "VJ08206",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "122",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "GUNDLAPALLI (HIRED)",
            WarehouseCode= "VJ08307",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "123",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "TANGUTURU (HIRED)",
            WarehouseCode= "VJ08308",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "124",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "THAMMAVARAM (IG)",
            WarehouseCode= "VJ08409",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "125",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "SANTHAMAGULURU (AMC)",
            WarehouseCode= "VJ08210",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "126",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "YEDUGUNDLAPADU (H)",
            WarehouseCode= "VJ08311",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "127",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "UPPALAPADU (H)",
            WarehouseCode= "VJ08312",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "128",
            Region = "Vijayawada",
            RegionCode= "R0004",
            District= "Prakasam",
            DistrictCode= "517",
            WarehouseName= "TNAIDUPALEM (H)",
            WarehouseCode= "VJ08313",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "129",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Nellore",
            DistrictCode= "515",
            WarehouseName= "Nellore (Own) ",
            WarehouseCode= "KD09101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "130",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Nellore",
            DistrictCode= "515",
            WarehouseName= "Atmakur (Own) ",
            WarehouseCode= "KD09102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "131",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Nellore",
            DistrictCode= "515",
            WarehouseName= "Vedayapalem (Own) ",
            WarehouseCode= "KD09103",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "132",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Nellore",
            DistrictCode= "515",
            WarehouseName= "Gudur (Own) ",
            WarehouseCode= "KD09104",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "133",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Nellore",
            DistrictCode= "515",
            WarehouseName= "Kavali(Own) ",
            WarehouseCode= "KD09105",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "134",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Nellore",
            DistrictCode= "515",
            WarehouseName= "Chandrashekarapuram(Own) ",
            WarehouseCode= "KD09106",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "135",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Nellore",
            DistrictCode= "515",
            WarehouseName= "Bogole (Own) ",
            WarehouseCode= "KD09107",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "136",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Nellore",
            DistrictCode= "515",
            WarehouseName= "Bandepalli (own)",
            WarehouseCode= "KD09108",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "137",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Nellore",
            DistrictCode= "515",
            WarehouseName= "Jammipalem-Hired ",
            WarehouseCode= "KD09309",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "138",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Kadapa (Own) ",
            WarehouseCode= "KD10101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "139",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Proddatur (Own) ",
            WarehouseCode= "KD10102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "140",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Sarveyreddipalli (Own) ",
            WarehouseCode= "KD10103",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "141",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Kadapa(IG)",
            WarehouseCode= "KD10404",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "142",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Pulivendula-HG",
            WarehouseCode= "KD10305",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "143",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Korrapadu-HG",
            WarehouseCode= "KD10306",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "144",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Thippaluru-HG",
            WarehouseCode= "KD10307",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "145",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Pottipadu-HG",
            WarehouseCode= "KD10308",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "146",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Kadapa-HG",
            WarehouseCode= "KD10309",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "147",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Potladurthi-HG",
            WarehouseCode= "KD10310",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "148",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kadapa",
            DistrictCode= "504",
            WarehouseName= "Muddanur-HG",
            WarehouseCode= "KD10311",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "149",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Kurnool - I&II (Own) ",
            WarehouseCode= "KD11101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "150",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Dhone (Own) ",
            WarehouseCode= "KD11102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "151",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Allagadda (Own) ",
            WarehouseCode= "KD11103",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "152",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Nandyal (Hired)",
            WarehouseCode= "KD11304",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "153",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Kurnool (IG)",
            WarehouseCode= "KD11405",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "154",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Allagadda(Hired) RK",
            WarehouseCode= "KD11306",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "155",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "AMC-Allagadda(Hired)",
            WarehouseCode= "KD11307",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "156",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Atmakur-Hired (Kurnool)",
            WarehouseCode= "KD11308",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "157",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Orvakal-Hired (Kurnool)",
            WarehouseCode= "KD11309",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "158",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Kodumur-Hired (Kurnool)",
            WarehouseCode= "KD11310",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "159",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Nandavaram - Hired (Kurnool)",
            WarehouseCode= "KD11311",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "160",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Pusulur - Hired (Kurnool)",
            WarehouseCode= "KD11312",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "161",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Govindapalli-Hired",
            WarehouseCode= "KD11313",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "162",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Atmakur-Hired-2 (K-Star)",
            WarehouseCode= "KD11314",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "163",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Kanalapalli-Hired (Sri Rama)",
            WarehouseCode= "KD11315",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "164",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Yerragudidinne-Hired (ALG)",
            WarehouseCode= "KD11316",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "165",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Atmakur-Hired-3 (Tappal)",
            WarehouseCode= "KD11317",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "166",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Maseedpuram-HG (Mahanandiswara)",
            WarehouseCode= "KD11318",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "167",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Nandyal-Hired-2 (Sradda)",
            WarehouseCode= "KD11319",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "168",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Atmakur-HG-4 (Royal)",
            WarehouseCode= "KD11320",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "169",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Maseedpuram-HG-2 (Mahesh)",
            WarehouseCode= "KD11321",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "170",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Koratamaddi-HG",
            WarehouseCode= "KD11322",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "171",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Thalamudipi-HG",
            WarehouseCode= "KD11323",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "172",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Kurnool",
            DistrictCode= "511",
            WarehouseName= "Bethamcherla-IG",
            WarehouseCode= "KD11424",
            WarehouseType= "IG",
            TypeCode= "4",
            WarehouseSubType= "IG",

        },new  WareHouseMaste
        {
           SNo= "173",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Anantapur (Own) ",
            WarehouseCode= "KD12101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "174",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Hindupur (Own) ",
            WarehouseCode= "KD12102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "175",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Guntakal (Own) ",
            WarehouseCode= "KD12103",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "176",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Timmencherla (Hired) ",
            WarehouseCode= "KD12304",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "177",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Atmakur(Hired) -ATP",
            WarehouseCode= "KD12305",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "178",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Ramagiri(Hired) -ATP",
            WarehouseCode= "KD12306",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "Hired",

        },new  WareHouseMaste
        {
           SNo= "179",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Anantapur-AMC-HG",
            WarehouseCode= "KD12307",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "AMC",

        },new  WareHouseMaste
        {
           SNo= "180",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Beluguppa-HG (Shiridi Sainath)",
            WarehouseCode= "KD12308",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "181",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Kanaganapalli-HG",
            WarehouseCode= "KD12309",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "182",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Beluguppa-HG-2(Mahalakshmi)",
            WarehouseCode= "KD12310",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "183",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Anantapur",
            DistrictCode= "502",
            WarehouseName= "Kanekal-HG",
            WarehouseCode= "KD12311",
            WarehouseType= "Hired",
            TypeCode= "3",
            WarehouseSubType= "HG",

        },new  WareHouseMaste
        {
           SNo= "184",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Chittoot",
            DistrictCode= "503",
            WarehouseName= "Chittoor (Own) ",
            WarehouseCode= "KD13101",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        },new  WareHouseMaste
        {
           SNo= "185",
            Region = "Kadapa",
            RegionCode= "R0005",
            District= "Chittoot",
            DistrictCode= "503",
            WarehouseName= "Piler (Own) ",
            WarehouseCode= "KD13102",
            WarehouseType= "Own",
            TypeCode= "1",
            WarehouseSubType= "Own",

        }


    };
    }
}
