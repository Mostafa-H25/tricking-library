using Microsoft.AspNetCore.Mvc;

namespace TrickingLibrary.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Home: ControllerBase
{
    public string Index()
    {
        return "Hello World!";
    }
}