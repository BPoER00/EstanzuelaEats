
namespace EstanzuelaEats.Common.Modelos
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Productos
    {
        [Key]
        public int ProductoId { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreProducto { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal PrecioProducto { get; set; }
        
        [StringLength(120)]
        [DataType(DataType.MultilineText)]
        public string DescripcionProducto { get; set; }

        [Display(Name = "Imagen")]
        public string ImagePath { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublicacionProducto { get; set; }

        public bool Existencias { get; set; }
        public override string ToString()
        {
            return this.NombreProducto;
        }
    }
}
