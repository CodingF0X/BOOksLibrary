using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library
{
    public partial class AdminMemberManagement : System.Web.UI.Page
    {
        string StrCon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();
        }

        //Go Button
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            getMemberById();
        }

       
        //Activate Account
        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            UpdateMemberStatus("Active");
        }

        //Pending account
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            UpdateMemberStatus("Pending");
        }

        // DeActivate account
        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            UpdateMemberStatus("Deactive");
        }

        //Delete Button
        protected void Button2_Click(object sender, EventArgs e)
        {
            if(checkIfMemberExists())
            deleteMember();

            else
                Response.Write("<script>alert('Member doesnt exist');</script>");
        }


        // User Defined Methods

        void getMemberById()
        {
            try
            {

                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("Select * From MemberTBL Where Member_Id = '" + TextBox1.Text.Trim() + "'", con);


                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TextBox2.Text = dr.GetValue(0).ToString();
                        //TextBox7.Text = dr.GetValue(10).ToString();
                        TextBox8.Text = dr.GetValue(1).ToString();
                        TextBox3.Text = dr.GetValue(2).ToString();
                        TextBox4.Text = dr.GetValue(3).ToString();
                        TextBox9.Text = dr.GetValue(4).ToString();
                        TextBox10.Text = dr.GetValue(5).ToString();
                        TextBox11.Text = dr.GetValue(6).ToString();
                        TextBox6.Text = dr.GetValue(7).ToString();


                    }

              
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

        void UpdateMemberStatus(String Status)
        {
            if (checkIfMemberExists())
            {
                try
                {

                    SqlConnection con = new SqlConnection(StrCon);
                    if (con.State == System.Data.ConnectionState.Closed)
                        con.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE MemberTBL SET Account_Status='" + Status + "' WHERE Member_Id = '" + TextBox1.Text.Trim() + "'", con);


                    //SqlDataReader dr = cmd.ExecuteReader();
                    //if (dr.HasRows)
                    //{
                    //    while (dr.Read())
                    //    {
                    //        TextBox7.Text = dr.GetValue(10).ToString();
                    //    }
                    //}
                    cmd.ExecuteNonQuery();
                    con.Close();
                    GridView1.DataBind();
                }
                catch
                {

                }
            }
            else
                Response.Write("<script>alert('member doesnt exist');</script>");
        }

        void deleteMember()
        {
            if (TextBox1.Text.Trim().Equals(""))
            {
                Response.Write("<script>alert('ID field Cannot be empty');</script>");
            }
            else
            {

                try
                {
                    SqlConnection con = new SqlConnection(StrCon);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM MemberTBL WHERE Member_ID = '" + TextBox1.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    GridView1.DataBind();
                    clearBoxes();

                }
                catch
                {

                }
            }
        }

        void clearBoxes()
        {
            TextBox2.Text = "";
            //TextBox7.Text = "";
            TextBox8.Text = "";
            TextBox3.Text = "";
            TextBox4.Text = "";
            TextBox9.Text = "";
            TextBox10.Text = "";
            TextBox11.Text = "";
            TextBox6.Text = "";
        }

        bool checkIfMemberExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("Select * From MemberTBL Where Member_Id ='" + TextBox1.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }

                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}