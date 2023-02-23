using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<ActionResult> Get(IdReq id)
        {
            
            // var StockList = new List<Stocking>();
            // if (id.Id != null && id.Id.ToString() != "string")
            // {
            //     foreach (var item in id.Id)
            //     {
            //         var st = await _context.getRepository<Stocking, string>().GetAsync(item);
            //         if(st!=null) StockList.Add(st);
            //     }
            //
            //     return Ok(ExtenFunction.ResponseDefault("Stocking",StockList.Select(e=> e.Id).ToList()));
            // }
            // else
            // {
                return Ok("test");
                // var all = _context.getRepository<Stocking, string>().GetAllQueryable().ToList();
                // var result = _mapper.Map<List<StockResponse>>(all);
                // return Ok(ExtenFunction.ResponseDefault("Stocking", result.Select(e=> e.Id).ToList(), false));
            // }
        }
    }
}