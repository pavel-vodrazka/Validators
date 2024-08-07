using Microsoft.AspNetCore.Mvc;
using Validators.Models;

namespace Validators.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CzechBirthNumberController : ControllerBase
    {
        private readonly ILogger<CzechBirthNumberController> _logger;

        public CzechBirthNumberController(ILogger<CzechBirthNumberController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("{birthNumberWithoutSlash}")]
        public BirthNumberValidationDto Get(string birthNumberWithoutSlash)
        {
            return BirthNumber.Validate(birthNumberWithoutSlash);
        }

        [HttpGet]
        [Route("{datePart}/{suffix}")]
        public BirthNumberValidationDto Get(string datePart, string suffix)
        {
            return BirthNumber.Validate($"{datePart}/{suffix}");
        }
    }
}
