using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profiles.Contracts.DTOs.Patient;
using Profiles.Contracts.Pagination;
using Profiles.Services.Abstractions;
using System.Net.Mime;

namespace Profiles.Presentation.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientsService _patientsService;

    public PatientsController(IPatientsService patientsService) =>
        _patientsService = patientsService;

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllPatients([FromQuery] PatientsQueryParameters queryParameters)
    {
        var getPatientsResult = await _patientsService.GetAllPatientsAsync(queryParameters);

        return getPatientsResult.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [HttpGet("{id}", Name = "GetPatientById")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPatientById(Guid id)
    {
        var getPatientResult = await _patientsService.GetPatientByIdAsync(id);

        return getPatientResult.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPatient(IValidator<PatientCreateDTO> validator,
        [FromBody] PatientCreateDTO newPatient)
    {
        var validationResult = validator.Validate(newPatient);

        if (validationResult.IsValid)
        {
            var createdPatientGuid = await _patientsService.CreatePatientAsync(newPatient);

            return CreatedAtAction("GetPatientById", new { id = createdPatientGuid }, createdPatientGuid);
        }

        return BadRequest(validationResult.ToDictionary());
    }

    [HttpPut("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePatient(IValidator<PatientUpdateDTO> validator,
        [FromRoute] Guid id, [FromBody] PatientUpdateDTO patientUpdate)
    {
        var validationResult = validator.Validate(patientUpdate);

        if (validationResult.IsValid)
        {
            var updatePatientResult =
                await _patientsService.UpdatePatientAsync(id, patientUpdate);

            return updatePatientResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
        }

        return BadRequest(validationResult.ToDictionary());
    }

    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePatient([FromRoute] Guid id)
    {
        var deletePatientResult = await _patientsService.DeletePatientAsync(id);

        return deletePatientResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
    }
}
