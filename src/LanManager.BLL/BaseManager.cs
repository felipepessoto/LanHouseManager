using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanManager.DAL;

namespace LanManager.BLL
{
    public class BaseManager : IDisposable
    {
        public LanManagerEntities db;
        private bool contextoCriado;

        public BaseManager()
            : this(new LanManagerEntities())
        {
            contextoCriado = true;
        }

        public BaseManager(LanManagerEntities contexto)
        {
            db = contexto;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        protected System.Data.Objects.ObjectQuery<T> IncluirRelacionamentos<T>(System.Data.Objects.ObjectQuery<T> tabela, string[] relacionamentosIncluidos)
        {
            if (relacionamentosIncluidos != null)
            {
                foreach (var item in relacionamentosIncluidos)
                {
                    tabela = tabela.Include(item);
                }
            }

            return tabela;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (contextoCriado)
                {
                    db.Dispose();
                }
            }
        }

        ~BaseManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
