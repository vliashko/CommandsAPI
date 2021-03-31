using System.Collections.Generic;
using AutoMapper;
using CommandAPI.Data;
using CommandAPI.Dtos;
using CommandAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpGet("{id}", Name = "GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var command = repository.GetCommandById(id);
            if(command == null)
                return NotFound();
            return Ok(mapper.Map<CommandReadDto>(command));
        }
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
        {
            var commandModel = mapper.Map<Command>(commandCreateDto);
            repository.CreateCommand(commandModel);
            repository.SaveChanges();

            var commandReadDto = mapper.Map<CommandReadDto>(commandModel);
            return CreatedAtRoute(nameof(GetCommandById), new { Id = commandReadDto.Id }, commandReadDto);
        }
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandFromRepo = repository.GetCommandById(id);
            if(commandFromRepo == null)
            {
                return NotFound();
            }
            mapper.Map(commandUpdateDto, commandFromRepo);
            repository.UpdateCommand(commandFromRepo);
            repository.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandFromRepo = repository.GetCommandById(id);
            if(commandFromRepo == null)
            {
                return NotFound();
            }
            var commandToPatch = mapper.Map<CommandUpdateDto>(commandFromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);
            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }
            mapper.Map(commandToPatch, commandFromRepo);
            repository.UpdateCommand(commandFromRepo);
            repository.SaveChanges();
            return NoContent();
        }
    }
}