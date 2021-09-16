using LibreriaMicroservice.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaMicroservice.Core.DbContext
{
    public interface IAutorContext
    {
        IMongoCollection<Autor> Autores { get; }
    }
}
