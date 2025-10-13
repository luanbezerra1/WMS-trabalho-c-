/**
 * Autor: Cauã Tobias
 * Data de Criação: 13/10/2025
 * Descrição: Controller responsável pelo gerenciamento de armazéns na aplicação WMS 
 Expõe endpoints REST para operações de CRUD.
**/

using Microsoft.AspNetCore.Mvc;
using Wms.Models;

namespace Wms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ControleArmazem : ControllerBase
    {
        private readonly AppDataContext _context;

        public ControleArmazem(AppDataContext context)
        {
            _context = context;
        }

        // POST: api/controlearmazem
        /*  Recebe os dados do armazém no corpo da requisição (JSON). */
        [HttpPost]
        public IActionResult CriarArmazem([FromBody] Armazem novoArmazem)
        {
            if (novoArmazem == null)
                return BadRequest("Dados inválidos.");

            _context.Armazem.Add(novoArmazem);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = novoArmazem.Id }, novoArmazem);
        }

        // GET: api/controlearmazem
        /* Lista todos os armazéns cadastrados. */
        [HttpGet]
        public IActionResult ListarArmazens()
        {
            var Armazem = _context.Armazem.ToList();
            return Ok(Armazem);
        }

        // GET: api/controlearmazem/
        /* Busca um armazém pelo seu ID. */
        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var armazem = _context.Armazem.Find(id);

            if (armazem == null)
                return NotFound("Armazém não encontrado.");

            return Ok(armazem);
        }

        // PUT: api/controlearmazem/
        /* Atualiza os dados de um armazém existente */
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] Armazem armazemAtualizado)
        {
            var armazem = _context.Armazem.Find(id);
            if (armazem == null)
                return NotFound("Armazém não encontrado.");

            armazem.nomeArmazem = armazemAtualizado.nomeArmazem;
            armazem.status = armazemAtualizado.status;
            armazem.capacidade = armazemAtualizado.capacidade;
            armazem.endereco = armazemAtualizado.endereco;

            _context.Armazem.Update(armazem);
            _context.SaveChanges();

            return Ok(armazem);
        }

        // DELETE: api/controlearmazem/
        /* Remove um armazém pelo seu ID */
        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var armazem = _context.Armazem.Find(id);
            if (armazem == null)
                return NotFound("Armazém não encontrado.");

            _context.Armazem.Remove(armazem);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
