using App.Entities.Comercial;
using App.Entities.Operaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class Transporte
    {
        public long NumeroTransporte { get; set; }
        public string ClaseTransporte { get; set; }
        //public string UsuarioCreo { get; set; }
        //public DateTime? FechaCreacion { get; set; }
        //public string UsuarioModifico { get; set; }
        //public DateTime? FechaModificacion { get; set; }
        public string CodigoRuta { get; set; }
        public string NombreRuta { get; set; }
        //public string CodigoRutaMinisterio { get; set; }
        public string Placa { get; set; }
        public string PlacaTrailer { get; set; }
        //public string PropiedadTrailer { get; set; }
        //public string TipoDocTenedorTrailer { get; set; }
        //public string NumeroDocTenedorTrailer { get; set; }
        //public string NombreTenedorTrailer { get; set; }
        //public string Apellido1TenedorTrailer { get; set; }
        //public string Apellido2TenedorTrailer { get; set; }
        //public string DireccionTenedorTrailer { get; set; }
        //public string TelefonoTenedorTrailer { get; set; }
        //public string CodigoCiudadTenedorTrailer { get; set; }
        //public string CodigoDeptoTenedorTrailer { get; set; }
        //public string NombreCiudadTenedorTrailer { get; set; }
        //public string NombreDepartamentoTenedorTrailer { get; set; }
        //public string MarcaTrailer { get; set; }
        //public string ModeloTrailer { get; set; }
        //public int EjesTrailer { get; set; }
        //public int PesoVacioTrailer { get; set; }
        //public Int64 NumeroDocTenedor { get; set; }
        public Int32 NumeroDocConductor { get; set; }
        //public double FletePagado { get; set; }
        //public List<PosicionTransporte> Posiciones { get; set; }
        //public string CodigoCiudadOrigen { get; set; }
        //public string CodigoCiudadDestino { get; set; }
        public string DescripcionCiudadOrigen { get; set; }
        public string DescripcionCiudadDestino { get; set; }
        //public double ValorRuta { get; set; }
        public string NombreConductor { get; set; }
        //public string NombreTenedor { get; set; }
        //public string ConfiguracionVehicular { get; set; }
        //public string TipoVehiculo { get; set; }
        //public string TipoTrailer { get; set; }
        //public string Programa { get; set; }
        //public string CategoriaConductor { get; set; }
        //public string CentroCostos { get; set; }
        //public string GestionEspecial { get; set; }
        //public float PDM { get; set; }
        //public float PorcentajePDM { get; set; }
        [SQLite.Ignore]
        public List<Entrega> Entregas { get; set; }
        //public string PuestoExpedicion { get; set; }
        //public string NombrePuestoExpedicion { get; set; }
        //public double FleteCobrado { get; set; }
        //public string GrupoTrailer { get; set; }
        //public float KilimetrosRuta { get; set; }
        //public decimal TiempoRuta { get; set; }
        public string CelularConductor { get; set; }
        //public DateTime? FechaActivacion { get; set; }
        //public string UsuarioActivacion { get; set; }
        //public int CodigoMotivoActivacionManual { get; set; }
        //public List<PuestoControlManifiesto> PuestosControl { get; set; }
        //public DateTime FechaEstimadaSalida { get; set; }
        //public DateTime? FechaLlegadaCiudadDestino { get; set; }
        //public string CodigoMarcaVehiculo { get; set; }
        ////[DataMember()]
        //public string NombreMarcaVehiculo { get; set; }
        ////[DataMember()]
        //public string CodigoLineaVehiculo { get; set; }
        ////[DataMember()]
        //public string NombreLineaVehiculo { get; set; }
        ////[DataMember()]
        //public string ModeloVehiculo { get; set; }
        ////[DataMember()]
        //public string ModeloRepotenciadoVehiculo { get; set; }
        ////[DataMember()]
        //public string ChasisVehiculo { get; set; }
        ////[DataMember()]
        //public string NombreColorVehiculo { get; set; }
        ////[DataMember()]
        //public string CodigoColorVehiculo { get; set; }
        ////[DataMember()]
        //public string CodigoTipoCarroceria { get; set; }
        ////[DataMember()]
        //public string NombreTipoCarroceria { get; set; }
        ////[DataMember()]
        //public decimal RegistroNacionalCarga { get; set; }
        ////[DataMember()]
        //public string ConfiguracionVehiculo { get; set; }
        ////[DataMember()]

        ////[DataMember()]
        //public int PesoVacioVehiculo { get; set; }
        ////[DataMember()]
        //public string NumeroSeguroObligatorio { get; set; }
        ////[DataMember()]
        //public DateTime FechaVencimientoSeguroObligatorio { get; set; }
        ////[DataMember()]
        //public DateTime FechaVencimientoGases { get; set; }

        ////[DataMember()]
        //public string TipoDocPropietario { get; set; }
        ////[DataMember()]
        //public string NumeroDocPropietario { get; set; }
        ////[DataMember()]
        //public string NombrePropietario { get; set; }
        ////[DataMember()]
        //public string Apellido1Propietario { get; set; }
        ////[DataMember()]
        //public string Apellido2Propietario { get; set; }
        ////[DataMember()]
        //public string DireccionPropietario { get; set; }
        ////[DataMember()]
        //public string TelefonoPropietario { get; set; }
        ////[DataMember()]
        //public string CodigoCiudadPropietario { get; set; }
        ////[DataMember()]
        //public string NombreCiudadPropietario { get; set; }
        //public string NombreDepartamentoPropietario { get; set; }
        ////[DataMember()]
        //public string TipoDocTenedor { get; set; }
        ////[DataMember()]
        //public string Apellido1Tenedor { get; set; }
        ////[DataMember()]
        //public string Apellido2Tenedor { get; set; }
        ////[DataMember()]
        //public string DireccionTenedor { get; set; }
        ////[DataMember()]
        //public string TelefonoTenedor { get; set; }
        ////[DataMember()]
        //public string CodigoCiudadTenedor { get; set; }
        ////[DataMember()]
        //public string NombreCiudadTenedor { get; set; }
        //public string NombreDepartamentoTenedor { get; set; }
        ////[DataMember()]
        //public string TipoDocConductor { get; set; }
        ////[DataMember()]
        //public string Apellido1Conductor { get; set; }
        ////[DataMember()]
        //public string Apellido2Conductor { get; set; }
        ////[DataMember()]
        //public string Clientes { get; set; }
        //public string DireccionConductor { get; set; }
        ////[DataMember()]
        //public string TelefonoFijoConductor { get; set; }
        ////[DataMember()]
        //public string TelefonoCelularConductor { get; set; }
        ////[DataMember()]
        //public string CodigoCiudadConductor { get; set; }
        ////[DataMember()]
        //public string NombreCiudadConductor { get; set; }
        //public string NombreDepartamentoConductor { get; set; }
        ////[DataMember()]
        //public string CategoriaLicenciaConductor { get; set; }
        //public string NumeroLicenciaConductor { get; set; }
        //public DateTime FechaVencimientoLicencia { get; set; }
        //public decimal ValorViaje { get; set; }
        //public decimal ValorGirado { get; set; }
        //public decimal RetencionFuente { get; set; }
        //public decimal ValorNeto { get; set; }
        //public decimal ValorAnticipo { get; set; }
        //public decimal ValorPrimaAlimentacion { get; set; }
        //public string CodigoAgenciaTurno { get; set; }
        //public string NombreAgenciaTurno { get; set; }
        //public string NombreAgenciaExpedicion { get; set; }
        //public bool ReportarseEnPuestosControl { get; set; }
        //public bool Encaravanado { get; set; }
        //public bool PermisoCargaExtradimensional { get; set; }
        //public bool TransitoSeisSeis { get; set; }
        //public bool ControlEscoltaOrigenDestino { get; set; }
        //public bool ControlEscoltaDestino { get; set; }
        //public bool ControlEscoltaOrigen { get; set; }
        //public bool ReporteSinCosto { get; set; }
        //public bool ReporteOficina { get; set; }
        //public bool ManifiestoOriginal { get; set; }
        //public bool RemesasFirmadas { get; set; }
        //public bool Remisiones { get; set; }
        //public bool Facturas { get; set; }
        ////[DataMember()]
        //public bool DTA { get; set; }
        //public bool Comodato { get; set; }
        //public bool NotaInspeccion { get; set; }
        //public bool Correspondencia { get; set; }
        //public bool PlanillaCargueDescargue { get; set; }
        //public bool ValeInstrucciones { get; set; }
        //public bool OrdenCargue { get; set; }
        //public bool Otros { get; set; }
        //public int NumeroManifiestoMinisterio { get; set; }
        //public string Observaciones { get; set; }

        //public string EstadoManifiesto { get; set; }
        //private string _estado;
        ////[DataMember()]
        //public string Estado
        //{
        //    get
        //    {
        //        return _estado;
        //    }
        //    set
        //    {
        //        _estado = value;
        //        if (value != null)
        //        {
        //            switch (value.ToLower())
        //            {
        //                case "pa":
        //                    this.PrioridadEstado = 10;
        //                    break;
        //                case "de":
        //                    this.PrioridadEstado = 50;
        //                    break;
        //                case "ve":
        //                    this.PrioridadEstado = 40;
        //                    break;
        //                case "am":
        //                    this.PrioridadEstado = 30;
        //                    break;
        //                case "ro":
        //                    this.PrioridadEstado = 20;
        //                    break;
        //                default:
        //                    this.PrioridadEstado = 60;
        //                    break;
        //            }
        //        }

        //    }
        //}
        ////[DataMember()]
        //public int PrioridadEstado { get; set; }
        ////[DataMember()]
        //public string UltimoComentario { get; set; }
        ////[DataMember()]
        //public DateTime? FechaVencimientoDTA { get; set; }
        ////[DataMember()]
        //public bool ManifiestoCerrado { get; set; }
        //public DateTime? FechaCierre { get; set; }


        ///*Relaciones con otras entidades */
        ////[DataMember()]
        //public PuestoControlManifiesto SiguientePuesto { get; set; }
        ////[DataMember()]

        ////[DataMember()]

        ////[DataMember()]
        //public List<ComentarioManifiesto> Comentarios { get; set; }
        ////[DataMember()]


        //public float Latitud { get; set; }
        //public float Longitud { get; set; }
        //public bool Encendido { get; set; }
        //public string Escoltado { get; set; }
        //public string Sellos { get; set; }

        //public bool Activar { get; set; }
        //public bool Reportar { get; set; }


        //public bool Inactivar { get; set; }
        //public string ImgPropiedad { get; set; }
        //public bool VerLogoGPS { get; set; }
        //public string ProveedorGPS { get; set; }
        //public string ImgGPS { get; set; }

        //public bool? CreacionNotificada { get; set; }
        //public bool? AnulacionNotificada { get; set; }

        //public string NombreDepartamentoOrigen { get; set; }
        //public string NombreDepartamentoDestino { get; set; }
        //public string TipoManifiesto { get; set; }
        //public string NombrePrograma { get; set; }
        //public string NumeroMotor { get; set; }
        //public DateTime? FechaPagoSaldo { get; set; }
        //public string NumeroDocAseguradora { get; set; }
        //public bool EstadoImpreso { get; set; }


    }
}
