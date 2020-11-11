using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App.Entities.Operaciones
{
    public class TipoEventoLogistico : EntidadApp
    {

        
        public int CodigoEvento { get; set; }
        public string NombreEvento { get; set; }
        public string urlIcono { get; set; }
        
        public bool EventoTipoOtros { get; set; }
        public int? IdEstado { get; set; }
        public bool TransporteObligatorio { get; set; }
        public bool RegistrarOffLine { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }
        public bool AplicaTerceros { get; set; }
    }

    public enum TipoEventoLogisticoEnum
    {
        ActivarViaje = 1, Enganche, LlegadaCargue, IniciaCargue, FinCargue, IniciaViaje, ParadaCamino, ContinuacionViaje, Desenganche, FinViaje,
        IniciaTransitoVacio, OcurrenciaTDM, ReporteFallas, EntradaTaller, SalidaTaller, MontadaLlanta, SolicitudLavada, Reanticipo, InicioJornada,
        FinJornada, NivelTanqueCryogas, NovedadTanqueCryogas, SolicitudTanqueo, Otro, IniciaDescargue, FinDescargue, InventarioTrailer, Tanqueo,
        InicioCicloVariable, FinCicloVariable, LlegadaDestino, MantenimientoTrailer,Cartagena, Buenaventura,SuspenderViaje, ReanudarViaje, DevolucionContenedor,
        ReportarUbicacion=44
    }
}
