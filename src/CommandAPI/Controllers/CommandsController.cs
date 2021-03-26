using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo repository;
        private readonly IMapper mapper;
        public CommandsController(ICommandAPIRepo repo, IMapper map)
        {
            repository = repo;
            mapper = map;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands() 
        {
            var commands = repository.GetAllCommands();
            return Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }
        [HttpGet("{id}")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var command = repository.GetCommandById(id);
            if(command == null)
                return NotFound();
            return Ok(mapper.Map<CommandReadDto>(command));
        }
    }
}