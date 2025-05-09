﻿using Application.Helpers;
using Application.Models.ToDos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    //[Route("api/[controller]/[Action]")]
    [ApiController]
    public class ToDoAPISampleController : ControllerBase
    {
        [HttpPost]

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status200OK,Type =typeof(ToDoDto))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
       // [ProducesResponseType(StatusCodes.Status404NotFound)]

        [Consumes(MediaTypeNames.Text.Plain)]
        //[Consumes(MediaTypeNames.Application.Json)]
        //[Consumes(MediaTypeNames.Application.Xml)]
       // [Consumes(MediaTypeNames.Application.Xml,MediaTypeNames.Application.Json)]

        //[Produces(MediaTypeNames.Application.Json)]
        //[Produces(MediaTypeNames.Text.Plain)]
        //[Produces(MediaTypeNames.Application.Xml)]
        public IActionResult CreateToDo(ToDoDto dto)
        {
            return Ok(dto);

            // return NotFound();
            // return BadRequest("bad req");
            // return Unauthorized();
            // return NoContent();
           // return Forbid();
        }
    }
}
