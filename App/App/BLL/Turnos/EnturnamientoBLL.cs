using App.Common;
using App.Entities.IT;
using App.Entities.Operaciones;
using App.Entities.Turnos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL.Turnos
{
    public class EnturnamientoBLL
    {
        public Task<List<Vehiculo>> ObtenerCabezotesTurnosPorUsuarioActual()
        {
            Task<List<Vehiculo>> vehiculos;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Enturnamiento/ObtenerCabezotesTurnosPorUsuario/";
            vehiculos = ds.RealizarPeticionApi<List<Vehiculo>>(url, DataService.TipoPeticionApi.Get);
            return vehiculos;
        }

        public Task<List<Enturnamiento>> ObtenerTurnosPorPlaca(string placa)
        {
            Task<List<Enturnamiento>> turnos;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Enturnamiento/ObtenerTurnosPorPlaca/" + placa;
            turnos = ds.RealizarPeticionApi<List<Enturnamiento>>(url, DataService.TipoPeticionApi.Get);
            return turnos;
        }

        public Task<List<Enturnamiento>> ObtenerTurnosPorUsuario()
        {
            Task<List<Enturnamiento>> turnos;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Enturnamiento/ObtenerTurnosPorUsuario/";
            turnos = ds.RealizarPeticionApi<List<Enturnamiento>>(url, DataService.TipoPeticionApi.Get);
            return turnos;
        }

        public Task<List<Proveedor>> ObtenerConductoresPorNombre(string nombre)
        {
            Task<List<Proveedor>> conductores;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Enturnamiento/ObtenerConductoresPorNombre/" + nombre;
            conductores = ds.RealizarPeticionApi<List<Proveedor>>(url, DataService.TipoPeticionApi.Get);
            return conductores;
        }

        public Task<Vehiculo> ObtenerVehiculo(string placa)
        {
            Task<Vehiculo> vehiculo;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Enturnamiento/ObtenerVehiculo/"+placa;
            vehiculo = ds.RealizarPeticionApi<Vehiculo>(url, DataService.TipoPeticionApi.Get);
            return vehiculo;
        }

        public Task<List<RespuestaServicio>> GuardarTurnos(List<Enturnamiento> registrosTurnos)
        {
            Task<List<RespuestaServicio>> respuestas;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Enturnamiento/GuardarTurnos";
            respuestas = ds.RealizarPeticionApi<List<RespuestaServicio>>(url, DataService.TipoPeticionApi.Post,registrosTurnos);            
            return respuestas;
        }

        public Task<List<RespuestaServicio>> EliminarTurno(long idTurno)
        {
            Task<List<RespuestaServicio>> respuestas;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Enturnamiento/BorrarTurnoSAP/"+idTurno;
            respuestas = ds.RealizarPeticionApi<List<RespuestaServicio>>(url, DataService.TipoPeticionApi.Delete);
            return respuestas;
        }

        public Task<List<Proveedor>> ObtenerConductoresTurnosPorUsuario()
        {
            Task<List<Proveedor>> conductores;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Enturnamiento/ObtenerConductoresTurnosPorUsuario";
            conductores = ds.RealizarPeticionApi<List<Proveedor>>(url, DataService.TipoPeticionApi.Get);
          
            return conductores;
        }

        public Task<List<Agencia>> ObtenerAgenciasEnturnamiento()
        {
            Task<List<Agencia>> agencias;
            DataService.AppDataService ds = new DataService.AppDataService(ParametrosSistema.TokenUsuarioActual);
            string url = "Enturnamiento/ObtenerAgenciasEnturnamiento";
            agencias = ds.RealizarPeticionApi<List<Agencia>>(url, DataService.TipoPeticionApi.Get);

            return agencias;
        }
    }
}
