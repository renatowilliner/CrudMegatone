using System;
using System.Collections.Generic;

namespace DesafioPractico.Models;

public partial class Producto
{
    public string CodigoProducto { get; set; } = null!;

    public string DescripcionProducto { get; set; } = null!;

    public decimal PrecioCosto { get; set; }

    public decimal PrecioVenta { get; set; }

    public int IdMarca { get; set; }

    public int IdFamilia { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public bool? Baja { get; set; }

    public DateTime? FechaBaja { get; set; }

    public virtual Familia IdFamiliaNavigation { get; set; } = null!;

    public virtual Marca IdMarcaNavigation { get; set; } = null!;
}
