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
    [Route(Constants.ApiLoanRoute)]
    [Produces(Constants.ApplicationJson)]
    [Consumes(Constants.ApplicationJson)]
    [ProducesResponseType(typeof(kamafi.core.data.IKamafiProblemDetail), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(kamafi.core.data.IKamafiProblemDetailBase), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(kamafi.core.data.IKamafiProblemDetailBase), StatusCodes.Status401Unauthorized)]
    public class LoanController : ControllerBase
    {
        private readonly ILoanRepository _repo;

        public LoanController(
            ILoanRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        /// <summary>
        /// Add Liability.Loan
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody, Required] LoanDto dto)
        {
            return Created(nameof(Loan), await _repo.AddAsync(dto));
        }
    }
}
