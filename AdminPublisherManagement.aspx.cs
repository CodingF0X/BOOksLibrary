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
    public partial class AdminPublisherManagement : System.Web.UI.Page
    {
        string StrCon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        // Add Publisher Button
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkIfPublisherExists())
                Response.Write("<script>alert('Publisher Already Exists');</script>");

            else
                addPublisher();

        }

        // Update Publisher button
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (checkIfPublisherExists())
                updatePublisher();
            else
                Response.Write("<script>alert('Publisher doesnt exist');</script>");
        }

        // Delete Publisher Button
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkIfPublisherExists())
                deletePublisher();

            else
                Response.Write("<script>alert('Publisher doesnt exist');</script>");

        }

        // Go Button
        protected void Button1_Click(object sender, EventArgs e)
        {
            getauthorById();
        }


        // User Defined Methods

        bool checkIfPublisherExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM PublisherTBL WHERE Publisher_Id ='" + TextBox1.Text.Trim() + "'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        void addPublisher()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO PublisherTBL (Publisher_Id,Publisher_Name) Values(@PublisherID,@PublisherName)", con);

                cmd.Parameters.AddWithValue("@PublisherID", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@PublisherName", TextBox2.Text.Trim());

                cmd.ExecuteNonQuery();
                con.Close();
                GridView1.DataBind();
                clearBoxes();
                //Response.Write("<script>alert('Publisher Added Successfully');</script>");
            }
            catch
            {

            }
        }

        void updatePublisher()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE PublisherTBL SET Publisher_Name=@PublisherName WHERE Publisher_Id = '" + TextBox1.Text.Trim() + "'", con);
                cmd.Parameters.AddWithValue("@PublisherName", TextBox2.Text.Trim());

                cmd.ExecuteNonQuery();
                con.Close();
                GridView1.DataBind();
                Response.Write("<script>alert('Publisher Updated Successfully');</script>");
            }
            catch
            {

            }
        }

        void deletePublisher()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM PublisherTBL WHERE Publisher_ID = '" + TextBox1.Text.Trim() + "'", con);

                cmd.ExecuteNonQuery();
                con.Close();
                GridView1.DataBind();
                Response.Write("<script>alert('Publisher deleted Successfully');</script>");
                clearBoxes();
            }
            catch
            {

            }
        }

        void getauthorById()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("Select * From PublisherTBL Where Publisher_Id ='" + TextBox1.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    TextBox2.Text = dt.Rows[0][1].ToString();
                }

                else
                {
                    Response.Write("<script>alert('Invalid Publisher Id');</script>");
                }
            }
            catch
            {

            }
        }

        void clearBoxes()
        {
            TextBox1.Text = "";
            TextBox2.Text = "";
        }


    }
}