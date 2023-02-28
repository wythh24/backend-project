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

        //modified
        //get all or specific ware
        [HttpGet("test")]
        public async Task<ActionResult> GetAll([FromBody] IdReq reqIdList)
        {
            var Warelist = new List<Ware>();
            //modified condition
            if (reqIdList.Id != null)
            {
                foreach (var item in reqIdList.Id)
                {
                    var ware = await _context.getRepository<Ware, string>().GetAsync(item);
                    if (ware != null) Warelist.Add(ware);
                }

                return Ok(Warelist.Count > 0
                    ? ExtenFunction.ResponseDefault("Ware", Warelist)
                    : ExtenFunction.ResponseDefault("Ware", Warelist, true, 400, true,
                        "Ids of wares are required to perform querying"));
            }

            Warelist = _context.getRepository<Ware, string>().GetAllQueryable().ToList();
            //var resultList = _mapper.Map<List<WareResponse>>(Warelist);

            return Ok(ExtenFunction.ResponseDefault(
                "Ware",
                _mapper.Map<List<WareResponse>>(Warelist),
                true,
                300
            ));
        }

        //get List Ware or specific (POST)
        [HttpPost("getAll")]
        public async Task<ActionResult> GetWareList(IdReq reqIdList)
        {
            var Warelist = new List<Ware>();
            //modified condition
            if (reqIdList.Id != null)
            {
                foreach (var item in reqIdList.Id)
                {
                    var ware = await _context.getRepository<Ware, string>().GetAsync(item);
                    if (ware != null) Warelist.Add(ware);
                }

                return Ok(Warelist.Count > 0
                    ? ExtenFunction.ResponseDefault("Ware", Warelist)
                    : ExtenFunction.ResponseDefault(
                        "Ware",
                        Warelist,
                        true,
                        400,
                        true,
                        "Ids of wares are required to perform querying"
                    )
                );
            }

            Warelist = _context.getRepository<Ware, string>().GetAllQueryable().ToList();
            // var resultList = _mapper.Map<List<WareResponse>>(Warelist);

            return Ok(ExtenFunction.ResponseDefault(
                "Ware",
                _mapper.Map<List<WareResponse>>(Warelist),
                true,
                300
            ));
        }

        //get by id (query)
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
                ? ExtenFunction.ResponseDefault("Ware", WareList, false, 302)
                : ExtenFunction.ResponseNotFound("Ware", WareList, false, 404, true,
                    $"Ware with id {id} not found")
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
                ? ExtenFunction.ResponseDefault("Ware", WareList, false, 302)
                : ExtenFunction.ResponseDefault("Ware", WareList, false, 404, true,
                    $"Ware with id {req.Id} not found"));
        }

        // modified add condition check exist ware by ware code
        [HttpPost]
        public async Task<ActionResult<Ware>> CreateWare(ListWareCreateReq req)
        {
            //change to Any()
            if (!req.command.Any()) return BadRequest("Request must be filled");

            //select all code from request
            var wareListId = req.command.Select(e => e.Code).Distinct().ToList();

            //get ware from code request
            var getWareList = _context.getRepository<Ware, string>().GetAllQueryable()
                .Where(e => wareListId.Contains(e.Code)).ToList();

            if (getWareList.Count > 0)
                return Ok(ExtenFunction.ResponseDefault<WareCreateReq>(
                        "Ware",
                        null,
                        false,
                        -201,
                        true,
                        $"No wares created"
                    )
                );

            var WareList = req.command.Select(item => _mapper.Map<Ware>(item)).ToList();

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Ware, string>().CreateBatchAsync(WareList);
                _context.Commit();

                return Ok(ExtenFunction.ResponseDefault(
                    "Ware",
                    WareList.Select(e => e.Id).ToList(),
                    true,
                    201,
                    true,
                    $"Successfully created {WareList.Count} Wares")
                );
            }
            catch (Exception)
            {
                _context.RollBack();
                return Conflict("Something gone wrong!");
            }
        }

        //fixme test update ware
        [HttpPut]
        public async Task<ActionResult> UpdateWare(ListWareUpdateReq req)
        {
            if (!req.command.Any()) return BadRequest("Not found Request");

            var WareList = new List<Ware>();

            if (req.command.Any())
            {
                foreach (var item in req.command)
                {
                    var ware = await _context.getRepository<Ware, string>().GetAsync(item.Id);
                    if (ware != null) WareList.Add(ware);
                }
            }

            var WareReq = req.command.ToList();

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

            if (WareList.Count < WareReq.Count)
                return BadRequest(ExtenFunction.ResponseDefault<WareUpdateReq>(
                    "Ware",
                    null,
                    false,
                    304,
                    true,
                    $"No wares updated"
                ));

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Ware, string>().UpdateBatchAsync(WareList);

                _context.Commit();

                return Ok(ExtenFunction.ResponseDefault(
                    "Ware",
                    WareList.Select(e => e.Id).ToList(),
                    true,
                    200,
                    true,
                    $"Successfully updated {WareList.Count} Wares"
                ));
            }
            catch (Exception)
            {
                _context.RollBack();

                return BadRequest($"Something gone wrong");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(IdReq req)
        {
            if (!req.Id.Any()) return BadRequest("Id must be filled");

            var wareList = new List<Ware>();

            foreach (var item in req.Id)
            {
                var ware = await _context.getRepository<Ware, string>().GetAsync(item);
                if (ware != null) wareList.Add(ware);
            }

            if (req.Id.Count > wareList.Count)
                return Ok(new
                {
                    id = req.Id.ToList()
                });

            _context.BeginTransaction();
            try
            {
                await _context.getRepository<Ware, string>().DeleteBatchAsync(wareList);
                _context.Commit();
                return Ok(ExtenFunction.ResponseDefault(
                    "Ware",
                    wareList.Select(e => e.Id).ToList(),
                    true,
                    200,
                    true,
                    $"Successfully deleted {wareList.Count} Wares"
                ));
            }
            catch (Exception)
            {
                _context.RollBack();
                return BadRequest($"Something gone wrong");
            }
        }
    }
}