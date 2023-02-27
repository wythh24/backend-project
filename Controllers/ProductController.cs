using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using productstockingv1.Data;
using productstockingv1.ExtensionFunction;
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

        // get product with post body (POST)
        [HttpPost("getAll")]
        public async Task<ActionResult> GetProduct(IdReq id = null)
        {
            var ProductList = new List<Product>();
            if (id.Id != null && id.Id.ToString() != "string")
            {
                foreach (var item in id.Id)
                {
                    var p = await _context.getRepository<Product, string>().GetAsync(item);
                    if (p != null) ProductList.Add(p);
                }
            }

            else if (
                id.Id == null
                || string.IsNullOrEmpty(id.Id.ToString())
                || id.Id.ToString() == "string"
            )
            {
                var all = _context.getRepository<Product, string>().GetAllQueryable().ToList();

                var result = _mapper.Map<List<ProductResponse>>(all);

                return Ok(ExtenFunction.ResponseDefault("Product", result, false));
            }

            return Ok(ExtenFunction.ResponseDefault("Product", ProductList));
        }

        // request with body (GET)
        [HttpGet]
        public async Task<ActionResult> GetProductById(IdReq id = null)
        {
            var ProductList = new List<Product>();

            if (id.Id != null)
            {
                foreach (var item in id.Id)
                {
                    var p = await _context.getRepository<Product, string>().GetAsync(item);
                    if (p != null) ProductList.Add(p);
                }
            }

            else if (string.IsNullOrEmpty(id.Id.ToString()))
            {
                var all = _context.getRepository<Product, string>().GetAllQueryable().ToList();

                var result = _mapper.Map<List<ProductResponse>>(all);

                return Ok(ExtenFunction.ResponseDefault(
                    "Product",
                    result,
                    false
                ));
            }

            return Ok(ExtenFunction.ResponseDefault("Product", ProductList));
        }

        // query string
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(string id)
        {
            var productList = new List<Product>();

            if (id != null)
            {
                var product = await _context.getRepository<Product, string>().GetAsync(id);
                if (product != null) productList.Add(product);
            }

            //passed
            return Ok(productList.Count > 0
                ? ExtenFunction.ResponseDefault("Product", productList, false, 302)
                : ExtenFunction.ResponseDefault("Product", productList, false)
            );
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetByBodyId([FromBody] GetT req)
        {
            var productList = new List<Product>();

            if (req.Id != null)
            {
                var product = await _context.getRepository<Product, string>().GetAsync(req.Id);
                if (product != null) productList.Add(product);
            }

            return Ok(productList.Count > 0
                ? ExtenFunction.ResponseDefault("Product", productList, false, 302)
                : ExtenFunction.ResponseDefault("Product", productList, false));
        }

        // create product
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(ListProductCreateReq req)
        {
            //change to Any()
            if (!req.command.Any()) return BadRequest("Request must be filled");

            var productList = req.command.Select(item => _mapper.Map<Product>(item)).ToList();

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Product, string>().CreateBatchAsync(productList);
                _context.Commit();
                return Ok(new
                {
                    success = true,
                    statusCode = 200,
                    message = $"Successfully created {productList.Count} products",
                    data = productList.Select(e => e.Id).ToList()
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return Conflict("Something gone wrong!");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(IdReq req)
        {
            if (req == null) return BadRequest("Id must be filled");

            var ProductList = new List<Product>();

            if (req != null)
            {
                foreach (var item in req.Id)
                {
                    var product = await _context.getRepository<Product, string>().GetAsync(item);
                    if (product != null) ProductList.Add(product);
                }
            }

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Product, string>().DeleteBatchAsync(ProductList);
                _context.Commit();
                return Ok(new
                {
                    success = true,
                    StatusCode(200).StatusCode,
                    message = $"Successfully deleted {ProductList.Count} products",
                    data = ProductList.Select(e => e.Id).ToList()
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest($"Something gone wrong");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ListProductUpdateReq req)
        {
            if (!req.command.Any())
                return BadRequest("Not found Request");

            var ProductList = new List<Product>();

            if (req.command.Any())
            {
                foreach (var item in req.command)
                {
                    var product = await _context.getRepository<Product, string>().GetAsync(item.Id);
                    ProductList.Add(product);
                }
            }

            var ProductReq = new List<ProductUpdateReq>();
            foreach (var item in req.command)
            {
                ProductReq.Add(item);
            }

            if (ProductList.Count > 0)
            {
                foreach (var product in ProductList)
                {
                    foreach (var proReq in ProductReq.Where(proReq => product.Id == proReq.Id))
                    {
                        product.Name = proReq.Name;
                        product.Price = (decimal) proReq.Price;
                    }
                }
            }


            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Product, string>().UpdateBatchAsync(ProductList);
                _context.Commit();
                return Ok($"Product update {ProductList.Count}");
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest("Update Product was false");
            }
        }
    }
}