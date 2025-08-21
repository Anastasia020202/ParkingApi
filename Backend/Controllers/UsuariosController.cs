using Microsoft.AspNetCore.Mvc;
using ParkingApi.Models;
using ParkingApi.Services;

namespace ParkingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Usuario>> GetUsuarios()
        {
            var usuarios = _usuarioService.GetAllUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public ActionResult<Usuario> GetUsuario(int id)
        {
            var usuario = _usuarioService.GetUsuarioById(id);
            if (usuario == null)
                return NotFound();
            return Ok(usuario);
        }

        [HttpPost]
        public ActionResult<Usuario> CreateUsuario(Usuario usuario)
        {
            var createdUsuario = _usuarioService.CreateUsuario(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = createdUsuario.Id }, createdUsuario);
        }

        [HttpPut("{id}")]
        public ActionResult<Usuario> UpdateUsuario(int id, Usuario usuario)
        {
            var updatedUsuario = _usuarioService.UpdateUsuario(id, usuario);
            if (updatedUsuario == null)
                return NotFound();
            
            return Ok(updatedUsuario);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUsuario(int id)
        {
            var deleted = _usuarioService.DeleteUsuario(id);
            if (!deleted)
                return NotFound();
            
            return NoContent();
        }
    }
}
