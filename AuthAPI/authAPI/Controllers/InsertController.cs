using authAPI.BLL;
using authAPI.Model.Common;
using DAL.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsertController : ControllerBase
    {

        private readonly InsertProduct _insertProduct;

        public InsertController(InsertProduct insertProduct)
        {
            _insertProduct = insertProduct;
        }

        [Authorize]
        [HttpPost("IN001")]
        public async Task<IActionResult> ProductInsert(commonProduct entitys)
        {
            try
            {
                var data = await _insertProduct.InsertProductFinally(entitys);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting product", ex);
            }
        }
    }
}
