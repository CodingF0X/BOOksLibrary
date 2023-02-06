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
    public partial class UserSignUp : System.Web.UI.Page
    {
        string StrCon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            if(checkMemberExists())
            {
                Response.Write("<script>alert('user already exists');</script>");
            }
            else
            {
                SignUpNewMember();
            }

            
        }

        //User defined methods

        bool checkMemberExists()
        {
            try
            {
               
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }


                SqlCommand cmd = new SqlCommand
                    ("SELECT * FROM membertbl WHERE member_ID='"+TextBox8.Text.Trim()+"'; ", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                { return true; }

                else
                { return false; }

                con.Close();
                Response.Write("<script>alert('sign up successful');</script>");
            }
                          

            catch (Exception ex)
            {
                return false;
            }

           

        }




        void SignUpNewMember()
        {

            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }


                SqlCommand cmd = new SqlCommand
                    (" INSERT INTO MemberTBL(full_name,DOB,Contact_No,Email,State,City,PinCode,Full_Address,Member_Id,Password,Account_Status ) " +
                    "Values (@FullName,@dob,@ContactNO,@email,@state,@city,@PinCode,@Full_Address,@MemberID,@PWD,@AccountStatus)", con);

                cmd.Parameters.AddWithValue("@FullName", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@dob", TextBox2.Text.Trim());
                cmd.Parameters.AddWithValue("@ContactNO", TextBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@email", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@state", DropDownList1.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@city", TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@PinCode", TextBox7.Text.Trim());
                cmd.Parameters.AddWithValue("@Full_Address", TextBox5.Text.Trim());
                cmd.Parameters.AddWithValue("@MemberID", TextBox8.Text.Trim());
                cmd.Parameters.AddWithValue("@PWD", TextBox9.Text.Trim());
                cmd.Parameters.AddWithValue("@AccountStatus", "Pending");


                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('sign up successful');</script>");
                Response.Redirect("HomePage.aspx");
            }
            catch (Exception ex)
            {

            }
        }

        protected void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}