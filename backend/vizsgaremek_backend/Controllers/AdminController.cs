using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vizsgaremek_backend.Models;

namespace vizsgaremek_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly VizsgaremekAbV1Context _context;

        public AdminController(VizsgaremekAbV1Context context)
        {
            _context = context;
        }
    }
}
