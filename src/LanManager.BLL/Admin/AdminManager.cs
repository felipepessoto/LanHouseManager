using System;
using System.Collections.Generic;
using System.Linq;

namespace LanManager.BLL.Admin
{
    public class AdminManager : BaseManager
    {
        public DAL.Admin LogOnAdmin(string userName, string password, string[] includeRelationships)
        {
            IQueryable<LanManager.DAL.Admin> query = IncluirRelacionamentos(db.Admin, includeRelationships);

            var admin = (from adm in query
                               where adm.UserName == userName && adm.Password == password && adm.Active
                               select adm).FirstOrDefault();

            return admin;
        }

        public void CreateAdmin(LanManager.DAL.Admin newAdmin)
        {
            db.AddToAdmin(newAdmin);
            db.SaveChanges();
        }

        public LanManager.DAL.Admin SearchByUsername(string userName, string[] includeRelationships)
        {
            IQueryable<LanManager.DAL.Admin> query = IncluirRelacionamentos(db.Admin, includeRelationships);

            var adminExists = (from adm in query
                               where adm.UserName == userName
                               select adm).FirstOrDefault();

            return adminExists;
        }

        public LanManager.DAL.Admin SearchById(Guid idAdmin, string[] includeRelationships)
        {
            IQueryable<LanManager.DAL.Admin> query = IncluirRelacionamentos(db.Admin, includeRelationships);

            var adminExists = (from adm in query
                               where adm.Id == idAdmin
                               select adm).FirstOrDefault();

            return adminExists;
        }

        public List<DAL.Admin> SearchAdmins(string keyword, string[] includeRelationships)
        {
            IQueryable<DAL.Admin> query = IncluirRelacionamentos(db.Admin, includeRelationships);

            var clients = (from cl in query
                           where string.IsNullOrEmpty(keyword) || cl.UserName.Contains(keyword) || cl.FullName.Contains(keyword)
                           select cl).ToList();

            return clients;
        }

        public void EditAdmin(Guid id, string password, string fullName, bool active)
        {
            int activeAdmins = (from actAdm in db.Admin
                                where actAdm.Active && actAdm.Id != id
                                select actAdm).Count();

            if(activeAdmins == 0 && !active)
            {
                throw new UniqueAdminActiveException("Você deve ter pelo menos um admin ativo");
            }

            DAL.Admin editingAdmin = SearchById(id, null);
            if (password.Length > 0)
                editingAdmin.Password = password;
            editingAdmin.FullName = fullName;
            editingAdmin.Active = active;

            db.SaveChanges();
        }
    }
}