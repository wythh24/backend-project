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
using productstockingv1.models.StockRes;

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
            var StockList = new List<Stocking>();
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
                    if (st != null) StockList.Add(st);
                }

                return Ok(ExtenFunction.ResponseDefault("Stocking",
                    StockList.Select(e => new
                    {
                        Id = e.Id,
                        Product = new
                        {
                            Id = e.product.Id,
                            code = e.product.Code,
                            name = e.product.Name,
                            price = e.product.Price,
                            description = e.product.Description
                        },
                        Ware = new
                        {
                            id = e.Ware.Id,
                            code = e.Ware.Code,
                            name = e.Ware.Name,
                            description = e.Ware.Description
                        },
                        Productid = e.ProductId,
                        wareId = e.WareId,
                        quantity = e.Quantity,
                        documentDate = e.DocumentDate,
                        postingDate = e.PostingDate,
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
                        Product = new
                        {
                            Id = e.product.Id,
                            code = e.product.Code,
                            name = e.product.Name,
                            price = e.product.Price,
                            description = e.product.Description
                        },
                        Ware = new
                        {
                            Id = e.Ware.Id,
                            code = e.Ware.Code,
                            name = e.Ware.Name,
                            description = e.Ware.Description
                        },
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
                    StockList.Select(e => new
                    {
                        Id = e.Id,
                        Product = new
                        {
                            Id = e.product.Id,
                            code = e.product.Code,
                            name = e.product.Name,
                            price = e.product.Price,
                            description = e.product.Description
                        },
                        Ware = new
                        {
                            Id = e.Ware.Id,
                            code = e.Ware.Code,
                            name = e.Ware.Name,
                            description = e.Ware.Description
                        },
                        Productid = e.ProductId,
                        wareId = e.WareId,
                        quantity = e.Quantity,
                        documentDate = e.DocumentDate,
                        postingDate = e.PostingDate,
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
                        id = e.Id,
                        Product = new
                        {
                            Id = e.product.Id,
                            code = e.product.Code,
                            name = e.product.Name,
                            price = e.product.Price,
                            description = e.product.Description
                        },
                        Ware = new
                        {
                            Id = e.Ware.Id,
                            code = e.Ware.Code,
                            name = e.Ware.Name,
                            description = e.Ware.Description
                        },
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

        //modified
        [HttpGet("GetByProduct")]
        public async Task<ActionResult> GetByProduct(ListStockStringReq req)
        {
            decimal Quantity = 0;
            var ProductList = new List<Product>();
            var Stock = new List<Stocking>();
            Stock = null;
            var table = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
            var map = _mapper.Map<List<StockResponse>>(table);
            if (req.ProductId != null)
            {
                foreach (var item in req.ProductId)
                {
                    // var p = await _context.getRepository<Product, string>().GetAsyncd(item);
                    var pro = _context.getRepository<Product, string>().GetAllQueryable().ToList()
                        .Where(e => e.Id == item);
                    if (pro != null)
                    {
                        foreach (var p in pro)
                        {
                            var st = _context.getRepository<Stocking, string>().GetAllQueryable().ToList()
                                .Where(e => e.ProductId == p.Id);
                            if (st != null)
                            {
                                return Ok(ExtenFunction.StockingResponse("Stocking",
                                    st.Select(e => new
                                    {
                                        product = new
                                        {
                                            Id = e.product.Id,
                                            code = e.product.Code,
                                            stockings = Stock,
                                            name = e.product.Name,
                                            price = e.product.Price,
                                            description = e.product.Description,
                                        },
                                        OnHands = map.Where(s => s.ProductId == p.Id).Sum(s => s.Quantity)
                                    }).ToList()
                                    , false, 302, true, null)
                                );
                            }
                        }
                    }
                }
            }

            return Ok(ExtenFunction.StockingResponse("Stocking",
                ProductList.ToList()
                , false, 404, false, null)
            );
        }

        //fixme modify condition (error)
        [HttpPost("GetByProduct")]
        public async Task<ActionResult> GetPostProduct(ListStockStringReq req)
        {
            if (!req.ProductId.Any()) return BadRequest("Must be filled request body");

            //get product with product id
            //modified to get by distinct
            var productList = _context.getRepository<Stocking, string>().GetAllQueryable()
                .Where(e => req.ProductId.ToList().Distinct().Contains(e.ProductId)).ToList();

            var getProductId = productList.Select(e => e.ProductId).Distinct().ToList();

            //fixme error response
            var getProductInStock = _context.getRepository<Product, string>().GetAllQueryable()
                .Where(e => getProductId.Contains(e.Id));

            var productData = new List<object>();

            foreach (var item in getProductInStock)
            {
                var product = new
                {
                    id = item.Id,
                    code = item.Code,
                    stocking = new List<string>(),
                    name = item.Name,
                    price = item.Price,
                    description = item.Description,
                    onhands = productList.Sum(e => e.Quantity)
                };

                productData.Add(product);
            }


            //check if list of product is incorrect
            var isEqual = getProductId.Count == req.ProductId.ToList().Distinct().Count();
            try
            {
                return Ok(new
                {
                    success = true,
                    statuscode = 200,
                    message = $"Successfully returned {(isEqual ? productList.Count : 0)} grouped stocking(s)",
                    data = isEqual
                        ? new
                        {
                            product = productData,
                        }
                        : (object) new List<string>()
                });
            }
            catch (Exception)
            {
                return BadRequest("Something gone wrong");
            }
        }

        //fixme error list of product code
        [HttpGet("GetByProductCodes")]
        public async Task<ActionResult> GetCodeGet(ListStockProductCode req)
        {
            decimal Quantity = 0;
            var ProductList = new List<Product>();
            var Stock = new List<Stocking>();
            Stock = null;

            var st = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();

            var map = _mapper.Map<List<StockResponse>>(st);

            if (req.ProductCodes != null)
            {
                foreach (var item in req.ProductCodes)
                {
                    var pro = _context.getRepository<Product, string>().GetAllQueryable().ToList()
                        .Where(e => e.Code == item);

                    if (pro != null)
                    {
                        foreach (var p in pro)
                        {
                            var s = _context.getRepository<Stocking, string>().GetAllQueryable().ToList()
                                .Where(e => e.ProductId == p.Id);

                            if (s != null)
                            {
                                return Ok(ExtenFunction.StockingResponse("stocking(s)"
                                        , s.Select(e => new
                                        {
                                            product = new
                                            {
                                                Id = e.product.Id,
                                                code = e.product.Code,
                                                stocking = Stock,
                                                price = e.product.Price,
                                                description = e.product.Description,
                                            },
                                            OnHands = map.Where(e => e.ProductId == p.Id).Sum(k => k.Quantity),
                                        }).ToList(), true, 200, true
                                    )
                                );
                            }
                        }
                    }
                }
            }

            return Ok(new
                {
                    success = true,
                    StatusCode = 200,
                    message = $"Success returned stocking",
                    data = Stock
                }
            );
        }

        [HttpPost("GetByProductCodes")]
        public async Task<ActionResult> GetCodeProduct(ListStockProductCode req)
        {
            decimal Quantity = 0;
            var ProductList = new List<Product>();
            var Stock = new List<Stocking>();
            Stock = null;

            var st = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
            var map = _mapper.Map<List<StockResponse>>(st);
            if (req.ProductCodes != null)
            {
                foreach (var item in req.ProductCodes)
                {
                    var pro = _context.getRepository<Product, string>().GetAllQueryable().ToList()
                        .Where(e => e.Code == item);

                    if (pro != null)
                    {
                        foreach (var p in pro)
                        {
                            var s = _context.getRepository<Stocking, string>().GetAllQueryable().ToList()
                                .Where(e => e.ProductId == p.Id);
                            if (s != null)
                            {
                                return Ok(ExtenFunction.StockingResponse("stocking(s)"
                                        , s.Select(e => new
                                        {
                                            product = new
                                            {
                                                id = e.product.Id,
                                                code = e.product.Code,
                                                stocking = Stock,
                                                price = e.product.Price,
                                                description = e.product.Description,
                                            },
                                            OnHands = map.Where(e => e.ProductId == p.Id).Sum(k => k.Quantity),
                                        }).ToList(), true, 200, true
                                    )
                                );
                            }
                        }
                    }
                }
            }

            //modified response
            return Ok(new
                {
                    success = true,
                    StatusCode = 200,
                    message = $"Success returned stocking",
                    data = ProductList.ToList()
                }
            );
        }

        [HttpGet("GetByWare")]
        public async Task<ActionResult> GetByWare(ListWareGet req)
        {
            decimal Quantity = 0;
            // var ProductList = new List<Product>();
            var WareList = new List<Ware>();
            var Stock = new List<Stocking>();
            Stock = null;
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
                            product = new
                            {
                                Id = s.product.Id,
                                code = s.product.Code,
                                stocking = Stock,
                                name = s.product.Name,
                                price = s.product.Price,
                                description = s.product.Description,
                            },
                            ware = s.Ware,
                            productid = s.ProductId,
                            wareId = s.WareId,
                            quantity = s.Quantity,
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
                            product = new
                            {
                                Id = s.product.Id,
                                code = s.product.Code,
                                stocking = Stock,
                                name = s.product.Name,
                                price = s.product.Price,
                                description = s.product.Description,
                            },
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
                            pproduct = new
                            {
                                Id = s.product.Id,
                                code = s.product.Code,
                                stocking = Stock,
                                name = s.product.Name,
                                price = s.product.Price,
                                description = s.product.Description,
                            },
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
                            product = new
                            {
                                Id = s.product.Id,
                                code = s.product.Code,
                                stocking = Stock,
                                name = s.product.Name,
                                price = s.product.Price,
                                description = s.product.Description,
                            },
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

        //passed  
        [HttpPost("Create")]
        public async Task<ActionResult> CreateExistProduct(ListStockingCreateReq req)
        {
            if (!req.commands.Any()) return BadRequest("Request body is empty");

            var stockList = new List<Stocking>();

            foreach (var reqItem in req.commands)
            {
                var ware = _context.getRepository<Ware, string>().GetAllQueryable().ToList()
                    .Where(e => e.Code == reqItem.WareCode);

                var product = _context.getRepository<Product, string>().GetAllQueryable().ToList()
                    .Where(e => e.Code == reqItem.ProductCode);

                // Once req must has two require data
                if (ware.Any() && product.Any())
                {
                    var stock = _mapper.Map<Stocking>(reqItem);
                    stockList.Add(stock);
                }
            }

            // check each request is correct
            if (stockList.Count != req.commands.ToList().Count)
                return Ok(ExtenFunction.ResponseDefault<StockingCreateReq>(
                    "Stock",
                    null,
                    false,
                    -201,
                    false,
                    $"No stocking created"
                ));

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Stocking, string>().CreateBatchAsync(stockList);

                _context.Commit();
                return Ok(ExtenFunction.ResponseDefault(
                    "Stocking",
                    stockList.Select(e => e.Id).ToList(),
                    true,
                    201,
                    true,
                    $"Successfully created {stockList.Count} stocking"
                ));
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

            var stockReq = new List<StockUpdateReq>();
            foreach (var item in req.command)
            {
                stockReq.Add(item);
            }

            if (StockList.Count > 0)
            {
                foreach (var stock in StockList)
                {
                    foreach (var stReq in stockReq.Where(stReq => stock.Id == stReq.Id))
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


        [HttpDelete]
        public async Task<ActionResult> DeleteStock(IdReq req)
        {
            //fixme modify condition 
            if (!req.Id.Any())
                return BadRequest(ExtenFunction.ResponseDefault<StockUpdateReq>(
                    "Stock",
                    null,
                    false,
                    400,
                    false,
                    $"No given ids to delete stockings"
                ));

            //fixme get product list without using loop

            var stockList = _context.getRepository<Stocking, string>().GetAllQueryable()
                .Where(e => req.Id.Contains(e.Id)).ToList();

            if (req.Id.Count != stockList.Count)
                return BadRequest(ExtenFunction.ResponseDefault<ProductUpdateReq>(
                    "Stocking",
                    null,
                    false,
                    304,
                    false,
                    $"No stocking deleted"
                ));

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Stocking, string>().DeleteBatchAsync(stockList);
                _context.Commit();
                return Ok(new
                {
                    success = true,
                    StatusCode(200).StatusCode,
                    message = $"Successfully deleted {stockList.Count} Stocking",
                    data = stockList.Select(e => e.Id).ToList()
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest($"Something gone wrong");
            }
        }

        [HttpPost("newproduct")]
        public async Task<ActionResult> CreateNewProduct(ListStockCreateProdReq req)
        {
            if (!req.commands.Any()) return BadRequest($"Must be filled request body");

            var stockList = new List<Stocking>();
            var dataResult = new List<string>();
            var notWareId = new List<string>();

            foreach (var item in req.commands)
            {
                // select ware with code
                var wareTarget = _context.getRepository<Ware, string>().GetAllQueryable()
                    .Where(e => item.WareCode.Contains(e.Code));


                if (wareTarget.Any())
                {
                    //create product
                    var product = new ProductCreateReq()
                    {
                        Code = item.ProductCode,
                        Name = item.ProductName,
                        Price = item.Price,
                        Description = item.ProductDescription
                    };

                    var mapProduct = _mapper.Map<Product>(product);
                    await _context.getRepository<Product, string>().CreateAsync(mapProduct);

                    var stock = _mapper.Map<Stocking>(item);

                    if (stock is not null)
                    {
                        stockList.Add(stock);
                        dataResult.Add($"{stock.Id} > {stock.WareId}({stock.ProductId}, {stock.Quantity})");
                    }
                }
                else
                {
                    notWareId.Add(item.WareCode);
                }
            }

            if (stockList.Count != req.commands.ToList().Count)
                return BadRequest(new
                {
                    success = false,
                    StatusCode(404).StatusCode,
                    message = $"No such ware match to request founded {notWareId.Count}",
                    data = notWareId
                });

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Stocking, string>().CreateBatchAsync(stockList);
                _context.Commit();
                return Ok(new
                {
                    success = true,
                    StatusCode(200).StatusCode,
                    message = $"Successfully created a new product with stocking: Checking inner results",
                    data = dataResult
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest($"Something gone wrong");
            }
        }

        [HttpPut("transfer")]
        public async Task<ActionResult> TransferQuantity(ListStockTransferQtyReq req)
        {
            if (!req.commands.Any()) return BadRequest("Empty body");

            var StockTransferList = new List<Stocking>();
            var AddNewStock = new List<Stocking>();

            foreach (var item in req.commands.ToList())
            {
                var sourceWare = _context.getRepository<Stocking, string>().GetAllQueryable()
                    .Where(e => item.productId.Contains(e.ProductId) && item.sourceWareId.Contains(e.WareId)).ToList();

                var targetWare = _context.getRepository<Stocking, string>().GetAllQueryable()
                    .Where(e => item.productId.Contains(e.ProductId) && item.targetWareId.Contains(e.WareId)).ToList();

                if (sourceWare.Any() && targetWare.Any())
                {
                    foreach (var source in sourceWare)
                    {
                        foreach (var target in targetWare)
                        {
                            if (source.Quantity >= item.quantity)
                            {
                                source.Quantity -= item.quantity;
                                target.Quantity += item.quantity;
                            }
                        }
                    }

                    StockTransferList.AddRange(sourceWare);
                    StockTransferList.AddRange(targetWare);
                }

                else if (item.productId != null && item.targetWareId != null)
                {
                    var getProCode = await _context.getRepository<Product, string>().GetAsync(item.productId);
                    var getWareCode = await _context.getRepository<Ware, string>().GetAsync(item.targetWareId);

                    try
                    {
                        var newStock = new StockingCreateReq()
                        {
                            ProductCode = getProCode.Code,
                            WareCode = getWareCode.Code,
                            Quantity = item.quantity
                        };
                        if (newStock != null) AddNewStock.Add(_mapper.Map<Stocking>(newStock));
                    }
                    catch (Exception)
                    {
                        return BadRequest("Something gone wrong");
                    }
                }
            }

            //fixme create response 
            return Ok(new
            {
                data = StockTransferList,
                stockCount = StockTransferList.Count,
                newStock = AddNewStock
            });
        }
    }
}