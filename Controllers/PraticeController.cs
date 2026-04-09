using Application.Interfaces;
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

        [HttpGet]
        public List<Product> Get()
        {
            return _productionService.GetAll();
        }
    }
}
