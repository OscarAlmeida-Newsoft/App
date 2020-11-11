using App.Common;
using App.Entities.Almacenamiento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Operaciones
{
    public class AlmacenamientoBLL: AppBLLBase
    {
        public async Task<InventarioContenedor> GuardarInventarioContenedor(InventarioContenedor inventario)
        {
            if (await ParametrosSistema.isOnline)
            {
                DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
                inventario.Id = await ds.RealizarPeticionApi<long>("Almacenamiento/GuardarInventarioContenedor", DataService.TipoPeticionApi.Post, inventario);
                
            }
            return inventario;
        }
    }
}
