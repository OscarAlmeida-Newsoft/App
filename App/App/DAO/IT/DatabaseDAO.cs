using App.Entities.Comercial;
using App.Entities.IT;
using App.Entities.Operaciones;
using App.Entities.Turnos;
using App.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.DAO.IT
{
    class DatabaseDAO: AppDAOBase
    {
        /// <summary>
        /// Crea una base de datos sqlite con una tabla para cada tipo pasado como parámetro
        /// </summary>
        /// <param name="tipos"></param>
        public void CrearBaseDeDatos(List<Type> tipos)
        {
            LimipiarBaseDeDatos();
            using (var db = DependencyService.Get<ISQLite>().GetConnection())
            {
                foreach(Type t in tipos)
                {   
                    db.CreateTable(t);
                }
                //db.CreateTable<EventoLogistico>();
                //db.CreateTable<ConfiguracionApp>();
                //db.CreateTable<CampoEventoLogistico>();
                //db.CreateTable<ItemCampoEventoLogistico>();
                //db.CreateTable<JerarquiaTipoEventoLogistico>();
                //db.CreateTable<Entrega>();

            }
        }

        public void LimipiarBaseDeDatos()
        {
            using (var db = DependencyService.Get<ISQLite>().GetConnection())
            {   
                db.DropTable<TipoEventoLogistico>();
                db.DropTable<EventoLogistico>();
                db.DropTable<ConfiguracionApp>();
                db.DropTable<CampoEventoLogistico>();
                db.DropTable<ItemCampoEventoLogistico>();
                db.DropTable<SubItemCampoEventoLogistico>();
                db.DropTable<JerarquiaTipoEventoLogistico>();
                db.DropTable<HistorialActivacionManifiesto>();
                db.DropTable<Entrega>();
                db.DropTable<Agencia>();                
            }
        }

        public bool GuardarRegistros<T>(List<T> registros)
        {
            bool resultado = true;
            using (var db = DependencyService.Get<ISQLite>().GetConnection())
            {
                try
                {
                    foreach (T obj in registros)
                    {
                        db.Insert(obj);
                    }
                }
                catch(Exception ex)
                {
                    resultado = false;
                }
                
            }

            return resultado;

        }
    }
}
