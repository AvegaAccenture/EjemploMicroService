using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaMicroservice.Core.Entities
{
    [BsonCollection("Libro")]
    public class LibroEntity : Document
    {
        [BsonElement("Titulo")]
        public string Titulo { get; set; }
        [BsonElement("Descripcion")]
        public string Descripcion { get; set; }
        [BsonElement("Precio")]
        public int Precio { get; set; }
        [BsonElement("FechaPublicacion")]
        public DateTime? FechaPublicacion { get; set; }
        [BsonElement("Autor")]
        public AutorEntity Autor { get; set; }
    }
}
