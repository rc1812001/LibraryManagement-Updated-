using LibraryManagement.Common;
using LibraryManagement.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;

namespace LibraryManagement.Web.DAL
{
    public class RegisterDataLayer
    {
        public string SignUpUser(UserModel model)
        {

            Password encryptPassword = new Password();
            //PasswordBase64 encryptPassword = new PasswordBase64();
            SqlConnection con = new SqlConnection("Data Source = (LocalDb)\\MSSQLLocalDB; Initial Catalog = ProjectDB; Integrated Security = True");
            try
            {
                SqlCommand cmd = new SqlCommand("proc_RegisterUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", model.USER_NAME);
                cmd.Parameters.AddWithValue("@FirstName", model.FIRST_NAME);
                cmd.Parameters.AddWithValue("@LastName", model.LAST_NAME);
                //   cmd.Parameters.AddWithValue("@Password", encryptPassword.EncryptPassword(model.Password)); //
                cmd.Parameters.AddWithValue("@Password", encryptPassword.EncryptPassword(model.PASSWORD));
                cmd.Parameters.AddWithValue("@Email", model.EMAIL_ID);
                cmd.Parameters.AddWithValue("@Mobile", model.MOBILE);
                cmd.Parameters.AddWithValue("@Gender", model.GENDER);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return ("Data save successfully");
            }
            catch (Exception ex)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return (ex.Message.ToString());
            }
        }
    }

}