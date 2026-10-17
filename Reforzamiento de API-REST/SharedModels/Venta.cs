using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModels
{
    public class Venta
    {
        public int VentaID { get; set; }
        public DateTime Registro { get; set; }

        public ICollection <VentaDetalle> VentasDetalles { get; set; }
    }
}
