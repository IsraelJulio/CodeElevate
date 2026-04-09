using Application.Interfaces;
using CodeElevate.DTOs;
using CodeElevate.Extensions;
using Domain.Entities;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CodeElevate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private static readonly ConcurrentDictionary<int, string> BackgroundTasks = new();

        private readonly IProductService _productionService;

        public ProductController(IProductService productionService)
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
        [HttpPost("WithoutTask")]
        public IActionResult GetWithoutTask(CreateProductDto dto)
        {
            var products = _productionService.GetAll().ToList();
            var entity = new Product { Id = products.Count + 1, Name = dto.Name, Price = dto.Price };
            products.Add(entity);
            return Created($"/Product/{entity.Id}", entity);

        }
        [HttpPost("WithTask")]
        public async Task<IActionResult> PostWithTask(CreateProductDto dto)
        {
            var products = _productionService.GetAll().ToList();
            var entity = new Product { Id = products.Count + 1, Name = dto.Name, Price = dto.Price };
            products.Add(entity);

            var processingId = entity.Id;
            BackgroundTasks[processingId] = "Processing";

            _ = Task.Run(async () =>
            {
                try
                {
                    // Simulates a long-running job that continues after the HTTP response like send email.
                    await Task.Delay(TimeSpan.FromSeconds(15));

                    BackgroundTasks.TryUpdate(processingId, "Complete", "Processing");
                }
                catch
                {
                    BackgroundTasks[processingId] = "Failed";
                }
            });

            return Accepted($"/Product/{entity.Id}", new
            {
                product = entity,
                processingId,
                status = BackgroundTasks[processingId]
            });
        }

        [HttpGet("WithTask/{processingId}/status")]
        public IActionResult GetTaskStatus(int processingId)
        {
            if (!BackgroundTasks.TryGetValue(processingId, out var status))
                return NotFound(new ProblemDetails
                {
                    Title = "Processing not found",
                    Detail = "No background processing was found for the provided id.",
                    Status = 404
                });

            return Ok(new { processingId, status });
        }
    }
}
