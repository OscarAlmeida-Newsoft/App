
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Comercial
{
    public class DetalleEntrega
    {
        private Int32 _NumeroEntrega;
        public Int32 NumeroEntrega
        {
            get { return _NumeroEntrega; }
            set
            {
                _NumeroEntrega = value;

            }
        }
        private Int32 _NumeroPosicion;
        public Int32 NumeroPosicion
        {
            get { return _NumeroPosicion; }
            set
            {
                _NumeroPosicion = value;

            }
        }
        private String _TipoPosicion;
        public String TipoPosicion
        {
            get { return _TipoPosicion; }
            set
            {
                _TipoPosicion = value;

            }
        }
        private String _NumeroMaterial;
        public String NumeroMaterial
        {
            get { return _NumeroMaterial; }
            set
            {
                _NumeroMaterial = value;

            }
        }
        private String _EstadoContenedor;
        public String EstadoContenedor
        {
            get { return _EstadoContenedor; }
            set
            {
                _EstadoContenedor = value;

            }
        }
        private String _NumeroContenedor;
        public String NumeroContenedor
        {
            get { return _NumeroContenedor; }
            set
            {
                _NumeroContenedor = value;

            }
        }
        private String _CodigoEmbalaje;
        public String CodigoEmbalaje
        {
            get { return _CodigoEmbalaje; }
            set
            {
                _CodigoEmbalaje = value;

            }
        }
        private String _NombreEmbalaje;
        public String NombreEmbalaje
        {
            get { return _NombreEmbalaje; }
            set
            {
                _NombreEmbalaje = value;

            }
        }
        private String _GrupoArticulo;
        public String GrupoArticulo
        {
            get { return _GrupoArticulo; }
            set
            {
                _GrupoArticulo = value;

            }
        }
        private String _GrupoMateriales1;
        public String GrupoMateriales1
        {
            get { return _GrupoMateriales1; }
            set
            {
                _GrupoMateriales1 = value;

            }
        }
        private Double _Cantidad;
        public Double Cantidad
        {
            get { return _Cantidad; }
            set
            {
                _Cantidad = value;

            }
        }
        private Double _PesoBruto;
        public Double PesoBruto
        {
            get { return _PesoBruto; }
            set
            {
                _PesoBruto = value;

            }
        }
        private Double _PesoNeto;
        public Double PesoNeto
        {
            get { return _PesoNeto; }
            set
            {
                _PesoNeto = value;

            }
        }
        private String _UsuarioCreacion;
        public String UsuarioCreacion
        {
            get { return _UsuarioCreacion; }
            set
            {
                _UsuarioCreacion = value;

            }
        }
        private DateTime _FechaCreacion;
        public DateTime FechaCreacion
        {
            get { return _FechaCreacion; }
            set
            {
                _FechaCreacion = value;

            }
        }
        private String _Centro;
        public String Centro
        {
            get { return _Centro; }
            set
            {
                _Centro = value;

            }
        }
        private String _UnidadPeso;
        public String UnidadPeso
        {
            get { return _UnidadPeso; }
            set
            {
                _UnidadPeso = value;

            }
        }
        private Double _Volumen;
        public Double Volumen
        {
            get { return _Volumen; }
            set
            {
                _Volumen = value;

            }
        }
        private String _UnidadVolumen;
        public String UnidadVolumen
        {
            get { return _UnidadVolumen; }
            set
            {
                _UnidadVolumen = value;

            }
        }
        private String _Denominacion;
        public String Denominacion
        {
            get { return _Denominacion; }
            set
            {
                _Denominacion = value;

            }
        }
        private String _IndicadorPicking;
        public String IndicadorPicking
        {
            get { return _IndicadorPicking; }
            set
            {
                _IndicadorPicking = value;

            }
        }
        private String _NumeroAlmacen;
        public String NumeroAlmacen
        {
            get { return _NumeroAlmacen; }
            set
            {
                _NumeroAlmacen = value;

            }
        }
        private String _ClaseMovimiento;
        public String ClaseMovimiento
        {
            get { return _ClaseMovimiento; }
            set
            {
                _ClaseMovimiento = value;

            }
        }
        private String _GrupoMateriales2;
        public String GrupoMateriales2
        {
            get { return _GrupoMateriales2; }
            set
            {
                _GrupoMateriales2 = value;

            }
        }
        private String _GrupoMateriales3;
        public String GrupoMateriales3
        {
            get { return _GrupoMateriales3; }
            set
            {
                _GrupoMateriales3 = value;

            }
        }

        private String _Unidad;
        public String Unidad
        {
            get { return _Unidad; }
            set
            {
                _Unidad = value;

            }
        }

        //public List<CondicionComercial> CondicionesComerciales { get; set; }




        public bool EsVenta
        {
            get
            {
                return false;               
            }

        }

    }
}
