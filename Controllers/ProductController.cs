using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using productstockingv1.Data;
using productstockingv1.Interfaces;
using productstockingv1.models;
using productstockingv1.Models.Request;

namespace productstockingv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost("getAll")]
        public async Task<ActionResult> GetProduct(ICollection<IdReq> id)
        {
            if (id == null)
            {
                var all = _context.getRepository<Product, string>().GetAllQueryable().ToList();

                var result = _mapper.Map<List<ProductResponse>>(all);

                return Ok(new
                {
                    success = true,
                    status = 200,
                    message = $"Successfully returned all products",
                    data = result
                });
            }

            return Ok(id);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ProductCreateReq req)
        {
            if (req == null) return BadRequest("Request must filled");

            var product = _mapper.Map<Product>(req);
            _context.BeginTransaction();
            try
            {
                // await _context.getRepository<Product, string>().CreateAsync(product);
                _context.Commit();
                return Ok(new
                {
                    success = true,
                    statusCode = 200,
                    message = $"successfully created ${10} products",
                    data = "test"
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return Conflict("false");
            }

            return CreatedAtAction("GetProduct", new {id = product.Id}, product);
        }


        // private bool ProductExist(string id) => _context.Products.Any(e => e.Id == id);
    }
}