using CrudDapperVideo.Dto;
using CrudDapperVideo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudDapperVideo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioInterface;

        public UsuarioController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }

        //vai retorna todos os usuários do banco
        [HttpGet]
        public async Task<IActionResult> BuscarUsuario()
        {   
            //esse BuscarUusiario eu criei la no service
            var usuarios = await _usuarioInterface.BuscarUsuarios();

            if(usuarios.Status == false)
            {
                return NotFound(usuarios);
            }
            //vai retorna a lista com todos o "Ok" é de 200
            return Ok(usuarios);
        }

        //dizendo que esse get precisa receber um usuarioId
        [HttpGet("{usuarioId}")]
        public async Task<IActionResult> BuscarUsuarioPorId(int usuarioId)
        {
            //chamando o método criado la no service
            var usuario = await _usuarioInterface.BuscarUsuarioPorId(usuarioId);

            if(usuario.Status == false)
            {
                return NotFound(usuario);
            }

            return Ok(usuario);
        }

        //esse vai criar, por isso usamos o httpPost
        [HttpPost]
        public async Task<IActionResult> CriarUsuario(UsuarioCriarDto usuarioCriarDto)
        {
            var usuarios = await _usuarioInterface.CriarUsuario(usuarioCriarDto);

            //quando ocorre uma má solicitação
            if (usuarios.Status == false)
            {
                return BadRequest(usuarios);
            }

            return Ok(usuarios);
        }

        //esse vai alterar algo da tabela
        [HttpPut]
        public async Task<IActionResult> EditarUsuario(UsuarioEditarDto usuarioEditarDto)
        {
            var usuarios = await _usuarioInterface.EditarUsuario(usuarioEditarDto);

            if (usuarios.Status == false)
            {
                return BadRequest(usuarios);
            }

            return Ok(usuarios);
        }


        //esse vai deletar algo da tabela
        [HttpDelete]
        public async  Task<IActionResult> RemoverUsuarios(int usuarioId)
        {
            var usuarios = await _usuarioInterface.RemoverUsuario(usuarioId);

            if(usuarios.Status == false)
            {
                return BadRequest(usuarios);
            }

            return Ok(usuarios);
        }
    }
}
