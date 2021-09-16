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
    public class LibreriaAutorController : ControllerBase
    {
        private readonly IMongoRepository<AutorEntity> _autorGenerico;

        public LibreriaAutorController(IMongoRepository<AutorEntity> autorGenerico)
        {
            _autorGenerico = autorGenerico;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorEntity>>> GetAll()
        {
            return Ok(await _autorGenerico.GetAll());
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<AutorEntity>> GetById(string Id)
        {
            return Ok(await _autorGenerico.GetById(Id));
        }

        [HttpPost]
        public async Task Create(AutorEntity autor)
        {
            await _autorGenerico.InsertDocument(autor);
        }

        [HttpPut("{Id}")]
        public async Task Update(string Id, AutorEntity autor)
        {
            autor.Id = Id;
            await _autorGenerico.UpdateDocument(autor);
        }

        [HttpDelete("{Id}")]
        public async Task Delete(string Id)
        {
            await _autorGenerico.DeleteById(Id);
        }

        [HttpPost("Pagination")]
        public async Task<ActionResult<PaginationEntity<AutorEntity>>> PostPagination(PaginationEntity<AutorEntity> Pagination)
        {
            var Resultados = await _autorGenerico.PaginationByFilter(Pagination);

            return Ok(Resultados);
        }


        

    }
}
