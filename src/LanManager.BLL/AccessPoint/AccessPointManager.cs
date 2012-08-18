using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanManager.BLL
{
    public class AccessPointManager : BaseManager
    {
        public AccessPointManager()
        {
        }

        public AccessPointManager(LanManagerEntities contexto)
            : base(contexto)
        {
        }

        public AccessPoint GetAccessPointById(Guid id)
        {
            try
            {
                var accessPoint = (from ap in db.AccessPoint
                                   where ap.Id == id
                                   select ap).FirstOrDefault();

                if(accessPoint == null)
                {
                    AccessPoint ap = new AccessPoint();
                    ap.Id = id;
                    ap.Name = System.Environment.MachineName;
                    db.AddToAccessPoint(ap);
                    db.SaveChanges();
                    return ap;
                }

                return accessPoint;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ResourceSet.Resources.InvalidAccessPointId, ex);
            }
        }
    }
}