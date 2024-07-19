using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profiles.Contracts.DTOs.Doctor;
using Profiles.Contracts.Pagination;
using Profiles.Services.Abstractions;
using System.Net.Mime;

namespace Profiles.Presentation.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorsService _doctorService;

    public DoctorsController(IDoctorsService doctorService) =>
        _doctorService = doctorService;

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDoctors([FromQuery] DoctorsQueryParameters parameters)
    {
        var getDoctorsResult = await _doctorService.GetAllDoctorsAsync(parameters);

        return getDoctorsResult.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [HttpGet("{id}", Name = "GetDoctorById")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var getDoctorResult = await _doctorService.GetDoctorByIdAsync(id);

        return getDoctorResult.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddDoctor(IValidator<DoctorCreateDTO> validator, 
        [FromBody] DoctorCreateDTO newDoctor)
    {
        var validationResult = validator.Validate(newDoctor);

        if (validationResult.IsValid)
        {
            var createdDoctor = await _doctorService.CreateDoctorAsync(newDoctor);

            return CreatedAtRoute("GetDoctorById", new { createdDoctor.Id }, createdDoctor);
        }

        return BadRequest(validationResult.ToDictionary());
    }

    [HttpPut("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDoctor(IValidator<DoctorUpdateDTO> validator,
        [FromRoute] Guid id, [FromBody] DoctorUpdateDTO editedDotctor)
    {
        var validationResult = validator.Validate(editedDotctor);

        if (validationResult.IsValid)
        {
            var updateDoctorResult = await _doctorService.UpdateDoctorAsync(id, editedDotctor);

            return updateDoctorResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
        }

        return BadRequest(validationResult.ToDictionary());
    }


    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDoctor([FromRoute] Guid id)
    {
        var deleteDoctorResult = await _doctorService.DeleteDoctorAsync(id);

        return deleteDoctorResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
    }
}
