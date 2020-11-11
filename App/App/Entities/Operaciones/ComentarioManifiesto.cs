using System;
using System.Collections.Generic;
namespace CoreTDM.Entities.Operaciones
{
	public class ComentarioManifiesto
	{

		public Int64 NumeroManifiesto{ get; set; }
		public int CodigoComentario{ get; set; }
		public string Comentario{ get; set; }
		public DateTime Fecha{ get; set; }
		public string Usuario{ get; set; }
        public short CodigoTipoComentario { get; set; }

        public List<AdjuntoComentarioManifiesto> Adjuntos { get; set; }

		public ComentarioManifiesto()
		{
		}
	}


}
