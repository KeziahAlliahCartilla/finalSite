using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Webdev_project.Models;

namespace Webdev_project.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AdminPage()
        {
            int userId = (int)Session["UserId"];

            using (var dbContext = new ActivityDBEntities())
            {
                var user = dbContext.Users.FirstOrDefault(a => a.user_Id == userId);

                if (user != null)
                {


                    return View(user);
                }
            }

            return RedirectToAction("LoginFailed");
        }



        public ActionResult UserList()
        {
            int userId = (int)Session["UserId"];

            using (var dbContext = new ActivityDBEntities())
            {
                var userList = dbContext.Users
                    .Where(a => a.user_Id != userId)
                    .Include(u => u.Role)
                    .ToList();

                return View(userList);
            }
        }


        public ActionResult ProfilePage()
        {
            int userId = (int)Session["UserId"];

            using (var dbContext = new ActivityDBEntities())
            {
                var user = dbContext.Users.Include("Role").FirstOrDefault(a => a.user_Id == userId);

                if (user != null)
                {
                    return View(user);
                }
            }

            return RedirectToAction("LoginFailed");
        }



        public ActionResult LoginFailed()
        {
            return View();
        }



        [HttpPost]
        public ActionResult AddUserToDatabase(FormCollection fc)
        {
            String firstName = fc["firstname"];
            String lastName = fc["lastname"];
            String email = fc["email"];
            String address = fc["address"];
            String password = fc["password"];
            String confirmPassword = fc["confirm-password"];

            //var redirectPage = "";
        
            if (password == confirmPassword)
            {
                User use = new User();
                use.firstname = firstName;
                use.lastname = lastName;
                use.email = email;
                use.address = address;
                use.password = password;
                use.role_Id = 1;

                ActivityDBEntities fe = new ActivityDBEntities();
                fe.Users.Add(use);
                fe.SaveChanges();

                return RedirectToAction("ProfilePage");

            }


            return RedirectToAction("LoginFailed");
        }


        [HttpPost]
        public ActionResult LoginAndRedirect(FormCollection gd)
        {
            string email = gd["email"];
            string password = gd["password"];

            using (var dbContext = new ActivityDBEntities())
            {
                var user = dbContext.Users.FirstOrDefault(a => a.email == email && a.password == password);

                if (user != null)
                {
                    // Store user ID in session
                    Session["UserId"] = user.user_Id;
                    Session["Role"] = user.role_Id;

                    if (Convert.ToInt32(Session["Role"]) == 1)
                    {
                        return RedirectToAction("AdminPage");
                    }
                    else if (Convert.ToInt32(Session["Role"]) == 2)
                    {
                        return RedirectToAction("ProfilePage");
                    }

                    TempData["SuccessMessage"] = "Update Successfully!";

                }
            }

            return RedirectToAction("LoginFailed");
        }

        [HttpPost]
        public ActionResult UpdateUser(FormCollection fc)
        {
            int userId = (int)Session["UserId"];

            using (var dbContext = new ActivityDBEntities())
            {
                var user = dbContext.Users.FirstOrDefault(a => a.user_Id == userId);

                if (user != null)
                {
                    string oldPassword = fc["old-password"];
                    string newPassword = fc["new_password"];

                    if (oldPassword == user.password && (newPassword != "" || newPassword != null))
                    {
                        user.firstname = fc["firstname"];
                        user.lastname = fc["lastname"];
                        user.email = fc["email"];
                        user.address = fc["address"];

                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            // Update the password if a new password is provided
                            user.password = newPassword;
                        }

                        dbContext.SaveChanges();
                        TempData["SuccessMessage"] = "Update Successfully!";

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid old password!";
                        return RedirectToAction("ProfilePage");
                    }
                }
            }

            return RedirectToAction("ProfilePage");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            // Show a flag indicating that the confirmation prompt should be displayed
            TempData["ShowLogoutConfirmation"] = true;

            return RedirectToAction("AdminPage");
        }

        public ActionResult LogoutConfirmed()
        {
            // Clear session variables
            Session.Clear();

            // Abandon the session
            Session.Abandon();

            return RedirectToAction("Index");
        }


        public ActionResult DeleteUser(int userId)
        {
            using (var dbContext = new ActivityDBEntities())
            {
                var user = dbContext.Users.FirstOrDefault(a => a.user_Id == userId);

                if (user != null)
                {
                    dbContext.Users.Remove(user);
                    dbContext.SaveChanges();
                }
            }

            return RedirectToAction("UserList");
        }



        [HttpPost]
        public ActionResult EditUser(int userId)
        {
            using (var dbContext = new ActivityDBEntities())
            {
                var user = dbContext.Users.FirstOrDefault(a => a.user_Id == userId);

                if (user != null)
                {
                    // Pass the user object to the EditUser view for editing
                    return View("UpdateUserLists", user);
                }
            }

            return RedirectToAction("UserList");
        }

        [HttpPost]
        public ActionResult UpdateUserLists(User model)
        {
            using (var dbContext = new ActivityDBEntities())
            {
                var user = dbContext.Users.FirstOrDefault(a => a.user_Id == model.user_Id);

                if (user != null)
                {
                    string oldPassword = Request.Form["old-password"];
                    string newPassword = Request.Form["new_password"];

                    if (oldPassword == user.password && (newPassword != "" || newPassword != null))
                    {
                        user.firstname = model.firstname;
                        user.lastname = model.lastname;
                        user.email = model.email;
                        user.address = model.address;

                        if (!string.IsNullOrEmpty(newPassword))
                        {
                            // Update the password if a new password is provided
                            user.password = newPassword;
                        }

                        dbContext.SaveChanges();
                        TempData["SuccessMessage"] = "Update Successfully!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid old password!";
                    }
                }
            }

            return RedirectToAction("UserList");
        }



    }
}