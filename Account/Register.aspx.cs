using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.UI;
using ASP.NET_Example;

using Segment;

public partial class Account_Register : Page
{
    protected void CreateUser_Click(object sender, EventArgs e)
    {
        var manager = new UserManager();
        var user = new ApplicationUser() { UserName = UserName.Text };
        IdentityResult result = manager.Create(user, Password.Text);
        if (result.Succeeded)
        {
            Analytics.Client.Identify(user.Id, new Segment.Model.Traits
            {
                { "name", user.UserName },
                { "email", user.Email }
            });

            Analytics.Client.Track(user.Id, "Signup");
            
            IdentityHelper.SignIn(manager, user, isPersistent: false);
            IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
        }
        else
        {
            ErrorMessage.Text = result.Errors.FirstOrDefault();
        }
    }
}