﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace FarmaceuticaBack.Models;

public partial class FacturasTiposPago
{
    public int IdFacturaTipoPago { get; set; }

    public int IdFactura { get; set; }

    public int? IdTipoPago { get; set; }

    public decimal? PorcentajePago { get; set; }

    public bool? EsCuotas { get; set; }

    public int? CantidadCuotas { get; set; }

    public virtual Factura IdFacturaNavigation { get; set; }

    public virtual TiposPago IdTipoPagoNavigation { get; set; }
}