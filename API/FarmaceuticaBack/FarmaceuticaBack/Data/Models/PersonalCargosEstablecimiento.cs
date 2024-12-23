﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FarmaceuticaBack.Models;

public partial class PersonalCargosEstablecimiento
{
    public int IdPersonalCargosEstablecimientos { get; set; }

    public int IdPersonal { get; set; }

    public int IdCargo { get; set; }

    public int IdEstablecimiento { get; set; }
    [JsonIgnore]
    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Cargo IdCargoNavigation { get; set; }

    public virtual Establecimiento IdEstablecimientoNavigation { get; set; }

    public virtual Personal IdPersonalNavigation { get; set; }

    [JsonIgnore]
    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}