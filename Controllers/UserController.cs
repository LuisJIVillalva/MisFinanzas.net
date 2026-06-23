using CursoApis.Models;
using CursoApis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace CursoApis.Controllers
{
    /// <summary>
    /// Controlador HTTP para gestionar usuarios (listar, obtener, crear, actualizar y eliminar).
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Retorna todos los usuarios registrados.
        /// </summary>
        /// <returns>Lista completa de usuarios.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Retorna un usuario por su identificador.
        /// </summary>
        /// <param name="id">Id del usuario a buscar.</param>
        /// <returns>Usuario encontrado o 404 si no existe.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Crea un nuevo usuario con los datos recibidos en el cuerpo.
        /// </summary>
        /// <param name="user">Datos del nuevo usuario.</param>
        /// <returns>Usuario creado con respuesta 201.</returns>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            var createdUser = await _userService.CreateAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Actualiza un usuario existente según su identificador.
        /// </summary>
        /// <param name="id">Id del usuario a actualizar.</param>
        /// <param name="user">Nuevos datos del usuario.</param>
        /// <returns>Usuario actualizado o 404 si no existe.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {
            var updatedUser = await _userService.UpdateByIdAsync(id, user);
            if (updatedUser == null) return NotFound();
            return Ok(updatedUser);
        }

        /// <summary>
        /// Elimina un usuario por su identificador.
        /// </summary>
        /// <param name="id">Id del usuario a eliminar.</param>
        /// <returns>Usuario eliminado o 404 si no existe.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var deletedUser = await _userService.DeleteByIdAsync(id);
            if (deletedUser == null) return NotFound();
            return Ok(deletedUser);
        }
    }
}