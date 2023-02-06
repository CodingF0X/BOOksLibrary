using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Library
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           string  sess = Session["role"] as string;
            try
            {
                if (string.IsNullOrEmpty((string)sess))
                {
                    LinkButton1.Visible = true; // User Login button
                    LinkButton2.Visible = true; // Sign Up Button
                    LinkButton3.Visible = false; // logout button
                    LinkButton7.Visible = false; // user profile button
                    LinkButton6.Visible = true; // Admin login button
                    LinkButton11.Visible = false; // author management button
                    LinkButton12.Visible = false; // publisgher management button

                    LinkButton8.Visible = false; // book inventorybutton
                    LinkButton9.Visible = false; // book issuing button
                    LinkButton10.Visible = false; // member management button

                }

                else if (sess.Equals("user"))
                {
                    LinkButton1.Visible = false; // User Login button
                    LinkButton2.Visible = false; // Sign Up Button
                    LinkButton3.Visible = true; // logout button
                    LinkButton7.Text ="Hello  " + Session["UserName"].ToString(); // user profile button
                    LinkButton6.Visible = true; // Admin login button

                    LinkButton11.Visible = false; // author management button
                    LinkButton12.Visible = false; // publisgher management button

                    LinkButton8.Visible = false; // book inventorybutton
                    LinkButton9.Visible = false; // book issuing button
                    LinkButton10.Visible = false; // member management button
                }

                else if (sess.Equals("admin"))
                {
                    LinkButton1.Visible = false; // User Login button
                    LinkButton2.Visible = false; // Sign Up Button
                    LinkButton3.Visible = true; // logout button
                    LinkButton7.Text = "Hello ADMIN "; //  profile button
                    LinkButton6.Visible = false; // Admin login button

                    LinkButton11.Visible = true; // author management button
                    LinkButton12.Visible = true; // publisgher management button

                    LinkButton8.Visible = true; // book inventorybutton
                    LinkButton9.Visible = true; // book issuing button
                    LinkButton10.Visible = true; // member management button
                } 
            }

            catch(Exception ex)
            {

            }
        }


        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminLogin.aspx");
        }

        protected void LinkButton11_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminAuthorManagement.aspx");
        }

        protected void LinkButton12_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPublisherManagement.aspx");
        }

        protected void LinkButton8_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminBookInventory.aspx");
        }

        protected void LinkButton9_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminIssuingPage.aspx");
        }

        protected void LinkButton10_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminMemberManagement.aspx");
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewBooks.aspx"); 
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserLogin.aspx");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserSignUP.aspx");
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Session["UserName"] = "";
            Session["FullName"] = "";
            Session["role"] = "";




            LinkButton1.Visible = true; // User Login button
            LinkButton2.Visible = true; // Sign Up Button
            LinkButton3.Visible = false; // logout button
            LinkButton7.Visible = false; // user profile button
            LinkButton6.Visible = true; // Admin login button
            LinkButton11.Visible = false; // author management button
            LinkButton12.Visible = false; // publisgher management button

            LinkButton8.Visible = false; // book inventorybutton
            LinkButton9.Visible = false; // book issuing button
            LinkButton10.Visible = false; // member management button

            Response.Redirect("HomePage.aspx");
        }
    }
}