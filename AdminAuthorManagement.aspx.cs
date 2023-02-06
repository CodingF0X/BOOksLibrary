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
    public partial class AdminAuthorManagement : System.Web.UI.Page
    {
        string StrCon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataBind();

        }
        // Add Author Button
        protected void Button2_Click(object sender, EventArgs e)
        {
            if (checkIfAuthorExists())
            {
                Response.Write("<script>alert('Author already exists');</script>");
            }

            else
            {
                addNewAuthor();
            }
        }

        // update author button
        protected void Button3_Click(object sender, EventArgs e)
        {
            if (checkIfAuthorExists())
                updateauthor();
           
            else
                Response.Write("<script>alert('Author doesnt exist');</script>");
        }

        // delete author button
        protected void Button4_Click(object sender, EventArgs e)
        {
            if (checkIfAuthorExists())
                deleteAuthor();
         
            else
                Response.Write("<script>alert('Author doesnt exist');</script>");
            

        }

        //Go button
        protected void Button1_Click(object sender, EventArgs e)
        {
            getauthorById();
        }

        // User Defined functions

        bool checkIfAuthorExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("Select * From AuthorTBL Where Author_Id ='" + TextBox1.Text.Trim() + "';", con);
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

        void addNewAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO AuthorTBL (Author_Id,Author_Name) Values (@AuthorID,@AuthorName)", con);
                cmd.Parameters.AddWithValue("@AuthorID", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@AuthorName", TextBox2.Text.Trim());

                cmd.ExecuteNonQuery();
                con.Close();
                GridView1.DataBind();
                Response.Write("<script>alert('Author Added Successfully');</script>");

            }
            catch
            {

            }
        }

        void updateauthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("UPDATE AuthorTBL SET Author_Name=@AuthorName where Author_Id= '" + TextBox1.Text.Trim() + "'", con);
                cmd.Parameters.AddWithValue("AuthorName", TextBox2.Text.Trim());

                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Author Updated Successfully');</script>");
                clearBoxes();
                GridView1.DataBind();


            }
            catch
            {
               
            }
        }

        void deleteAuthor()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == ConnectionState.Closed)
                    con.Open();

                SqlCommand cmd = new SqlCommand("DELETE FROM AuthorTBL WHERE Author_Id ='" + TextBox1.Text.Trim() + "'", con);

                cmd.ExecuteNonQuery();
                con.Close();
                GridView1.DataBind();

                Response.Write("<script>alert('Author deleted succesfully');</script>");
                clearBoxes();
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

        void getauthorById()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("Select * From AuthorTBL Where Author_Id ='" + TextBox1.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    TextBox2.Text = dt.Rows[0][1].ToString();
                }

                else
                {
                    Response.Write("<script>alert('Invalid author Id');</script>");
                }
            }
            catch
            {
             
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}