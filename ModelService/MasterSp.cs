using System;
using System.Collections.Generic;
using System.Text;

namespace ModelService
{
    public class MasterSp
    {

        public string DIRECTION_ID { get; set; }
        public string TYPEID { get; set; }
        public string INPUT_01 { get; set; }
        public string INPUT_02 { get; set; }
        public string INPUT_03 { get; set; }
        public string INPUT_04 { get; set; }
        public string INPUT_05 { get; set; }
        public string INPUT_06 { get; set; }
        public string INPUT_07 { get; set; }
        public string INPUT_08 { get; set; }
        public string INPUT_09 { get; set; }
        public string INPUT_10 { get; set; }
        public string INPUT_11 { get; set; }
        public string INPUT_12 { get; set; }
        public string INPUT_13 { get; set; }
        public string INPUT_14 { get; set; }
        public string INPUT_15 { get; set; }
        public string INPUT_16 { get; set; }
        public string INPUT_17 { get; set; }
        public string INPUT_18 { get; set; }
        public string INPUT_19 { get; set; }
        public string INPUT_20 { get; set; }
        public string INPUT_21 { get; set; }
        public string INPUT_22 { get; set; }
        public string INPUT_23 { get; set; }
        public string INPUT_24 { get; set; }
        public string INPUT_25 { get; set; }
        public string INPUT_26 { get; set; }
        public string INPUT_27 { get; set; }
        public string INPUT_28 { get; set; }
        public string INPUT_29 { get; set; }
        public string INPUT_30 { get; set; }
        public string INPUT_31 { get; set; }
        public string INPUT_32 { get; set; }
        public string INPUT_33 { get; set; }
        public string INPUT_34 { get; set; }
        public string INPUT_35 { get; set; }
        public string USER_NAME { get; set; }
        public string CALL_SOURCE { get; set; }
        public string CALL_PAGE_ACTIVITY { get; set; }
        public string CALL_BRO_APP_VER { get; set; }
        public string CALL_MOBILE_MODEL { get; set; }
        public string CALL_LATITUDE { get; set; }
        public string CALL_LONGITUDE { get; set; }
        public string CALL_IP_IMEI { get; set; }


    }

    public class FamilyListCls 
    {
        public List<MasterSp> FamilyList { get; set; }
    }

    public class EmployeeMasterSp
    {

        public string DIRECTION_ID { get; set; }
        public string TYPEID { get; set; }
        public string INPUT_01 { get; set; }
        public string INPUT_02 { get; set; }
        public string INPUT_03 { get; set; }
        public string INPUT_04 { get; set; }
        public string INPUT_05 { get; set; }
        public string INPUT_06 { get; set; }
        public string INPUT_07 { get; set; }
        public string INPUT_08 { get; set; }
        public string INPUT_09 { get; set; }
        public string INPUT_10 { get; set; }
        public string INPUT_11 { get; set; }
        public string INPUT_12 { get; set; }
        public string INPUT_13 { get; set; }
        public string INPUT_14 { get; set; }
        public string INPUT_15 { get; set; }
        public string INPUT_16 { get; set; }
        public string INPUT_17 { get; set; }
        public string INPUT_18 { get; set; }
        public string INPUT_19 { get; set; }
        public string INPUT_20 { get; set; }
        public string INPUT_21 { get; set; }
        public string INPUT_22 { get; set; }
        public string INPUT_23 { get; set; }
        public string INPUT_24 { get; set; }
        public string INPUT_25 { get; set; }
        public string INPUT_26 { get; set; }
        public string INPUT_27 { get; set; }
        public string INPUT_28 { get; set; }
        public string INPUT_29 { get; set; }
        public string INPUT_30 { get; set; }
        public string INPUT_31 { get; set; }
        public string INPUT_32 { get; set; }
        public string INPUT_33 { get; set; }
        public string INPUT_34 { get; set; }
        public string INPUT_35 { get; set; }
        public string INPUT_36 { get; set; }
        public string INPUT_37 { get; set; }
        public string INPUT_38 { get; set; }
        public string INPUT_39 { get; set; }
        public string INPUT_40 { get; set; }
        public string INPUT_41 { get; set; }
        public string INPUT_42 { get; set; }
        public string INPUT_43 { get; set; }
        public string INPUT_44 { get; set; }
        public string INPUT_45 { get; set; }
        public string INPUT_46 { get; set; }
        public string INPUT_47 { get; set; }
        public string INPUT_48 { get; set; }
        public string INPUT_49 { get; set; }
        public string INPUT_50 { get; set; }
        public string USER_NAME { get; set; }
        public string CALL_SOURCE { get; set; }
        public string CALL_PAGE_ACTIVITY { get; set; }
        public string CALL_BRO_APP_VER { get; set; }
        public string CALL_MOBILE_MODEL { get; set; }
        public string CALL_LATITUDE { get; set; }
        public string CALL_LONGITUDE { get; set; }
        public string CALL_IP_IMEI { get; set; }


    }

    public class DigiLocker
    {
        public string url { get; set; }
        public string code { get; set; }
        public string RedirectURL { get; set; }
        public string token { get; set; }
        public string username { get; set; }
        public string pagename { get; set; }
        public string mime { get; set; }
        public string INPUT_01 { get; set; }
        public string INPUT_02 { get; set; }
        public string DIRECTION_ID { get; set; }
        public string TYPEID { get; set; }
    }

    public class DLDocs
    {
        public List<files> items { get; set; }
        public string resource { get; set; }
    }

    public class files
    {
        public string name { get; set; }
        public string type { get; set; }
        public string size { get; set; }
        public string date { get; set; }
        public string parent { get; set; }
        public List<string> mime { get; set; }
        public string uri { get; set; }
        public string doctype { get; set; }
        public string description { get; set; }
        public string issuerid { get; set; }
        public string issuer { get; set; }
    }

    public class AccessToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }
        public string scope { get; set; }
        public string refresh_token { get; set; }
        public string digilockerid { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string eaadhaar { get; set; }
        public string reference_key { get; set; }
        public string new_account { get; set; }
    }

    public class Item
    {
        public string name { get; set; }
        public string type { get; set; }
        public string size { get; set; }
        public string date { get; set; }
        public string parent { get; set; }
        public string mime { get; set; }
        public string uri { get; set; }
        public string description { get; set; }
        public string issuer { get; set; }
    }

    public class UploadFiles
    {
        public List<Item> items { get; set; }
    }
}
