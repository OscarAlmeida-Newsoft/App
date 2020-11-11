using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Entities.Operaciones
{
    public class EventoLogistico : EntidadApp
    {
        
        public Int64 ID { get; set; }
        public int IDDispositivo { get; set; }
        public Int64 NumeroManifiesto { get; set; }
        public string NumeroDocumentoConductor { get; set; }
        public string NombreConductor { get; set; }
        public string Placa { get; set; }
        public int IdTipoEvento { get; set; }
        public string NombreTipoEvento { get; set; }
        public string Mensaje { get; set; }
        public DateTime? FechaEvento { get; set; }
        public string Observaciones { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }
        public string campo3 { get; set; }
        public string campo4 { get; set; }
        public string campo5 { get; set; }
        public string campo6 { get; set; }
        public string campo7 { get; set; }
        public string campo8 { get; set; }
        public string campo9 { get; set; }
        public string campo10 { get; set; }
        public string campo11 { get; set; }
        public string campo12 { get; set; }
        public string campo13 { get; set; }
        public string campo14 { get; set; }
        public string campo15 { get; set; }
        public string campo16 { get; set; }
        public string campo17 { get; set; }
        public string campo18 { get; set; }
        public string campo19 { get; set; }
        public string campo20 { get; set; }
        public string campo21 { get; set; }
        public string campo22 { get; set; }
        public string campo23 { get; set; }
        public string campo24 { get; set; }
        public string campo25 { get; set; }
        public string campo26 { get; set; }
        public string campo27 { get; set; }
        public string campo28 { get; set; }
        public string campo29 { get; set; }
        public string campo30 { get; set; }
        public string campo31 { get; set; }
        public string campo32 { get; set; }
        public string campo33 { get; set; }
        public string campo34 { get; set; }
        public string campo35 { get; set; }
        public string campo36 { get; set; }
        public string campo37 { get; set; }
        public string campo38 { get; set; }
        public string campo39 { get; set; }
        public string campo40 { get; set; }
        public string campo41 { get; set; }
        public string campo42 { get; set; }
        public string campo43 { get; set; }
        public string campo44 { get; set; }
        public string campo45 { get; set; }
        public string campo46 { get; set; }
        public string campo47 { get; set; }
        public string campo48 { get; set; }
        public string campo49 { get; set; }
        public string campo50 { get; set; }
        public string campo51 { get; set; }
        public string campo52 { get; set; }
        public string campo53 { get; set; }
        public string campo54 { get; set; }
        public string campo55 { get; set; }
        public string campo56 { get; set; }
        public string campo57 { get; set; }
        public string campo58 { get; set; }
        public string campo59 { get; set; }
        public string campo60 { get; set; }
        public string campo61 { get; set; }
        public string campo62 { get; set; }
        public string campo63 { get; set; }
        public string campo64 { get; set; }
        public string campo65 { get; set; }
        public string campo66 { get; set; }
        public string campo67 { get; set; }
        public string campo68 { get; set; }
        public string campo69 { get; set; }
        public string campo70 { get; set; }
        public string campo71 { get; set; }
        public string campo72 { get; set; }
        public string campo73 { get; set; }
        public string campo74 { get; set; }
        public string campo75 { get; set; }
        public string campo76 { get; set; }
        public string campo77 { get; set; }
        public string campo78 { get; set; }
        public string campo79 { get; set; }
        public string campo80 { get; set; }
        public string campo81 { get; set; }
        public string campo82 { get; set; }
        public string campo83 { get; set; }
        public string campo84 { get; set; }
        public string campo85 { get; set; }
        public string campo86 { get; set; }
        public string campo87 { get; set; }
        public string campo88 { get; set; }
        public string campo89 { get; set; }
        public string campo90 { get; set; }
        public string campo91 { get; set; }
        public string campo92 { get; set; }
        public string campo93 { get; set; }
        public string campo94 { get; set; }
        public string campo95 { get; set; }
        public string campo96 { get; set; }
        public string campo97 { get; set; }
        public string campo98 { get; set; }
        public string campo99 { get; set; }
        public string campo100 { get; set; }
        public float? Latitud { get; set; }
        public float? Longitud { get; set; }
        public string DescripcionPosicion { get; set; }

        public string Estado { get; set; }
        [Ignore]
        public List<AdjuntoEventoLogistico> Adjuntos { get; set; }

        
    }
}
