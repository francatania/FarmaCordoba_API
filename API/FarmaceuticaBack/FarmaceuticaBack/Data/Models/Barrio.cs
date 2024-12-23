﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FarmaceuticaBack.Models;

public partial class Barrio
{
    public int IdBarrio { get; set; }

    public string Barrio1 { get; set; }

    public int IdCiudad { get; set; }
    [JsonIgnore]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    [JsonIgnore]
    public virtual ICollection<EmpresaLogistica> EmpresaLogisticas { get; set; } = new List<EmpresaLogistica>();
    [JsonIgnore]
    public virtual ICollection<Establecimiento> Establecimientos { get; set; } = new List<Establecimiento>();

    public virtual Ciudad IdCiudadNavigation { get; set; }
    [JsonIgnore]
    public virtual ICollection<Personal> Personals { get; set; } = new List<Personal>();
    [JsonIgnore]
    public virtual ICollection<Proveedor> Proveedores { get; set; } = new List<Proveedor>();
}