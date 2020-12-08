using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using WebApplication2.Models;

namespace WebApplication2.ADO
{
    public class DAL
    {

        public string GetConStringFromJson()
        {
            string constring = string.Empty;
            try
            {
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.json");
                   // $"//wwwroot\\{"\\appsettings.json"}");
                var JSON = System.IO.File.ReadAllText(folderDetails);
                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(JSON);
                constring = jsonObj["ConnectionString"];
                //constring = Configuration["ConnectionString"];
            }
            catch(Exception ex)
            {
                
            }
            return constring;
        }

        public JWTUserModel GetLoggedUserDetail(string userid)
        {
            DataSet ds = new DataSet(); JWTUserModel userModel = new JWTUserModel();
            try
            {
                string constring = GetConStringFromJson();
                using (SqlConnection con = new SqlConnection(constring))
                {
                    string SqlQuery =
                "SELECT U.userid,R.role FROM JWTUsers U with (nolock) inner join JWTUsers_Roles R with(nolock) ON U.userid = R.userid where U.userid = '"
                        + userid + "'";

                    SqlDataAdapter da = new SqlDataAdapter(SqlQuery, con);
                    da.Fill(ds);

                }
                if (ds.Tables.Count > 0)
                {
                    userModel.userid = ds.Tables[0].Rows[0][0].ToString();
                    userModel.role = ds.Tables[0].Rows[0][1].ToString();
                }
            }
            catch(Exception ex)
            {

            }
            return userModel;
        }
       

        public JWTUserModel AuthenticateUser(string userid,string password)
        {
            DataSet ds = new DataSet(); JWTUserModel userModel = new JWTUserModel();
            try
            {
                string constring = GetConStringFromJson();
                using (SqlConnection con = new SqlConnection(constring))
                {
                    string SqlQuery = "SELECT userid,email,phoneno,created_date  FROM JWTUsers with(nolock) where userid= '" + userid+"' and password='"+password+"'";

                    SqlDataAdapter da = new SqlDataAdapter(SqlQuery, con);
                    da.Fill(ds);

                }
                if (ds.Tables.Count > 0)
                {
                    userModel.userid = ds.Tables[0].Rows[0][0].ToString();
                    userModel.email = ds.Tables[0].Rows[0][1].ToString();
                    userModel.phoneno = ds.Tables[0].Rows[0][2].ToString();
                    userModel.created_date = (DateTime)ds.Tables[3].Rows[0][1];
                }
            }
            catch (Exception ex)
            {

            }
            return userModel;
        }
    }
}
