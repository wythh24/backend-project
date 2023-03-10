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
        public async Task<ActionResult> GetProduct(IdReq req = null)
        {
            //fixme get product without loop
            if (req.Id != null)
            {
                var proList = _context.getRepository<Product, string>().GetAllQueryable()
                    .Where(e => req.Id.Contains(e.Id)).Include(e => e.Stockings).ToList();

                return Ok(ExtenFunction.ResponseDefault("Product", proList.Select(e => new
                {
                    id = e.Id,
                    code = e.Code,
                    stocking = e.Stockings.Select(s => new
                    {
                        id = s.Id,
                        productid = s.ProductId,
                        wareid = s.WareId,
                        quantity = s.Quantity,
                        documentdate = s.DocumentDate,
                        postingdate = s.PostingDate
                    }).Where(w => w.productid == e.Id).ToList(),
                    name = e.Name,
                    price = e.Price,
                    description = e.Description
                }).ToList()));
            }

            //modified delete else if 
            var all = _context.getRepository<Product, string>().GetAllQueryable().Include(e => e.Stockings).ToList();

            return Ok(ExtenFunction.ResponseDefault("Product", all.Select(e => new
            {
                id = e.Id,
                code = e.Code,
                stocking = e.Stockings.Select(s => new
                {
                    id = s.Id,
                    productid = s.ProductId,
                    wareid = s.WareId,
                    quantity = s.Quantity,
                    documentdate = s.DocumentDate,
                    postingdate = s.PostingDate
                }).Where(w => w.productid == e.Id).ToList(),
                name = e.Name,
                price = e.Price,
                description = e.Description
            }).ToList(),false));
        }

        // request with body (GET)
        [HttpGet]
        public async Task<ActionResult> GetProductById([FromBody] IdReq req = null)
        {
            // modified using query 
            //fixme get product without loop
            if (req.Id != null)
            {
                var proList = _context.getRepository<Product, string>().GetAllQueryable()
                    .Where(e => req.Id.Contains(e.Id)).Include(e => e.Stockings).ToList();

                return Ok(ExtenFunction.ResponseDefault("Product", proList.Select(e => new
                {
                    id = e.Id,
                    code = e.Code,
                    stocking = e.Stockings.Select(s => new
                    {
                        id = s.Id,
                        productid = s.ProductId,
                        wareid = s.WareId,
                        quantity = s.Quantity,
                        documentdate = s.DocumentDate,
                        postingdate = s.PostingDate
                    }).Where(w => w.productid == e.Id).ToList(),
                    name = e.Name,
                    price = e.Price,
                    description = e.Description
                }).ToList()));
            }

            //modified delete else if 
            var all = _context.getRepository<Product, string>().GetAllQueryable().Include(e => e.Stockings).ToList();

            return Ok(ExtenFunction.ResponseDefault("Product", all.Select(e => new
            {
                id = e.Id,
                code = e.Code,
                stocking = e.Stockings.Select(s => new
                {
                    id = s.Id,
                    productid = s.ProductId,
                    wareid = s.WareId,
                    quantity = s.Quantity,
                    documentdate = s.DocumentDate,
                    postingdate = s.PostingDate
                }).Where(w => w.productid == e.Id).ToList(),
                name = e.Name,
                price = e.Price,
                description = e.Description
            }).ToList(),false));
        }

        // query string
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProduct(string id)
        {
            var productList = new List<Product>();
            //fixme get product without add to another list

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

            //fixme get product without add to another list
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
        public async Task<ActionResult> CreateProduct(ListProductCreateReq req)
        {
            //change to Any()
            if (!req.command.Any()) return BadRequest("Request must be filled");

            var productList = req.command.Select(item => _mapper.Map<Product>(item)).ToList();

            if (productList.Count != req.command.ToList().Count) return BadRequest("Create product was false");

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Product, string>().CreateBatchAsync(productList);
                _context.Commit();

                return Ok(new
                {
                    success = true,
                    statusCode = 200,
                    message =
                        $"Successfully created {productList.Count} product{(productList.Count.Equals(1) ? "" : "s")}",
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
            if (!req.Id.Any()) return BadRequest($"Id must be filled");

            //modified removed product id list
            //paste req.id as list into ProductList directly
            var ProductList = _context.getRepository<Product, string>().GetAllQueryable()
                .Where(e => req.Id.ToList().Contains(e.Id)).ToList();

            //if list of product has incorrect id
            if (ProductList.Count != req.Id.Count) return BadRequest($"Delete was false");

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Product, string>().DeleteBatchAsync(ProductList);

                _context.Commit();
                return Ok(new
                {
                    success = true,
                    StatusCode(200).StatusCode,
                    message = $"Successfully deleted {ProductList.ToList().Count} products",
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
            if (!req.command.Any()) return BadRequest("Not found Request");

            var ProductList = new List<Product>();

            //fixme get product without loop
            foreach (var item in req.command)
            {
                var product = await _context.getRepository<Product, string>().GetAsync(item.Id);
                if (product != null) ProductList.Add(product);
            }

            var ProductReq = req.command.ToList();

            if (ProductList.Count != ProductReq.Count) return Ok($"Updated was false");

            foreach (var product in ProductList)
            {
                foreach (var proReq in ProductReq.Where(proReq => product.Id == proReq.Id))
                {
                    product.Name = proReq.Name;
                    product.Price = (decimal) proReq.Price;
                    product.Description = proReq.Description;
                }
            }

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Product, string>().UpdateBatchAsync(ProductList);
                _context.Commit();
                //modified response
                return Ok(new
                {
                    success = true,
                    statuscode = 200,
                    message = $"Successfully updated {ProductList.Count} product{(ProductList.Count > 1 ? "s" : "")}",
                    data = ProductList.Select(e => e.Id).ToList()
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest("Something gone wrong");
            }
        }
    }
}