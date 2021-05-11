
namespace EstanzuelaEats.Common.Modelos
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Productos
    {

        #region Atributos

        [Key]
        public int ProductoId { get; set; }

        public int IdCategoria { get; set; }

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

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        [NotMapped]
        public byte[] ImageArray { get; set; }

        public bool Existencias { get; set; }

        public double Latitud { get; set; }

        public double Longitude { get; set; }

        #endregion



        #region Propiedades

        public string ImageFullPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ImagePath))
                {
                    return "NoProducto.png";
                }

                return $"https://estanzuelaeats.azurewebsites.net/{this.ImagePath.Substring(1)}";
            }
        }


        public override string ToString()
        {
            return this.NombreProducto;
        }

        #endregion


    }
}
