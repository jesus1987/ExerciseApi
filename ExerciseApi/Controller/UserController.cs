using ExerciseApiBusiness.Interfaces.SmartSearch;
using ExerciseApiBusiness.Interfaces.User;
using ExerciseApiViewModel.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExerciseApi.Controller
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserBusiness _user;
        private readonly ISmartSearchBusiness _smartSearch;
        public UserController(IUserBusiness user, ISmartSearchBusiness smartSearch)
        {
            _user = user;
            _smartSearch = smartSearch;
        }

        [HttpGet("/GetData")]
        public async Task<IActionResult> GetData(string naturalQuery)
        {
            var result = await _smartSearch.SearchAsync(naturalQuery);
            return Ok(result.Value);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var result = await _user.GetAllAsync();
            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _user.GetByIdAsync(id);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserViewModel request)
        {
            var result = await _user.CreateAsync(request);

            if (result.IsFailed)
            {
                return BadRequest(new
                {
                    Errors = result.Errors.Select(e => e.Message)
                });
            }

            return CreatedAtAction(nameof(Get), new { id = result.Value }, result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserViewModel request)
        {
            if (id != request.Id)
            {
                return BadRequest("ID in the URL does not match the ID in the request body.");
            }

            var result = await _user.UpdateAsync(request);

            if (result.IsFailed)
            {
                return BadRequest(new
                {
                    Errors = result.Errors.Select(e => e.Message)
                });
            }

            return Ok(result.Value);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _user.DeleteAsync(id);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);
        }
    }
}
