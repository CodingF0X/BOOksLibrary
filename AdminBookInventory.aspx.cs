using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library
{
    public partial class AdminBookInventory : System.Web.UI.Page
    {
        string StrCon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        static string global_FilePath;
        static int global_Actual_Stock, Global_Current_Stock, Global_Issued_Books;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            getAuthor_PublisherValues();

            GridView1.DataBind();
        }

        // Go button
        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            getBookDetails();
        }
        //Add Button
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (checkIfBookExists())
            {
                Response.Write("<script>alert('Book already exists');</script>");

            }
            else
            {

                addNewBook();
            }
        }

        //Update button
        protected void Button3_Click(object sender, EventArgs e)
        {
            updateBookDetails();
        }

        //Delete Button
        protected void Button2_Click(object sender, EventArgs e)
        {
            deleteBook();
        }



        // User Defined Functions

        void getAuthor_PublisherValues()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT Author_Name FROM AuthorTBL", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DropDownList3.DataSource = dt;
                DropDownList3.DataValueField = "Author_Name";
                DropDownList3.DataBind();




                cmd = new SqlCommand("SELECT Publisher_Name FROM PublisherTBL", con);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                DropDownList2.DataSource = dt;
                DropDownList2.DataValueField = "Publisher_Name";
                DropDownList2.DataBind();



            }
            catch
            {

            }
        }

        bool checkIfBookExists()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("SELECT * FROM BookTBL WHERE Book_Id='"+TextBox1.Text.Trim()+"' OR Book_Name='"+TextBox2.Text.Trim()+"'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                //if (string.IsNullOrEmpty(da.))
                //    return false;
                //else
                //    return true;

                if (dt.Rows.Count >= 1)
                {
                    return true;
                }
                else
                { return false; }
            }
            catch
            {
                return false;
            }
        }

        void getBookDetails()
        {
            try
            {
                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("Select * From BookTBL Where Book_Id ='" + TextBox1.Text.Trim() + "';", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                da.Fill(dt);

                if (dt.Rows.Count >= 1)
                {
                    TextBox2.Text = dt.Rows[0]["Book_Name"].ToString();
                    TextBox3.Text = dt.Rows[0]["Publish_Date"].ToString();
                    TextBox9.Text = dt.Rows[0]["Edition"].ToString();
                    TextBox10.Text = dt.Rows[0]["Book_cost"].ToString();
                    TextBox11.Text = dt.Rows[0]["No_Of_Pages"].ToString();
                    TextBox4.Text = dt.Rows[0]["Actual_Stock"].ToString().Trim();


                    TextBox5.Text = dt.Rows[0]["Current_Stock"].ToString();
                    TextBox6.Text = dt.Rows[0]["Book_Description"].ToString();
                    TextBox7.Text =  "" +(Convert.ToInt32( dt.Rows[0]["Actual_Stock"].ToString())- Convert.ToInt32(dt.Rows[0]["Current_Stock"].ToString()));


                    DropDownList1.SelectedValue = dt.Rows[0]["Language"].ToString().Trim();
                    DropDownList2.SelectedValue = dt.Rows[0]["Publisher_Name"].ToString().Trim();
                    DropDownList3.SelectedValue = dt.Rows[0]["Author_Name"].ToString().Trim();



                    global_Actual_Stock = Convert.ToInt32(dt.Rows[0]["Actual_Stock"].ToString().Trim());
                    Global_Current_Stock = Convert.ToInt32(dt.Rows[0]["Current_Stock"].ToString());
                    Global_Issued_Books = global_Actual_Stock - Global_Current_Stock;
                    global_FilePath = dt.Rows[0]["Book_IMG_Link"].ToString();



                }

                else
                {
                    Response.Write("<script>alert('There's no book with such ID');</script>");
                }
            }
            catch
            {

            }
        }

        void updateBookDetails()
        {
            if (checkIfBookExists())
            {
                string genres = "";
                foreach (int i in ListBox1.GetSelectedIndices())
                {
                    genres = genres + ListBox1.Items[i] + ",";
                }
                // genres = Adventure,Self Help,
                genres = genres.Remove(genres.Length - 1);



                string filepath = "~/Book_Cover/books1";
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                if (filename == "" || filename == null)
                {
                    filepath = global_FilePath;

                }
                else
                {
                    FileUpload1.SaveAs(Server.MapPath("Book_Cover/" + filename));
                    filepath = "~/Book_Cover/" + filename;
                }




                //Checking for books stock in the library
                int actual_Stock = Convert.ToInt32(TextBox4.Text.Trim());
                int current_Stock = Convert.ToInt32(TextBox5.Text.Trim());

                if (global_Actual_Stock == actual_Stock)
                {

                }
                else
                {
                    if (actual_Stock < Global_Issued_Books)
                    {  

                    Response.Write("<script>alert('you cant have stock less than issued books');</script>");
                    return;

                    }
                    else
                    {
                        current_Stock = actual_Stock - Global_Issued_Books;
                        TextBox5.Text = "" + current_Stock;
                    }
                }


                try
                {

                    SqlConnection con = new SqlConnection(StrCon);
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    SqlCommand cmd = new SqlCommand("UPDATE BookTBL SET book_name=@book_name, genre=@genre, author_name=@author_name, publisher_name=@publisher_name, publish_date=@publish_date, language=@language, edition=@edition, book_cost=@book_cost, no_of_pages=@no_of_pages, book_description=@book_description, actual_stock=@actual_stock, current_stock=@current_stock, book_img_link=@book_img_link where book_id='" + TextBox1.Text.Trim() + "'", con);

                    cmd.Parameters.AddWithValue("@book_name", TextBox2.Text.Trim());
                    cmd.Parameters.AddWithValue("@genre", genres);
                    cmd.Parameters.AddWithValue("@author_name", DropDownList3.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@publisher_name", DropDownList2.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@publish_date", TextBox3.Text.Trim());
                    cmd.Parameters.AddWithValue("@language", DropDownList1.SelectedItem.Value);
                    cmd.Parameters.AddWithValue("@edition", TextBox9.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_cost", TextBox10.Text.Trim());
                    cmd.Parameters.AddWithValue("@no_of_pages", TextBox11.Text.Trim());
                    cmd.Parameters.AddWithValue("@book_description", TextBox6.Text.Trim());
                    cmd.Parameters.AddWithValue("@actual_stock", actual_Stock.ToString());
                    cmd.Parameters.AddWithValue("@current_stock", current_Stock.ToString());
                    cmd.Parameters.AddWithValue("@book_img_link", filepath);


                    cmd.ExecuteNonQuery();
                    con.Close();
                    GridView1.DataBind();
                    Response.Write("<script>alert('Book Updated Successfully');</script>");


                }
                catch
                {

                }
            }
            else
                Response.Write("<script>alert('book doesnt exist');</script>");
        }

        void deleteBook()
        {
            if (checkIfBookExists())
            {
                try
                {

                    SqlConnection con = new SqlConnection(StrCon);
                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    SqlCommand cmd = new SqlCommand("DELETE FROM BookTBL WHERE Book_ID = '" + TextBox1.Text.Trim() + "'", con);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    GridView1.DataBind();
                    //clearBoxes();

                }
                catch
                {

                }
            }
            else
                Response.Write("<script>alert('Book doesnt exist');</script>");
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //void addNewBook()
        //{

        //    try
        //    {
        //        //selecting MULTIPLE Items from the listBOx
        //        string genres = "";
        //        foreach (int i in ListBox1.GetSelectedIndices())
        //        {
        //            genres = genres + ListBox1.Items[i] + ",";
        //        }

        //        genres = genres.Remove(genres.Length - 1);



        //        //Storing books covers

        //        string filepath = "~/Book_Cover/books1.png";
        //        string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
        //        FileUpload1.SaveAs(Server.MapPath("Book_Cover/" + filename));
        //        filepath = "~/Book_Cover/" + filename;




        //        SqlConnection con = new SqlConnection(StrCon);
        //        if (con.State == System.Data.ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }

        //        SqlCommand cmd = new SqlCommand("INSERT INTO BookTBL(Book_id,Book_name,Genre,Author_Name,Publisher_Name,Publish_date,Language,edition,Book_Cost,No_Of_Pages,Book_Description,Actual_Stock,Current_Stock,Book_img_link)" +
        //            "Values (@book_id,@book_name,@Genre,@Author_Name,@Publisher_Name,@Publish_date,@Language,@edition,@Book_Cost,@No_Of_Pages,@Book_Description,@Actual_Stock,@Current_Stock,@Book_img_link) ", con);

        //        cmd.Parameters.AddWithValue("@book_Id", TextBox1.Text.Trim());
        //        cmd.Parameters.AddWithValue("@book_Name", TextBox2.Text.Trim());

        //        cmd.Parameters.AddWithValue("@Genre", genres);
        //        cmd.Parameters.AddWithValue("@Author_Name", DropDownList3.SelectedItem.Value);
        //        cmd.Parameters.AddWithValue("@Publisher_Name", DropDownList2.SelectedItem.Value);
        //        cmd.Parameters.AddWithValue("@Publish_date", TextBox3.Text.Trim());
        //        cmd.Parameters.AddWithValue("@Language", DropDownList1.SelectedItem.Value);
        //        cmd.Parameters.AddWithValue("@edition", TextBox9.Text.Trim());
        //        cmd.Parameters.AddWithValue("@Book_Cost", TextBox10.Text.Trim());
        //        cmd.Parameters.AddWithValue("@No_Of_Pages", TextBox11.Text.Trim());
        //        cmd.Parameters.AddWithValue("@Book_Description", TextBox6.Text.Trim());
        //        cmd.Parameters.AddWithValue("@Actual_Stock", TextBox4.Text.Trim());
        //        cmd.Parameters.AddWithValue("@Current_Stock", TextBox4.Text.Trim());
        //        cmd.Parameters.AddWithValue("@Book_img_link", filepath);


        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //        Response.Write("<script>alert('book added');</script>");
        //        GridView1.DataBind();


        //    }
        //    catch
        //    {
        //        Response.Write("<script>alert('error');</script>");
        //    }




        //}










        void addNewBook()
        {
            try
            {
                string genres = "";
                foreach (int i in ListBox1.GetSelectedIndices())
                {
                    genres = genres + ListBox1.Items[i] + ",";
                }
                // genres = Adventure,Self Help,
                genres = genres.Remove(genres.Length - 1);

                string filepath = "~/Book_Cover/books1.png";
                string filename = Path.GetFileName(FileUpload1.PostedFile.FileName);
                FileUpload1.SaveAs(Server.MapPath("Book_Cover/" + filename));
                filepath = "~/Book_Cover/" + filename;

                SqlConnection con = new SqlConnection(StrCon);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO BookTBL(Book_id,Book_name,Genre,Author_Name,Publisher_Name,Publish_date,Language,edition,Book_Cost,No_Of_Pages,Book_Description,Actual_Stock,Current_Stock,Book_img_link)" +
                           "Values (@book_id,@book_name,@Genre,@Author_Name,@Publisher_Name,@Publish_date,@Language,@edition,@Book_Cost,@No_Of_Pages,@Book_Description,@Actual_Stock,@Current_Stock,@Book_img_link) ", con);
                //cmd.Parameters.AddWithValue("@book_id", TextBox1.Text.Trim());
                //cmd.Parameters.AddWithValue("@book_name", TextBox2.Text.Trim());
                //cmd.Parameters.AddWithValue("@genre", genres);
                //cmd.Parameters.AddWithValue("@author_name", DropDownList3.SelectedItem.Value);
                //cmd.Parameters.AddWithValue("@publisher_name", DropDownList2.SelectedItem.Value);
                //cmd.Parameters.AddWithValue("@publish_date", TextBox3.Text.Trim());
                //cmd.Parameters.AddWithValue("@language", DropDownList1.SelectedItem.Value);
                //cmd.Parameters.AddWithValue("@edition", TextBox9.Text.Trim());
                //cmd.Parameters.AddWithValue("@book_cost", TextBox10.Text.Trim());
                //cmd.Parameters.AddWithValue("@no_of_pages", TextBox11.Text.Trim());
                //cmd.Parameters.AddWithValue("@book_description", TextBox6.Text.Trim());
                //cmd.Parameters.AddWithValue("@actual_stock", TextBox4.Text.Trim());
                //cmd.Parameters.AddWithValue("@current_stock", TextBox4.Text.Trim());
                //cmd.Parameters.AddWithValue("@book_img_link", filepath);

                cmd.Parameters.AddWithValue("@book_Id", TextBox1.Text.Trim());
                cmd.Parameters.AddWithValue("@book_Name", TextBox2.Text.Trim());

                cmd.Parameters.AddWithValue("@Genre", genres);
                cmd.Parameters.AddWithValue("@Author_Name", DropDownList3.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@Publisher_Name", DropDownList2.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@Publish_date", TextBox3.Text.Trim());
                cmd.Parameters.AddWithValue("@Language", DropDownList1.SelectedItem.Value);
                cmd.Parameters.AddWithValue("@edition", TextBox9.Text.Trim());
                cmd.Parameters.AddWithValue("@Book_Cost", TextBox10.Text.Trim());
                cmd.Parameters.AddWithValue("@No_Of_Pages", TextBox11.Text.Trim());
                cmd.Parameters.AddWithValue("@Book_Description", TextBox6.Text.Trim());
                cmd.Parameters.AddWithValue("@Actual_Stock", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@Current_Stock", TextBox4.Text.Trim());
                cmd.Parameters.AddWithValue("@Book_img_link", filepath);



                cmd.ExecuteNonQuery();
                con.Close();
                Response.Write("<script>alert('Book added successfully.');</script>");
                GridView1.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}