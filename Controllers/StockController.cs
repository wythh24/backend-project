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
                        Product=new
                        {
                            Id=e.product.Id,
                            code=e.product.Code,
                            name=e.product.Name,
                            price=e.product.Price,
                            description=e.product.Description
                            
                        },
                        Ware=new
                        {
                            Id=e.Ware.Id,
                            code=e.Ware.Code,
                            name=e.Ware.Name,
                            description=e.Ware.Description
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
                        Product=new
                        {
                            Id=e.product.Id,
                            code=e.product.Code,
                            name=e.product.Name,
                            price=e.product.Price,
                            description=e.product.Description
                            
                        },
                        Ware=new
                        {
                            Id=e.Ware.Id,
                            code=e.Ware.Code,
                            name=e.Ware.Name,
                            description=e.Ware.Description
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
                        Product=new
                        {
                            Id=e.product.Id,
                            code=e.product.Code,
                            name=e.product.Name,
                            price=e.product.Price,
                            description=e.product.Description
                            
                        },
                        Ware=new
                        {
                            Id=e.Ware.Id,
                            code=e.Ware.Code,
                            name=e.Ware.Name,
                            description=e.Ware.Description
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
                        Id = e.Id,
                        Product=new
                        {
                            Id=e.product.Id,
                            code=e.product.Code,
                            name=e.product.Name,
                            price=e.product.Price,
                            description=e.product.Description
                            
                        },
                        Ware=new
                        {
                            Id=e.Ware.Id,
                            code=e.Ware.Code,
                            name=e.Ware.Name,
                            description=e.Ware.Description
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
        
        [HttpGet("GetByProduct")]
        public async Task<ActionResult> GetByProduct(ListStockCreateReq req)
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
        [HttpPost("GetByProduct")]
        public async Task<ActionResult> GetPostProduct(ListStockCreateReq req)
        {
            // if (!req.ProductId.Any()) return BadRequest("");
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
                        Stock = null;
                    }
                    else ProductList = null;
                }
            }
            else ProductList = null;

            return Ok(ProductList == null
                ? ExtenFunction.StockingResponse("Stocking", 
                    Stock.ToList()
                    , false, 404, false, null)
                : ExtenFunction.StockingResponse("Stocking", 
                    ProductList.Select(e=> new
                    {
                        Id=e.Id,
                        Code=e.Code,
                        name=e.Name,
                        price=e.Price,
                        Stocking=Stock,
                        OnHands=map.Where(s=> s.ProductId==e.Id).Sum(s=> s.Quantity),
                        description=e.Description,
                    }).ToList()
                    , false, 302, true, null)
            );
        }
        [HttpGet("GetByProductCodes")]
        public async Task<ActionResult> GetCodeGet(ListCodeProduct req)
        {
            decimal Quantity = 0;
            var ProductList = new List<Product>();
            var Stock = new List<Stocking>();
            Stock = null;
            var st = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
            var map = _mapper.Map<List<StockResponse>>(st);
            if (req.productcodes != null)
            {
                foreach (var item in req.productcodes)
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
                                            product=new
                                            {
                                                Id=e.product.Id,
                                                code=e.product.Code,
                                                stocking=Stock,
                                                price=e.product.Price,
                                                description=e.product.Description,
                                            },
                                            OnHands=map.Where(e=>e.ProductId==p.Id).Sum(k=>k.Quantity),
                                        }).ToList(), true, 200, true
                                    )
                                );
                            }

                            else ProductList = null;
                        }
                    }
                    ProductList = null;
                }
            }
            else ProductList = null;

            return Ok((new
                {
                    success=true,
                    StatusCode=StatusCode(200),
                    message=$"Success returned {ProductList.Count} stocking",
                    data=ProductList.Select(e => new
                    {
                        Id=e.Id,
                        Code=e.Code,
                        Stock=Stock,
                        Name=e.Name,
                        price=e.Price,
                        description=e.Description,
                        OnHands=map.Where(s=> s.ProductId==e.Id).Sum(s=> s.Quantity),
                    }
                    ).ToList(),
                })
            );
        }
        [HttpPost("GetByProductCodes")]
        public async Task<ActionResult> GetCodeProduct(ListCodeProduct req)
        {
            decimal Quantity = 0;
            var ProductList = new List<Product>();
            var Stock = new List<Stocking>();
            Stock = null;
            var st = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
            var map = _mapper.Map<List<StockResponse>>(st);
            if (req.productcodes != null)
            {
                foreach (var item in req.productcodes)
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
                                            product=new
                                            {
                                                Id=e.product.Id,
                                                code=e.product.Code,
                                                stocking=Stock,
                                                price=e.product.Price,
                                                description=e.product.Description,
                                            },
                                            OnHands=map.Where(e=>e.ProductId==p.Id).Sum(k=>k.Quantity),
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
              success=true,
              StatusCode(200).StatusCode,
              message=$"{ProductList.ToList().Count}",
              data=ProductList.ToList()
            });
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
                    if (w != null) {WareList.Add(w);}
                    else WareList = null;
                }
            }
            else WareList = null;

            return Ok(WareList == null
                ? ExtenFunction.StockingResponse("Ware", 
                    WareList.Select(e=> new
                    {
                        Id=e.Id,
                        Code=e.Code,
                        Stock=e.Stockings.Select(s=> new
                        {
                            Id=s.Id,
                            product=new
                            {
                                Id=s.product.Id,
                                code=s.product.Code,
                                stocking=Stock,
                                name=s.product.Name,
                                price=s.product.Price,
                                description=s.product.Description,
                            },
                            Ware=s.Ware,
                            productid=s.ProductId,
                            wareId=s.WareId,
                            Quantity=s.Quantity,
                            documentDate=s.DocumentDate,
                            postingDate=s.PostingDate
                        }).ToList(),
                        name=e.Name,
                        description=e.Description
                    }).ToList()
                    , false, 404, false, null)
                : ExtenFunction.StockingResponse("Ware", 
                    WareList.Select(e=> new
                    {
                        Id=e.Id,
                        Code=e.Code,
                        Stock=e.Stockings.Select(s=> new
                        {
                            Id=s.Id,
                            product=new
                            {
                                Id=s.product.Id,
                                code=s.product.Code,
                                stocking=Stock,
                                name=s.product.Name,
                                price=s.product.Price,
                                description=s.product.Description,
                            },
                            Ware=s.Ware,
                            productid=s.ProductId,
                            wareId=s.WareId,
                            Quantity=s.Quantity,
                            documentDate=s.DocumentDate,
                            postingDate=s.PostingDate
                        }).ToList(),
                        name=e.Name,
                        description=e.Description,
                        OnHands=map.Sum(s=> s.Quantity)
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
                    if (w != null) {WareList.Add(w);}
                    else WareList = null;
                }
            }
            else WareList = null;

            return Ok(WareList == null
                ? ExtenFunction.StockingResponse("Ware", 
                    WareList.Select(e=> new
                    {
                        Id=e.Id,
                        Code=e.Code,
                        Stock=e.Stockings.Select(s=> new
                        {
                            Id=s.Id,
                            pproduct=new
                            {
                                Id=s.product.Id,
                                code=s.product.Code,
                                stocking=Stock,
                                name=s.product.Name,
                                price=s.product.Price,
                                description=s.product.Description,
                            },
                            Ware=s.Ware,
                            productid=s.ProductId,
                            wareId=s.WareId,
                            Quantity=s.Quantity,
                            documentDate=s.DocumentDate,
                            postingDate=s.PostingDate
                        }).ToList(),
                        name=e.Name,
                        description=e.Description
                    }).ToList()
                    , false, 404, false, null)
                : ExtenFunction.StockingResponse("Ware", 
                    WareList.Select(e=> new
                    {
                        Id=e.Id,
                        Code=e.Code,
                        Stock=e.Stockings.Select(s=> new
                        {
                            Id=s.Id,
                            product=new
                            {
                                Id=s.product.Id,
                                code=s.product.Code,
                                stocking=Stock,
                                name=s.product.Name,
                                price=s.product.Price,
                                description=s.product.Description,
                            },
                            Ware=s.Ware,
                            productid=s.ProductId,
                            wareId=s.WareId,
                            Quantity=s.Quantity,
                            documentDate=s.DocumentDate,
                            postingDate=s.PostingDate
                        }).ToList(),
                        name=e.Name,
                        description=e.Description,
                        OnHands=map.Sum(s=> s.Quantity)
                    }).ToList()
                    , false, 302, true, null)
            );
        }
        //fixme
        [HttpPost("Create")]
        public async Task<ActionResult<List<Stocking>>> Create(ListCreate req)
        {
            if (!req.command.Any()) return BadRequest();

            var stockList = new List<Stocking>();
            
            foreach (var ree in req.command)
            {
                var ware = _context.getRepository<Ware, string>().GetAllQueryable().ToList()
                    .Where(e => e.Code == ree.WareCode);
                
                var product = _context.getRepository<Product, string>().GetAllQueryable().ToList()
                    .Where(e => e.Code == ree.ProductCode);

                if (ware.Any() && product.Any())
                {
                }

            }
       
            _context.BeginTransaction();
            try
            {
                // await _context.getRepository<Product, string>().CreateBatchAsync(StockList);
                _context.Commit();
                return Ok("in try");
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
                return Ok(ExtenFunction.ResponseDefault("Stocking",StockList));
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest(ExtenFunction.ResponseDefault("Stocking",StockList,false,404));
            }
        }
        
        
        
        [HttpDelete]
        public async Task<ActionResult> DeleteStock(IdReq id)
        {
            var StockList = new List<Stocking>();
            if (id == null) return BadRequest(new
            {
                succes=false,
                StatusCode=400,
                message="No giving Ids to delete stocking",
                data=StockList=null,
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