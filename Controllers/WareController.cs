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
        [HttpGet]
        public async Task<ActionResult> GetWare()
        {
            var list = new List<Ware>();
            return Ok(ExtenFunction.ResponseDefault("Ware", list));
        }
    }
}