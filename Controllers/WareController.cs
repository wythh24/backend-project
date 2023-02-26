using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using productstockingv1.ExtensionFunction;
using productstockingv1.Interfaces;
using productstockingv1.models;
using productstockingv1.Models.Request;

namespace productstockingv1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WareController : ControllerBase
    {
        private readonly IUnitOfWork _context;
        private readonly IMapper _mapper;

        public WareController(IUnitOfWork context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //get all or specific ware
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll(IdReq id)
        {
            var Warelist = new List<Ware>();
            if (id.Id != null && id.Id.ToString() != "string" && !string.IsNullOrEmpty(id.Id.ToString()))
            {
                foreach (var item in id.Id)
                {
                    var ware = await _context.getRepository<Ware, string>().GetAsync(item);
                    if(ware!=null) Warelist.Add(ware);
                }

                return Ok(ExtenFunction.ResponseDefault("ware", Warelist));
            }
            else
            {
                var all = _context.getRepository<Ware, string>().GetAllQueryable().ToList();
                var result = _mapper.Map<List<WareResponse>>(all);
                return Ok(ExtenFunction.ResponseDefault("ware", result, false, 400));
            }
        }
        [HttpPost("getAll")]
        public async Task<ActionResult> GetWare(IdReq id = null)
        {
            var WareList = new List<Ware>();
            if (id.Id != null && id.Id.ToString() != "string")
            {
                foreach (var item in id.Id)
                {
                    var p = await _context.getRepository<Ware, string>().GetAsync(item);
                    if (p != null) WareList.Add(p);
                }
            }

            else if (id.Id == null || string.IsNullOrEmpty(id.Id.ToString()) || id.Id.ToString() == "string")
            {
                var all = _context.getRepository<Ware, string>().GetAllQueryable().ToList();

                var result = _mapper.Map<List<WareResponse>>(all);

                return Ok(ExtenFunction.ResponseDefault("Product", result, false));
            }

            return Ok(ExtenFunction.ResponseDefault("Product", WareList));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetWare(string id)
        {
            var WareList = new List<Ware>();

            if (id != null)
            {
                var ware = await _context.getRepository<Ware, string>().GetAsync(id);
                if (ware != null) WareList.Add(ware);
            }

            //passed
            return Ok(WareList.Count > 0
                ? ExtenFunction.ResponseDefault("Product", WareList, false, 302)
                : ExtenFunction.ResponseDefault("Product", WareList, false)
            );
        }
        [HttpGet("GetById")]
        public async Task<ActionResult> GetByBodyId([FromBody] GetT req)
        {
            var WareList = new List<Ware>();

            if (req.Id != null)
            {
                var ware = await _context.getRepository<Ware, string>().GetAsync(req.Id);
                if (ware != null) WareList.Add(ware);
            }
            return Ok(WareList.Count > 0
                ? ExtenFunction.ResponseDefault("Product", WareList, false, 302)
                : ExtenFunction.ResponseDefault("Product", WareList, false));
        }
        [HttpPost]
        public async Task<ActionResult<Ware>> CreateWare(ListWareCreateReq req)
        {
            //change to Any()
            if (!req.command.Any()) return BadRequest("Request must be filled");

            var WareList = req.command.Select(item => _mapper.Map<Ware>(item)).ToList();

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Ware, string>().CreateBatchAsync(WareList);
                _context.Commit();
                return Ok(new
                {
                    success = true,
                    statusCode = 200,
                    message = $"Successfully created {WareList.Count} products",
                    data = WareList.Select(e => e.Id).ToList()
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return Conflict("Something gone wrong!");
            }
        }
        
        [HttpPut]
        public async Task<ActionResult> UpdateWare(ListWareUpdateReq req)
        {
            if (!req.command.Any())
                return BadRequest("Not found Request");

            var WareList = new List<Ware>();

            if (req.command.Any())
            {
                foreach (var item in req.command)
                {
                    var ware = await _context.getRepository<Ware, string>().GetAsync(item.Id);
                    WareList.Add(ware);
                }
            }

            var WareReq = new List<WareUpdateReq>();
            foreach (var item in req.command)
            {
                WareReq.Add(item);
            }

            if (WareList.Count > 0)
            {
                foreach (var ware in WareList)
                {
                    foreach (var wreq in WareReq.Where(wreq => ware.Id == wreq.Id))
                    {
                        ware.Name = wreq.name;
                        ware.Description = wreq.description;
                    }
                }
            }


            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Ware, string>().UpdateBatchAsync(WareList);
                _context.Commit();
                return Ok($"Product update {WareList.Count}");
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest("Update Product was false");
            }
        }
        
        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(IdReq req)
        {
            if (req == null) return BadRequest("Id must be filled");

            var wareList = new List<Ware>();

            if (req != null)
            {
                foreach (var item in req.Id)
                {
                    var ware = await _context.getRepository<Ware, string>().GetAsync(item);
                    if (ware != null) wareList.Add(ware);
                }
            }

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Ware, string>().DeleteBatchAsync(wareList);
                _context.Commit();
                return Ok(new
                {
                    success = true,
                    StatusCode(200).StatusCode,
                    message = $"Successfully deleted {wareList.Count} products",
                    data = wareList.Select(e => e.Id).ToList()
                });
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest($"Something gone wrong");
            }
        }
    }
}