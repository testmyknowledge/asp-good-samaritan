﻿using GoodSamaritan.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Web.Security;

namespace GoodSamaritan.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserManageController : Controller
    {

        RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        // GET: UserManage
        public ActionResult Index()
        {
            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();

            List<String> usernames = new List<String>();
            List<String> userid = new List<String>();

            List<String> userroles = new List<String>();
            List<String> userrolesids = new List<String>();

            foreach (var user in users)
            {
                userid.Add(user.Id);
                usernames.Add(user.UserName);
            }

            foreach (var role in roles)
            {
                userroles.Add(role.Name);
                userrolesids.Add(role.Id);
            }

            ViewBag.Id = userid;
            ViewBag.Users = usernames;
            ViewBag.Roles = userroles;
            ViewBag.RoleIds = userrolesids;

            return View();
        }

        // GET: UserManage/Details/5
        public ActionResult UserDetails(string id)
        {
            ViewBag.User = userManager.FindById(id).UserName;
            var roles = userManager.FindById(id).Roles.ToList();

            List<String> userRoles = new List<String>();
            List<String> userRolesNames = new List<String>();
            foreach (var role in roles)
            {

                userRoles.Add(role.RoleId);
                userRolesNames.Add(roleManager.FindById(role.RoleId).Name);
            }

            var allRoles = roleManager.Roles.ToList();


            List<String> allRolesList = new List<String>();
            List<String> allRolesId = new List<String>();
            foreach (var role in allRoles)
            {
                allRolesList.Add(role.Name);
                allRolesId.Add(role.Id);
            }


            ViewBag.RolesId = userRoles;
            ViewBag.RolesNames = userRolesNames;

            ViewBag.AllRoles = allRolesList;
            ViewBag.AllRolesIds = allRolesId;

            ViewBag.UserId = id;
            return View();
        }

        public ActionResult RoleDetails(string id)
        {
            ViewBag.Roles = roleManager.FindById(id);
            ViewBag.Users = roleManager.FindById(id).Users.ToList();
            return View();
        }

        public ActionResult RemoveUser(string id)
        {
            List<IdentityUserRole> roles = userManager.FindById(id).Roles.ToList();
            bool isFinalAdmin = false;

            //foreach of the roles this user belongs to
            foreach (IdentityUserRole role in roles)
            {
                //if this role is an administrator role
                if (roleManager.FindById(role.RoleId).Name.Equals("Administrator"))
                {
                    //if there is only 1 user with the admin role then we can not proceed
                    //because this user must be the user with that role
                    if (roleManager.FindByName("Administrator").Users.Count() == 1)
                    {
                        ViewBag.RemoveUserMessage = "There must be at least 1 Admin user. This user can not be removed";
                        isFinalAdmin = true;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            //if this is not the final admin user, we can delete this user
            if (!isFinalAdmin)
            {
                userManager.Delete(userManager.FindById(id));
                ViewBag.RemoveUserMessage = "The User Was Successfuly Removed";
            }

            /* Load Index Page */

            var users = userManager.Users.ToList();
            var rolesList = roleManager.Roles.ToList();

            List<String> usernames = new List<String>();
            List<String> userid = new List<String>();

            List<String> userroles = new List<String>();
            List<String> userrolesids = new List<String>();

            foreach (var user in users)
            {
                userid.Add(user.Id);
                usernames.Add(user.UserName);
            }

            foreach (var role in rolesList)
            {
                userroles.Add(role.Name);
                userrolesids.Add(role.Id);
            }

            ViewBag.Id = userid;
            ViewBag.Users = usernames;
            ViewBag.Roles = userroles;
            ViewBag.RoleIds = userrolesids;

            return View("~/Views/UserManage/Index.cshtml");




        }

        public ActionResult RemoveFromRole(string id, string role)
        {
            // If this is the Administrator role being removed, need to check there is still at least
            //one user with the Administrator role
            string name = roleManager.FindById(role).Name;
            if (name.Equals("Administrator"))
            {
                int numOfAdminUsers = roleManager.FindById(role).Users.Count();

                if (numOfAdminUsers > 1)
                {
                    userManager.RemoveFromRole(id, roleManager.FindById(role).Name);
                    ViewBag.message = "User Removed From Role";
                }
                else
                {
                    ViewBag.message = "There must be at least 1 Admin user. This user can not be removed from the Administrator role";
                }
            }
            else
            {
                userManager.RemoveFromRole(id, roleManager.FindById(role).Name);
                ViewBag.message = "User Removed From Role";
            }


            /* Setup Logic For User Detail Page */

            ViewBag.User = userManager.FindById(id).UserName;
            var roles = userManager.FindById(id).Roles.ToList();

            List<String> userRoles = new List<String>();
            List<String> userRolesNames = new List<String>();
            foreach (var urole in roles)
            {

                userRoles.Add(urole.RoleId);
                userRolesNames.Add(roleManager.FindById(urole.RoleId).Name);
            }

            var allRoles = roleManager.Roles.ToList();
            List<String> allRolesList = new List<String>();
            List<String> allRolesId = new List<String>();
            foreach (var urole in allRoles)
            {
                allRolesList.Add(urole.Name);
                allRolesId.Add(urole.Id);
            }


            ViewBag.RolesId = userRoles;
            ViewBag.RolesNames = userRolesNames;

            ViewBag.AllRoles = allRolesList;
            ViewBag.AllRolesIds = allRolesId;

            ViewBag.UserId = id;
            return View("~/Views/UserManage/UserDetails.cshtml");
        }

        [HttpPost]
        public ActionResult AddToRole(FormCollection formCollection)
        {

            string[] keys = formCollection.AllKeys;

            foreach (string key in formCollection.Keys)
            {
                if (key.Equals("userid"))
                {
                    continue;
                }
                string state = formCollection[key];

                if (state.Equals("true,false"))
                {
                    Debug.WriteLine(key + "Is being added as a role to the user");
                    userManager.AddToRole(formCollection["userid"], roleManager.FindByName(key).Name);
                }


            }


            ViewBag.AddRoleMessage = "User Added To Role";

            string id = formCollection["userid"];

            ViewBag.User = userManager.FindById(id).UserName;
            var roles = userManager.FindById(id).Roles.ToList();

            List<String> userRoles = new List<String>();
            List<String> userRolesNames = new List<String>();
            foreach (var urole in roles)
            {

                userRoles.Add(urole.RoleId);
                userRolesNames.Add(roleManager.FindById(urole.RoleId).Name);
            }

            var allRoles = roleManager.Roles.ToList();
            List<String> allRolesList = new List<String>();
            List<String> allRolesId = new List<String>();
            foreach (var urole in allRoles)
            {
                allRolesList.Add(urole.Name);
                allRolesId.Add(urole.Id);
            }


            ViewBag.RolesId = userRoles;
            ViewBag.RolesNames = userRolesNames;

            ViewBag.AllRoles = allRolesList;
            ViewBag.AllRolesIds = allRolesId;

            ViewBag.UserId = id;
            return View("~/Views/UserManage/UserDetails.cshtml");
        }

        public ActionResult RemoveRole(string id)
        {
            List<ApplicationUser> userList = userManager.Users.ToList();

            foreach (ApplicationUser user in userList)
            {
                userManager.RemoveFromRole(user.Id, roleManager.FindById(id).Name);
            }

            roleManager.Delete(roleManager.FindById(id));

            ViewBag.RemoveRoleMessage = "Role Successfuly Removed";

            /* LOAD INDEX PAGE */

            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();

            List<String> usernames = new List<String>();
            List<String> userid = new List<String>();

            List<String> userroles = new List<String>();
            List<String> userrolesids = new List<String>();

            foreach (var user in users)
            {
                userid.Add(user.Id);
                usernames.Add(user.UserName);
            }

            foreach (var role in roles)
            {
                userroles.Add(role.Name);
                userrolesids.Add(role.Id);
            }

            ViewBag.Id = userid;
            ViewBag.Users = usernames;
            ViewBag.Roles = userroles;
            ViewBag.RoleIds = userrolesids;

            return View("~/Views/UserManage/Index.cshtml");
            //return RedirectToAction("Index");
        }

        public ActionResult ToggleSuspend(string id)
        {
            //logic for suspending or unsuspending a user

            if (!userManager.FindById(id).LockoutEnabled)
            {
                userManager.SetLockoutEnabled(id, true);

                ViewBag.ToggleSuspendMessage = "User Has Been Successfuly Suspended";
            }
            else
            {
                userManager.SetLockoutEnabled(id, false);
                ViewBag.ToggleSuspendMessage = "User Has Been Successfuly UnSuspended";
            }

            var users = userManager.Users.ToList();
            var rolesList = roleManager.Roles.ToList();

            List<String> usernames = new List<String>();
            List<String> userid = new List<String>();

            List<String> userroles = new List<String>();
            List<String> userrolesids = new List<String>();

            foreach (var user in users)
            {
                userid.Add(user.Id);
                usernames.Add(user.UserName);
            }

            foreach (var role in rolesList)
            {
                userroles.Add(role.Name);
                userrolesids.Add(role.Id);
            }

            ViewBag.Id = userid;
            ViewBag.Users = usernames;
            ViewBag.Roles = userroles;
            ViewBag.RoleIds = userrolesids;


            return View("~/Views/UserManage/Index.cshtml");
        }

        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection)
        {
            string roleName = formCollection["rolename"];

            if (roleName.Equals("Role Name") || roleName.Length < 2 || roleName.Contains("  ") || roleName.First().Equals(' '))
            {
                ViewBag.CreateRoleMessage = "Invalid Role Entered";
                return View("~/Views/UserManage/CreateRole.cshtml");
            }

            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
                ViewBag.CreateRoleMessage = "Role Successfuly Created";
            }
            else
            {
                ViewBag.CreateRoleMessage = "This Role Already Exists and Has Not Been Created";
            }

            return View("~/Views/UserManage/CreateRole.cshtml");
        }

        public ActionResult EditRole(string id)
        {
            string roleName = roleManager.FindById(id).Name;

            ViewBag.RoleName = roleName;
            ViewBag.RoleId = id;

            return View();
        }

        [HttpPost]
        public ActionResult EditRole(FormCollection formCollection)
        {
            string oldRoleId = formCollection["oldroleid"];
            string newRoleName = formCollection["rolename"];

            if (newRoleName.Equals("Role Name") || newRoleName.Length < 2 || newRoleName.Contains("  ") || newRoleName.First().Equals(' '))
            {
                ViewBag.EditRoleMessage = "Invalid Role Entered";
                return View("~/Views/UserManage/CreateRole.cshtml");
            }

            IdentityRole oldRole = roleManager.FindById(oldRoleId);

            oldRole.Name = newRoleName;

            roleManager.Update(oldRole);

            ViewBag.EditRoleMessage = "Role Successfuly Updated";

            return View();
        }

    }
}
