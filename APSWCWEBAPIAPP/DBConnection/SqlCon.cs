using APSWCWEBAPIAPP.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ModelService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace APSWCWEBAPIAPP.DBConnection
{
    public class SqlCon
    {
        private readonly string _connectionString;
       private string exFolder = Path.Combine("ExceptionLogs");
        private string exPathToSave = string.Empty;
        public SqlCon(IConfiguration configuration)
        {
           exPathToSave= Path.Combine(Directory.GetCurrentDirectory(), exFolder);
            _connectionString = configuration.GetConnectionString("apswcproddb");
        }

        public async Task<dynamic> GetApswcWareHouseMaster()
        {
            try
            {
                MasterSp m = new MasterSp();
                m.DIRECTION_ID = "1";
                m.TYPEID = "101";
                return await APSWCMasterSp(m);
            }
            catch (Exception ex)
            {

                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetApswcWareHouseMaster:Method:"+jsondata));
                return ex.Message.ToString();
            }
            
        }

       public  async Task<dynamic> GetBoardofDirectors()
        {
            try
            {
                MasterSp m = new MasterSp();
                m.DIRECTION_ID = "1";
                m.TYPEID = "BOARD_OF_DIRECTORS";
                return await APSWCMasterSp(m);
            }
            catch(Exception ex)
            {
                
                //string mappath = Server.MapPath("UpdateMailMobileFormLogs");
                string jsondata = JsonConvert.SerializeObject(ex.Message);
                Task WriteTask = Task.Factory.StartNew(() => Logfile.Write_Log(exPathToSave, "GetBoardofDirectors:Method:"+jsondata));
                return ex.Message.ToString();
            }
        }
        private async Task<DataTable>APSWCMasterSp(MasterSp objMa)
        {
            SqlConnection sqlcon = new SqlConnection(_connectionString);
            SqlCommand cmd = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("SP_MASTER_PROC", sqlcon);
                SqlDataAdapter adp = new SqlDataAdapter();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DIRECTION_ID", objMa.DIRECTION_ID);
                cmd.Parameters.AddWithValue("@TYPEID", objMa.TYPEID);
                cmd.Parameters.AddWithValue("@INPUT_01", objMa.INPUT_01);
                cmd.Parameters.AddWithValue("@INPUT_02", objMa.INPUT_02);
                cmd.Parameters.AddWithValue("@INPUT_03", objMa.INPUT_03);
                cmd.Parameters.AddWithValue("@INPUT_04", objMa.INPUT_04);
                cmd.Parameters.AddWithValue("@INPUT_05", objMa.INPUT_05);
                cmd.Parameters.AddWithValue("@INPUT_06", objMa.INPUT_06);
                cmd.Parameters.AddWithValue("@INPUT_07", objMa.INPUT_07);
                cmd.Parameters.AddWithValue("@INPUT_08", objMa.INPUT_08);
                cmd.Parameters.AddWithValue("@INPUT_09", objMa.INPUT_09);
                cmd.Parameters.AddWithValue("@INPUT_10", objMa.INPUT_10);
                cmd.Parameters.AddWithValue("@INPUT_11", objMa.INPUT_11);
                cmd.Parameters.AddWithValue("@INPUT_12", objMa.INPUT_12);
                cmd.Parameters.AddWithValue("@INPUT_13", objMa.INPUT_13);
                cmd.Parameters.AddWithValue("@INPUT_14", objMa.INPUT_14);
                cmd.Parameters.AddWithValue("@INPUT_15", objMa.INPUT_15);
                cmd.Parameters.AddWithValue("@INPUT_16", objMa.INPUT_16);
                cmd.Parameters.AddWithValue("@INPUT_17", objMa.INPUT_17);
                cmd.Parameters.AddWithValue("@INPUT_18", objMa.INPUT_18);
                cmd.Parameters.AddWithValue("@INPUT_19", objMa.INPUT_19);
                cmd.Parameters.AddWithValue("@INPUT_20", objMa.INPUT_20);
                cmd.Parameters.AddWithValue("@INPUT_21", objMa.INPUT_21);
                cmd.Parameters.AddWithValue("@INPUT_22", objMa.INPUT_22);
                cmd.Parameters.AddWithValue("@INPUT_23", objMa.INPUT_23);
                cmd.Parameters.AddWithValue("@INPUT_24", objMa.INPUT_24);
                cmd.Parameters.AddWithValue("@INPUT_25", objMa.INPUT_25);
                cmd.Parameters.AddWithValue("@INPUT_26", objMa.INPUT_26);
                cmd.Parameters.AddWithValue("@INPUT_27", objMa.INPUT_27);
                cmd.Parameters.AddWithValue("@INPUT_28", objMa.INPUT_28);
                cmd.Parameters.AddWithValue("@INPUT_29", objMa.INPUT_29);
                cmd.Parameters.AddWithValue("@INPUT_30", objMa.INPUT_30);
                cmd.Parameters.AddWithValue("@INPUT_31", objMa.INPUT_31);
                cmd.Parameters.AddWithValue("@INPUT_32", objMa.INPUT_32);
                cmd.Parameters.AddWithValue("@INPUT_33", objMa.INPUT_33);
                cmd.Parameters.AddWithValue("@INPUT_34", objMa.INPUT_34);
                cmd.Parameters.AddWithValue("@INPUT_35", objMa.INPUT_35);
                cmd.Parameters.AddWithValue("@USER_NAME", objMa.USER_NAME);
                cmd.Parameters.AddWithValue("@CALL_SOURCE", objMa.CALL_SOURCE);
                cmd.Parameters.AddWithValue("@CALL_PAGE_ACTIVITY", objMa.CALL_PAGE_ACTIVITY);
                cmd.Parameters.AddWithValue("@CALL_BRO_APP_VER", objMa.CALL_BRO_APP_VER);
                cmd.Parameters.AddWithValue("@CALL_MOBILE_MODEL", objMa.CALL_MOBILE_MODEL);
                cmd.Parameters.AddWithValue("@CALL_LATITUDE", objMa.CALL_LATITUDE);
                cmd.Parameters.AddWithValue("@CALL_LONGITUDE", objMa.CALL_LONGITUDE);
                cmd.Parameters.AddWithValue("@CALL_IP_IMEI", objMa.CALL_IP_IMEI);
                await sqlcon.OpenAsync();
                adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
                return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }

          }
    }
}
