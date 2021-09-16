using LibreriaMicroservice.Core.Entities;
using LibreriaMicroservice.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaServicioController : ControllerBase
    {
        private readonly IMongoRepository<AutorEntity> _autorEntityRespository;
        private readonly IMongoRepository<LibroEntity> _libroEntityRespository;


        public LibreriaServicioController(IMongoRepository<AutorEntity> autorEntityRespository, IMongoRepository<LibroEntity> libroEntityRespository)
        {
            _autorEntityRespository = autorEntityRespository;
            _libroEntityRespository = libroEntityRespository;
        }

        [HttpGet("Autores")]
        public async Task<ActionResult<IEnumerable<AutorEntity>>> GetAutores()
        {
            var autores = await _autorEntityRespository.GetAll();
            return Ok(autores);
        }

        [HttpGet("Libros")]
        public async Task<ActionResult<IEnumerable<LibroEntity>>> GetLibros()
        {
            var libros = await _libroEntityRespository.GetAll();
            return Ok(libros);
        }

    }
}
