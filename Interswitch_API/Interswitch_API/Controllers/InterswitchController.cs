using Interswitch_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Interswitch_API.Controllers;


[ApiController]
[Route("[controller]")]
public class InterswitchController : ControllerBase
{
    private readonly IInterswitchService _interswitchService;

    public InterswitchController(IInterswitchService interswitchService)
    {
        this._interswitchService = interswitchService;
    }

    [HttpGet("banks")]
    public async Task<IActionResult> GetFundsTransferBanks()
    {
        try
        {
            var getBanks = await _interswitchService.GetFundsTransferBanksAsync();

            return Ok(getBanks);
        }
        catch (Exception ex) 
        {
            return BadRequest(ex.Message);
        }
    }
}
