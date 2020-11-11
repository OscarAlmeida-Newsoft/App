using App.DAO.Operaciones;
using App.Entities.Operaciones;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Operaciones
{
    public class AgenciaBLL
    {
        public List<Agencia> SeleccionarAgencias()
        {
            AgenciaDAO dao = new AgenciaDAO();
            return dao.SeleccionarAgencias();
        }
    }
}
