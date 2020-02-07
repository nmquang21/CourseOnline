using CourseOnline.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CourseOnline.Service
{
    public class MemberService
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool CheckExpiredMember(ApplicationUser user)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            if (user == null)
            {
                return true;
            }
            var listRole = UserManager.GetRoles(user.Id);
            var listMemberType = db.Members.ToList();
            foreach (var member in listMemberType)
            {
                if (listRole.Contains(member.MemberType))
                {
                    var listMemberShip = db.Memberships.ToList();
                    var listUserMemberShip = db.Memberships.Where(m => m.ApplicationUser.Id == user.Id && m.MemberId == member.Id).ToList();
                    if (listUserMemberShip == null)
                    {
                        return true;
                    }

                    var createdAt = listUserMemberShip[0].CreatedAt;
                    if (createdAt.AddMonths(listUserMemberShip[0].Member.ExpiredMonths) < DateTime.Now)
                    {
                        //Xoa memberShip neu het han:
                        db.Memberships.Remove(listUserMemberShip[0]);
                        db.SaveChanges();
                        Debug.WriteLine("Da xoa memberShip");
                        //Xoa role neu het han:
                        UserManager.RemoveFromRole(user.Id, member.MemberType);
                        Debug.WriteLine("Da xoa role");
                        return true;
                    }
                    if (createdAt.AddMonths(listUserMemberShip[0].Member.ExpiredMonths) > DateTime.Now)
                    {
                        return false;
                    }

                }
            }
            return true;
        }
    }
}