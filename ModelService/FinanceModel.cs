using System;
using System.Collections.Generic;
using System.Text;

namespace ModelService
{
    public class WHCTAPREJCL
    {
        public string LOCATION_NAME { get; set; }
        public string department { get; set; }
        public string estimatioN_AMOUNT { get; set; }
        public string inserteD_BY { get; set; }
        public string requesT_ID { get; set; }
        public string wH_CONS_CAPACITY { get; set; }
        public string status { get; set; }
        public string warehousE_CODE { get; set; }
    }

    public class warehousereq
    {
        public string DIRECTION_ID { get; set; }
        public string TYPEID { get; set; }
        public string INPUT_01 { get; set; }/// <summary>
                                            /// WAREHOUSE_CODE
                                            /// </summary>
        public string INPUT_02 { get; set; }//CONSTRUCTION_LOCATION_NAME
        public string INPUT_03 { get; set; }//ESTIMATION_AMOUNT
        public string INPUT_04 { get; set; }//LATITUDE
        public string INPUT_05 { get; set; }//LONGITUDE
        public string INPUT_06 { get; set; }//REQ_DOCUMENT
        public string INPUT_07 { get; set; }//WH_CONS_AREA
        public string INPUT_08 { get; set; }//WH_CONS_CAPACITY
        public string INPUT_09 { get; set; }//DISTRICT_CODE
        public string INPUT_10 { get; set; }//MANDAL_CODE
        public string INPUT_11 { get; set; }//REGION_CODE
        public string INPUT_12 { get; set; }//REMARKS
        public string INPUT_13 { get; set; }//USER_ROLE
        public string USER_NAME { get; set; }//INSERTED_BY
        public string CALL_SOURCE { get; set; }//IS_WEB_MOBILE
    }

    public class WHLoancl
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
        public string USER_NAME { get; set; }
        public string CALL_SOURCE { get; set; }
    }
}
