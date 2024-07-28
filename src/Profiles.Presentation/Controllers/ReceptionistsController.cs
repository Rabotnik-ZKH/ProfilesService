using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Profiles.Contracts.DTOs.Receptionist;
using Profiles.Contracts.Pagination;
using Profiles.Services.Abstractions;
using System.Net.Mime;

namespace Profiles.Presentation.Controllers;

[ApiController]
[Route("api/receptionists")]
public class ReceptionistsController : ControllerBase
{
    private readonly IReceptionistsService _receptionistsService;

    public ReceptionistsController(IReceptionistsService receptionistsService) =>
        _receptionistsService = receptionistsService;

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllReceptionists([FromQuery] ReceptionistsQueryParameters parameters)
    {
        var getReceptionistsResult = await _receptionistsService.GetAllReceptionistsAsync(parameters);

        return getReceptionistsResult.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [HttpGet("{id}", Name = "GetReceptionistById")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var getReceptionistResult = await _receptionistsService.GetReceptionistByIdAsync(id);

        return getReceptionistResult.Match<IActionResult>(Ok, notFound => NotFound());
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddReceptionist(IValidator<ReceptionistCreateDTO> validator, 
        [FromBody] ReceptionistCreateDTO newRecepionist)
    {
        var validationResult = validator.Validate(newRecepionist);

        if (validationResult.IsValid)
        {
            var createdReceptionistId = await _receptionistsService.CreateReceptionistAsync(newRecepionist);

            return CreatedAtRoute("GetReceptionistById", new { createdReceptionistId }, createdReceptionistId);
        }

        return BadRequest(validationResult.ToDictionary());
    }

    [HttpPut("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReceptionist(IValidator<ReceptionistUpdateDTO> validator,
        [FromRoute] Guid id, [FromBody] ReceptionistUpdateDTO updatedReceptionist)
    {
        var validationResult = validator.Validate(updatedReceptionist);

        if (validationResult.IsValid)
        {
            var updateReceptionistResult =
                await _receptionistsService.UpdateReceptionistAsync(id, updatedReceptionist);

            return updateReceptionistResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
        }

        return BadRequest(validationResult.ToDictionary());
    }

    [HttpDelete("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteReceptionist(Guid id)
    {
        var deleteReceptionistResult = await _receptionistsService.DeleteReceptionistAsync(id);

        return deleteReceptionistResult.Match<IActionResult>(success => NoContent(), notFound => NotFound());
    }
}
