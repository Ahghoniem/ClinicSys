using LibraryMAnagementSystem.API.Controllers.Base;
using LibraryManagementSystem.BLL.DTOs.ProductDTOs;
using LibraryManagementSystem.BLL.Helpers;
using LibraryManagementSystem.BLL.Specifications;
using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.BLL.ServicesContracts;
using LibraryMAnagementSystem.API.ResponseHandler;

namespace LibraryMAnagementSystem.API.Controllers.Products
{
    [NonController]
    public class ProductsController(IProductServices productServices) : BaseAPIController
    {
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetAllProducts([FromQuery] ProductSpeceficationsParams parameters)
        {
            var products = await productServices.GetProductsAsync(parameters);
            return Ok(products);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var product = await productServices.GetProductById(id);

            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var brands = await productServices.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategoires()
        {
            var Categories = await productServices.GetCategoriesAsync();
            return Ok(Categories);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteBrand(int id)
        {


            await productServices.DeleteBrandByIdAsync(id);
            var successResponse = new OldApiResponse<object>(200, $"Brand with Id {id} is deleted successfully");
            return Ok(successResponse);
        }
        [HttpPost]
        public async Task<ActionResult<BrandDTO>> AddBrand(AddBrandDTO addBrandDTO)
        {
            var res = await productServices.AddBrandAsync(addBrandDTO);

            if (res == 1)
            {
                var successResponse = new OldApiResponse<AddBrandDTO>(200, "Brand added successfully", addBrandDTO);
                return Ok(successResponse);
            }

            var errorResponse = new OldApiResponse<object>(400, "Failed to add brand");
            return BadRequest(errorResponse);
        } 
        [HttpPut]
        public async Task<ActionResult<BrandDTO>> AUpdateBrand(EditBrandDTO editBrandDTO)
        {
            var updatedBrand = await productServices.UpdateBrandAsync(editBrandDTO);

            if (updatedBrand == null)
            {
                var errorResponse = new OldApiResponse<object>(404, $"Brand with Id {editBrandDTO.BrandId} not found");
                return NotFound(errorResponse);
            }

            var successResponse = new OldApiResponse<object>(200, "Brand updated successfully", updatedBrand);
            return Ok(successResponse);
        }
    }
}
