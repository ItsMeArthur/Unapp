using Microsoft.AspNetCore.Mvc;
using Una.Presentation.API.Entities;
using Una.Presentation.API.Repositories.Contracts;

namespace Una.Presentation.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDataRepository<User> _repository;

        public UserController(IDataRepository<User> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repository.ListAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingle(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            var addedUser = await _repository.AddAsync(user);
            return CreatedAtAction(nameof(GetSingle), new { Id = addedUser.Id}, addedUser);
        }

        [HttpPost("seed")]
        public async Task<IActionResult> CreateMultiple(List<User> users)
        {
            foreach (var user in users)
            {
                await _repository.AddAsync(user);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, User updatedUser)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound();
            }
            
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.Username = updatedUser.Username;
            existingUser.Password = updatedUser.Password;
            existingUser.DateOfBirth = updatedUser.DateOfBirth;
            existingUser.Gender = updatedUser.Gender;
            existingUser.IsActive = updatedUser.IsActive;
            existingUser.UpdatedAt = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(existingUser);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "Ocorreu um erro ao salvar as alterações.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingUser = await _repository.GetByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            var result =  await _repository.DeleteAsync(existingUser);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "Ocorreu um erro ao excluir o registro.");
            }
        }
    }
}
