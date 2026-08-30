

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var message = new
        {
            message = "okay"
        };

        return Ok(message);
    }
    [HttpGet("string")]
    public string GetString()
    {


        return "cool";
    }

    
    // [HttpGet("error")]
    // public IActionResult Error()
    // {
    //     // throw new Exception("Testing Global Exception Middleware");
    // }
}