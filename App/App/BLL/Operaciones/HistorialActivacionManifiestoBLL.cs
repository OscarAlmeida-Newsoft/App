using App.DAO.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Operaciones
{
    public class HistorialActivacionManifiestoBLL
    {
        public bool EliminarHistorialActivacionManifiesto(long numeroManifiesto)
        {
            HistorialActivacionManifiestoDAO DAO = new HistorialActivacionManifiestoDAO();
            return DAO.EliminarHistorialActivacionManifiesto(numeroManifiesto);
        }
    }
}
