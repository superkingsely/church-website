

using domain.src.entities;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly AppDbContext dbContext;

    public TestController(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
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

    [HttpPost("new-member")]
    public async Task<IActionResult> Post([FromBody] CreateMemberCommand newmember)
    {
        // Process the posted value
        var member = new Member
        {
            FirstName = newmember.FirstName,
            LastName = newmember.LastName,
            Email = newmember.Email,
            PhoneNumber = newmember.PhoneNumber,
            CreatedAt = DateTime.UtcNow
        };
        dbContext.Members.Add(member);
        await dbContext.SaveChangesAsync();
        return Ok(new { message = "Member created", data = newmember });
    }
    
    
    // [HttpGet("error")]
    // public IActionResult Error()
    // {
    //     // throw new Exception("Testing Global Exception Middleware");
    // }
}