using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerarToken.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class SaudacoesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Ola mundo bonito!");
    }
}