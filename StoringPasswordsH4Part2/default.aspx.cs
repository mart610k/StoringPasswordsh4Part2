using StoringPasswordsH4Part2.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StoringPasswordsH4Part2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        PasswordService passwordService = new PasswordService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.Cookies["CurrentAttempts"] == null)
            {

                Response.Cookies.Add(new HttpCookie("CurrentAttempts", "0"));
            }
        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {
            int currentAttempts = 0;

            if (this.Request.Cookies["CurrentAttempts"] != null)
            {
                try
                {
                    currentAttempts = int.Parse(Request.Cookies["CurrentAttempts"].Value);
                    Response.Cookies.Add(new HttpCookie("CurrentAttempts", (currentAttempts + 1).ToString()));
                }
                catch 
                {
                    Thread.Sleep(5000);
                    currentAttempts = 5;

                }
            }

                if (passwordService.CheckUserCredentials(UserName.Text, Password.Text))
            {
                ShowPopUpMsg("Hello "+ UserName.Text + "");
                currentAttempts = 0;
            }
            else
            {
                ShowPopUpMsg("You have tried logging in, wrong username or password," + currentAttempts);
                currentAttempts++;
            }
            
            if(currentAttempts >= 5)
            {
                Thread.Sleep(5000);
                currentAttempts = 0;
                Response.Cookies.Add(new HttpCookie("CurrentAttempts", "0"));
            }

        }

        
        private void ShowPopUpMsg(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("alert('");
            sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
            sb.Append("');");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
        }
    }
}