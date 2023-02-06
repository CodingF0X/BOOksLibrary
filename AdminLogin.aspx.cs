using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        string StrCon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("Select * From AdminLoginTBL Where UserName = '" + TextBox1.Text.Trim() + "'" +
                    "AND Password = '"+TextBox2.Text.Trim()+"'", con);

                SqlDataReader dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Response.Write("<script>alert('YOU loged in');</script>");
                        Session["UserName"] = dr.GetValue(0).ToString();
                        Session["FullName"] = dr.GetValue(2).ToString();
                        Session["role"] = "admin";
                    }

                    Response.Redirect("Homepage.aspx");
                }

                else
                {
                    Response.Write("<script>alert('invalid credentials');</script>");
                }

            }
            catch
            {

            }
        }

        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}