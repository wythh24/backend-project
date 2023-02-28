using System.ComponentModel;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using Org.BouncyCastle.Ocsp;
using productstockingv1.ExtensionFunction;
using productstockingv1.Interfaces;
using productstockingv1.models;
using productstockingv1.Models.Request;

namespace productstockingv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _context;

        public StockController(IMapper mapper, IUnitOfWork context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("Getall")]
        public async Task<ActionResult<IEnumerable<Stocking>>> Get(IdReq id)
        {
            var stockList = new List<Stocking>();
            var StockResponse = new List<StockResponse>();
            var pro_table = _context.getRepository<Product, string>().GetAllQueryable().ToList();
            var resultproduct = _mapper.Map<List<ProductResponse>>(pro_table);
            var ware_table = _context.getRepository<Ware, string>().GetAllQueryable().ToList();
            var resultWare = _mapper.Map<List<WareResponse>>(ware_table);
            if (id.Id != null && id.Id.ToString() != "string" && !string.IsNullOrEmpty(id.Id.ToString()))
            {
                foreach (var item in id.Id)
                {
                    var st = await _context.getRepository<Stocking, string>().GetAsync(item);
                    if (st != null) stockList.Add(st);
                }

                return Ok(ExtenFunction.ResponseDefault("Stocking",
                    stockList.Select(e =>
                    {
                        return new
                        {
                            Id = e.Id,
                            Product = resultproduct.Where(n => n.Id == e.Id),
                            Ware = resultWare.Where(n => n.Id == e.Id),
                            ProductId = e.ProductId,
                            WareId = e.WareId,
                            Quantity = e.Quantity,
                            DocumentDate = e.DocumentDate,
                            PostingDate = e.PostingDate,
                        };
                    }).ToList()
                    , true));
            }

            else
            {
                var all = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
                var result = _mapper.Map<List<StockResponse>>(all);


                return Ok(ExtenFunction.ResponseDefault(
                    "Stocking",
                    result.Select(e => new
                    {
                        Id = e.Id,
                        Product = resultproduct.Where(n => n.Id == e.Id),
                        Ware = resultWare.Where(n => n.Id == e.Id),
                        Productid = e.ProductId,
                        wareId = e.WareId,
                        quantity = e.Quantity,
                        documentDate = e.DocumentDate,
                        postingDate = e.PostingDate,
                    }).ToList()
                    ,
                    false));
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetAll(IdReq id)
        {
            var StockList = new List<Stocking>();
            var pro_table = _context.getRepository<Product, string>().GetAllQueryable().ToList();
            var resultperson = _mapper.Map<List<ProductResponse>>(pro_table);
            var StockResponse = new List<StockResponse>();

            var war_table = _context.getRepository<Ware, string>().GetAllQueryable().ToList();
            var resultWare = _mapper.Map<List<WareResponse>>(war_table);

            if (id.Id != null && id.Id.ToString() != "string" && !string.IsNullOrEmpty(id.Id.ToString()))
            {
                foreach (var item in id.Id)
                {
                    var st = await _context.getRepository<Stocking, string>().GetAsync(item);
                    if (st != null) StockList.Add(st);
                }

                return Ok(ExtenFunction.ResponseDefault("Stocking",
                    StockList.Select(e =>
                    {
                        return new
                        {
                            Id = e.Id,
                            ProductId = e.ProductId,
                            WareId = e.WareId,
                            Quantity = e.Quantity,
                            DocumentDate = e.DocumentDate,
                            PostingDate = e.PostingDate,
                            Product = resultperson.Where(n => n.Id == e.Id),
                            Ware = resultWare.Where(n => n.Id == e.Id)
                        };
                    }).ToList()
                    , true));
            }

            else
            {
                var all = _context.getRepository<Stocking, string>().GetAllQueryable()
                    .ToList();

                var result = _mapper.Map<List<StockResponse>>(all);


                return Ok(ExtenFunction.ResponseDefault(
                    "Stocking",
                    result.Select(e => new
                    {
                        Id = e.Id,
                        product = resultperson.Where(n => n.Id == e.ProductId),
                        ware = resultWare.Where(n => n.Id == e.Id),
                        Productid = e.ProductId,
                        wareId = e.WareId,
                        quantity = e.Quantity,
                        documentDate = e.DocumentDate,
                        postingDate = e.PostingDate,
                    }).ToList()
                    ,
                    false));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Getbyid(string id)
        {
            var StockList = new List<Stocking>();
            if (id != null)
            {
                var stock = await _context.getRepository<Stocking, string>().GetAsync(id);
                if (stock != null) StockList.Add(stock);
                else StockList = null;
            }

            return Ok(StockList == null
                ? ExtenFunction.StockingResponse("Stocking", StockList, false, 404, false, id)
                : ExtenFunction.StockingResponse("Stocking", StockList, false, 302, true, id)
            );
        }

        [HttpGet("GetById")]
        public async Task<ActionResult> GetById([FromBody] GetT? req)
        {
            var StockList = new List<Stocking>();

            if (req.Id != null)
            {
                var stock = await _context.getRepository<Stocking, string>().GetAsync(req.Id);
                if (stock != null) StockList.Add(stock);
                else StockList = null;
            }
            else StockList = null;

            return Ok(StockList == null
                ? ExtenFunction.StockingResponse("Stocking", StockList, false, 404, false, req.Id)
                : ExtenFunction.StockingResponse("Stocking", StockList, false, 302, true, req.Id)
            );
        }

        [HttpGet("GetByProduct")]
        public async Task<ActionResult> GetByProduct(ListStockCreateReq req)
        {
            decimal Quantity = 0;
            var ProductList = new List<Product>();
            var Stock = new List<Stocking>();
            var table = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
            var map = _mapper.Map<List<StockResponse>>(table);
            if (req.ProductId != null)
            {
                foreach (var item in req.ProductId)
                {
                    var p = await _context.getRepository<Product, string>().GetAsyncd(item);
                    if (p != null)
                    {
                        ProductList.Add(p);
                    }
                    else ProductList = null;
                }
            }
            else ProductList = null;

            return Ok(ProductList == null
                ? ExtenFunction.StockingResponse("Stocking",
                    ProductList.Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        name = e.Name,
                        price = e.Price,
                        description = e.Description,
                        OnHands = map.Sum(s => s.Quantity)
                    }).ToList()
                    , false, 404, false, null)
                : ExtenFunction.StockingResponse("Stocking",
                    ProductList.Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        name = e.Name,
                        price = e.Price,
                        Stocking = e.Stockings.Select(s => new
                        {
                            Quantity = s.Quantity,
                            DocumentData = s.DocumentDate,
                            PostingDate = s.PostingDate
                        }),
                        OnHands = map.Where(s => s.ProductId == e.Id).Sum(s => s.Quantity),
                        description = e.Description,
                    }).ToList()
                    , false, 302, true, null)
            );
        }

        [HttpPost("GetByProduct")]
        public async Task<ActionResult> GetPostProduct(ListStockCreateReq req)
        {
            decimal Quantity = 0;
            var ProductList = new List<Product>();
            var Stock = new List<Stocking>();
            var table = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
            var map = _mapper.Map<List<StockResponse>>(table);
            if (req.ProductId != null)
            {
                foreach (var item in req.ProductId)
                {
                    var p = await _context.getRepository<Product, string>().GetAsyncd(item);
                    if (p != null)
                    {
                        ProductList.Add(p);
                    }
                    else ProductList = null;
                }
            }
            else ProductList = null;

            return Ok(ProductList == null
                ? ExtenFunction.StockingResponse("Stocking",
                    ProductList.Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        name = e.Name,
                        price = e.Price,
                        description = e.Description,
                        OnHands = map.Sum(s => s.Quantity)
                    }).ToList()
                    , false, 404, false, null)
                : ExtenFunction.StockingResponse("Stocking",
                    ProductList.Select(e => new
                    {
                        id = e.Id,
                        Code = e.Code,
                        name = e.Name,
                        price = e.Price,
                        Stocking = e.Stockings.Select(s => new
                        {
                            Quantity = s.Quantity,
                            DocumentData = s.DocumentDate,
                            PostingDate = s.PostingDate
                        }),
                        OnHands = map.Where(s => s.ProductId == e.Id).Sum(s => s.Quantity),
                        description = e.Description,
                    }).ToList()
                    , false, 302, true, null)
            );
        }

        [HttpPost("GetByProductCodes")]
        public async Task<ActionResult> GetCodeProduct(ListCodeProduct req)
        {
            decimal Quantity = 0;
            var ProductList = new List<Product>();
            var Stock = new List<Stocking>();
            var table = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
            var map = _mapper.Map<List<StockResponse>>(table);
            if (req.productcodes != null)
            {
                foreach (var item in req.productcodes)
                {
                    // var p = await _context.getRepository<Product, string>().GetAsync(item);
                    // if (p != null) {ProductList.Add(p);}
                    // else ProductList = null;
                    var s = _context.getRepository<Product, string>().GetAllQueryable().ToList();
                }
            }
            else ProductList = null;


            return Ok(ProductList == null
                ? ExtenFunction.StockingResponse("Stocking",
                    ProductList.Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        name = e.Name,
                        price = e.Price,
                        description = e.Description,
                        OnHands = map.Sum(s => s.Quantity)
                    }).Where(e => e.Code == req.productcodes.ToString()).ToList()
                    , false, 404, false, null)
                : ExtenFunction.StockingResponse("Stocking",
                    ProductList.Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        name = e.Name,
                        price = e.Price,
                        Stocking = e.Stockings.Select(s => new
                        {
                            Quantity = s.Quantity,
                            DocumentData = s.DocumentDate,
                            PostingDate = s.PostingDate
                        }),
                        OnHands = map.Where(s => s.ProductId == e.Id).Sum(s => s.Quantity),
                        description = e.Description,
                    }).Where(e => e.Code == req.productcodes.ToString()).ToList()
                    , false, 302, true, null)
            );
        }


        [HttpGet("GetByWare")]
        public async Task<ActionResult> GetByWare(ListWareGet req)
        {
            decimal Quantity = 0;
            // var ProductList = new List<Product>();
            var WareList = new List<Ware>();
            var Stock = new List<Stocking>();
            var table = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
            var map = _mapper.Map<List<StockResponse>>(table);
            if (req.wareId != null)
            {
                foreach (var item in req.wareId)
                {
                    var w = await _context.getRepository<Ware, string>().GetAsyncd(item);
                    if (w != null)
                    {
                        WareList.Add(w);
                    }
                    else WareList = null;
                }
            }
            else WareList = null;

            return Ok(WareList == null
                ? ExtenFunction.StockingResponse("Ware",
                    WareList.Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        Stock = e.Stockings.Select(s => new
                        {
                            Id = s.Id,
                            //notyet
                            product = s.product,
                            Ware = s.Ware,
                            productid = s.ProductId,
                            wareId = s.WareId,
                            Quantity = s.Quantity,
                            documentDate = s.DocumentDate,
                            postingDate = s.PostingDate
                        }).ToList(),
                        name = e.Name,
                        description = e.Description
                    }).ToList()
                    , false, 404, false, null)
                : ExtenFunction.StockingResponse("Ware",
                    WareList.Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        Stock = e.Stockings.Select(s => new
                        {
                            Id = s.Id,
                            //notyet
                            product = s.product,
                            Ware = s.Ware,
                            productid = s.ProductId,
                            wareId = s.WareId,
                            Quantity = s.Quantity,
                            documentDate = s.DocumentDate,
                            postingDate = s.PostingDate
                        }).ToList(),
                        name = e.Name,
                        description = e.Description,
                        OnHands = map.Sum(s => s.Quantity)
                    }).ToList()
                    , false, 302, true, null)
            );
        }


        [HttpPost("GetByWare")]
        public async Task<ActionResult> GetPostProduct(ListWareGet req)
        {
            decimal Quantity = 0;
            // var ProductList = new List<Product>();
            var WareList = new List<Ware>();
            var Stock = new List<Stocking>();
            var table = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
            var map = _mapper.Map<List<StockResponse>>(table);
            if (req.wareId != null)
            {
                foreach (var item in req.wareId)
                {
                    var w = await _context.getRepository<Ware, string>().GetAsyncd(item);
                    if (w != null)
                    {
                        WareList.Add(w);
                    }
                    else WareList = null;
                }
            }
            else WareList = null;

            return Ok(WareList == null
                ? ExtenFunction.StockingResponse("Ware",
                    WareList.Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        Stock = e.Stockings.Select(s => new
                        {
                            Id = s.Id,
                            //notyet
                            product = s.product,
                            Ware = s.Ware,
                            productid = s.ProductId,
                            wareId = s.WareId,
                            Quantity = s.Quantity,
                            documentDate = s.DocumentDate,
                            postingDate = s.PostingDate
                        }).ToList(),
                        name = e.Name,
                        description = e.Description
                    }).ToList()
                    , false, 404, false, null)
                : ExtenFunction.StockingResponse("Ware",
                    WareList.Select(e => new
                    {
                        Id = e.Id,
                        Code = e.Code,
                        Stock = e.Stockings.Select(s => new
                        {
                            Id = s.Id,
                            //notyet
                            product = s.product,
                            Ware = s.Ware,
                            productid = s.ProductId,
                            wareId = s.WareId,
                            Quantity = s.Quantity,
                            documentDate = s.DocumentDate,
                            postingDate = s.PostingDate
                        }).ToList(),
                        name = e.Name,
                        description = e.Description,
                        OnHands = map.Sum(s => s.Quantity)
                    }).ToList()
                    , false, 302, true, null)
            );
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateExistProduct(ListCreate req)
        {
            var product = new List<Product>();
            var result = new List<Stocking>();
            var ware = new List<Ware>();
            //change to Any()
            if (!req.command.Any()) return BadRequest("Request must be filled");

            foreach (var wa in ware)
            {
                foreach (var pro in product)
                {
                    if (pro != null && wa != null)
                    {
                        result = req.command
                            .Select(e => _mapper.Map<Stocking>(e.Quantity)).ToList();
                    }
                    else return Ok("Fail");
                }
            }

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Stocking, string>().CreateBatchAsync(result);
                _context.Commit();
                return Ok(new
                {
                    success = true,
                    statusCode = 200,
                    message = $"Successfully created {result.Count} products",
                    data = result.Select(e => e.Quantity).ToList()
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return Conflict("Something gone wrong!");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ListStockUpdateReq req)
        {
            if (!req.command.Any())
                return BadRequest("Not found Request");

            var StockList = new List<Stocking>();

            if (req.command.Any())
            {
                foreach (var item in req.command)
                {
                    var stock = await _context.getRepository<Stocking, string>().GetAsync(item.Id);
                    if (stock != null) StockList.Add(stock);
                    else StockList = null;
                }
            }

            var StockReq = new List<StockUpdateReq>();
            foreach (var item in req.command)
            {
                StockReq.Add(item);
            }

            if (StockList.Count > 0)
            {
                foreach (var stock in StockList)
                {
                    foreach (var stReq in StockReq.Where(stReq => stock.Id == stReq.Id))
                    {
                        stock.Quantity = stReq.Quantity;
                    }
                }
            }


            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Stocking, string>().UpdateBatchAsync(StockList);
                _context.Commit();
                return Ok(ExtenFunction.ResponseDefault("Stocking", StockList));
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest(ExtenFunction.ResponseDefault("Stocking", StockList, false, 404));
            }
        }

        // [HttpPut("transfer")]
        // public async Task<ActionResult> Transfer(ListStockTransferReq req)
        // {
        //     var StockList = new List<Stocking>();
        //     if (!req.commands.Any())
        //     {
        //         return BadRequest();
        //     }
        //     
        // }

        [HttpDelete]
        public async Task<ActionResult> DeleteStock(IdReq id)
        {
            var StockList = new List<Stocking>();
            if (id == null)
                return BadRequest(new
                {
                    succes = false,
                    StatusCode = 400,
                    message = "No giving Ids to delete stocking",
                    data = StockList = null,
                });


            if (id != null)
            {
                foreach (var item in id.Id)
                {
                    var stock = await _context.getRepository<Stocking, string>().GetAsync(item);
                    if (stock != null) StockList.Add(stock);
                }
            }

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Stocking, string>().DeleteBatchAsync(StockList);
                _context.Commit();
                return Ok(new
                {
                    success = true,
                    StatusCode(200).StatusCode,
                    message = $"Successfully deleted {StockList.Count} Stocking",
                    data = StockList.Select(e => e.Id).ToList()
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest(new
                {
                    success = false,
                    StatusCode(304).StatusCode,
                    message = $"No Stocking Deleted",
                    data = StockList.Select(e => e.Id).ToList()
                });
            }
        }
    }
}