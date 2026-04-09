using Application.Interfaces;
using CodeElevate.DTOs;
using CodeElevate.Extensions;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CodeElevate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PraticeController : ControllerBase
    {

        private readonly IProductService _productionService;

        public PraticeController(IProductService productionService)
        {
            _productionService = productionService;
        }

        [HttpGet("Paginated")]
        public IActionResult GetPaginated(int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid pagination parameters",
                    Detail = "page and pageSize must be greater than 0.",
                    Status = 400
                });

            var products = QueryableExtensions.Paginate<Product>(_productionService.GetAll().OrderBy(x => x.Id), page, pageSize);

            return Ok(products);
        }
    }
}
