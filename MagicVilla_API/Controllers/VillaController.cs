﻿using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {

        private readonly ILogger<VillaController> _logger;
        public VillaController(ILogger<VillaController> logger)
        {

            _logger = logger;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener las Villas");
            return Ok(VillaStore.villaList);
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)] //Con esto documentamos los codigos de estado
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
                _logger.LogError("Error al traer Villa con Id " + id);
                return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            if (villa is null)
                return NotFound();
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Validaciones personalizadas
            if (VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower() == villaDto.Name.ToLower()) is not null)
            {
                ModelState.AddModelError("NombreExiste", "La Villa con ese nombre ya existe!");
                return BadRequest(ModelState);
            }

            if (villaDto is null)
                return BadRequest(villaDto);

            if (villaDto.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).First().Id + 1;
            VillaStore.villaList.Add(villaDto);

            //Nos retorna la ruta del id que se le esta pasando
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
        }

        //Siempre que se trabaje con delete se debe retornar un NoContent
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa is null)
                return NotFound();

            VillaStore.villaList.Remove(villa);
            return NoContent();

        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int id,[FromBody] VillaDto villaDto)
        {
            if(villaDto is null || id != villaDto.Id)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa is null)
                return NotFound();

            villa.Name = villaDto.Name;
            villa.Ocupantes = villaDto.Ocupantes;
            villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            return NoContent();
        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if (patchDto is null || id == 0)
                return BadRequest();

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa is null)
                return NotFound();

            patchDto.ApplyTo(villa, ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent();

        }
    }
}
