using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Data;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalkDiffultyAsync()
        {
            // below is shorthand for what was done below but doesn't use AutoMapper or the DTO
            // better to do what is uncommented below
            //return Ok(await walkDifficultyRepository.GetAllAsync());

            var walkDifficulty = await walkDifficultyRepository.GetAllAsync();
            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);

            if (walkDifficulty == null)
            {
                return NotFound();
            }

            var wdDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(wdDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficultyAsyn(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //if (!ValidateAddWalkDifficultyAsync(addWalkDifficultyRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            // Set Domain Model
            var wd = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code
            };

            // Pass details to repository
            wd = await walkDifficultyRepository.AddAsync(wd);

            var wdDTO = new Models.DTO.WalkDifficulty()
            {
                Id = wd.Id,
                Code = wd.Code
            };

            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id = wdDTO.Id }, wdDTO);
        }


        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //if (!ValidateUpdateWalkDifficultyAsync(updateWalkDifficultyRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            var wd = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            wd = await walkDifficultyRepository.UpdateAsync(id, wd);

            // If null then return NotFound
            if (wd == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var wdDTO = new Models.DTO.WalkDifficulty
            {
                Id = wd.Id,
                Code = wd.Code
            };
            // Return Ok response
            return Ok(wdDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var wd = await walkDifficultyRepository.DeleteAsync(id);

            // If null then return NotFound
            if (wd == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO
            var wdDTO = new Models.DTO.WalkDifficulty
            {
                Id = wd.Id,
                Code = wd.Code
            };
            // Return Ok response
            return Ok(wdDTO);

        }

        #region Private methods

        private bool ValidateAddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if (addWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"Please add Walk Difficulty data.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code), $"{nameof(addWalkDifficultyRequest.Code)} must not be empty.");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"Please update Walk Difficulty data.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} must not be empty.");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        #endregion
    }

}
