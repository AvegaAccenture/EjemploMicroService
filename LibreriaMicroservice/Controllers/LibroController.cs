using LibreriaMicroservice.Core.Entities;
using LibreriaMicroservice.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibreriaMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : Controller
    {
        private readonly IMongoRepository<LibroEntity> _LibroContext;

        public LibroController(IMongoRepository<LibroEntity> LibroContext)
        {
            _LibroContext = LibroContext;
        }

        [HttpPost]
        public async Task Post(LibroEntity libro)
        {
            await _LibroContext.InsertDocument(libro);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroEntity>>> Get()
        {
            return Ok(await _LibroContext.GetAll());
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<PaginationEntity<LibroEntity>>> PostPagination(PaginationEntity<LibroEntity> pagination)
        {
            var resultados = await _LibroContext.PaginationByFilter(pagination);
            return Ok(resultados);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string Id)
        {
            var Libro = await _LibroContext.GetById(Id);
            return Ok(Libro);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
