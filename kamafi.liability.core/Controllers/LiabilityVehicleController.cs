using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

using kamafi.liability.services;
using kamafi.liability.data;

namespace kamafi.liability.core
{
    [Authorize]
    [ApiController]
    [ApiVersion(Constants.ApiV1)]
    [Route(Constants.ApiVehicleRoute)]
    [Produces(Constants.ApplicationJson)]
    [Consumes(Constants.ApplicationJson)]
    [ProducesResponseType(typeof(kamafi.core.data.IKamafiProblemDetail), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(kamafi.core.data.IKamafiProblemDetailBase), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(kamafi.core.data.IKamafiProblemDetailBase), StatusCodes.Status401Unauthorized)]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleRepository _repo;

        public VehicleController(
            IVehicleRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Add Liability.Vehicle
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody, Required] VehicleDto dto)
        {
            return Created(nameof(Vehicle), await _repo.AddAsync(dto));
        }
    }
}
