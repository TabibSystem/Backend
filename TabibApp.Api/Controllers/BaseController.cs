using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TabibApp.Api.Controllers;

public class BaseController : Controller
{
    protected string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}