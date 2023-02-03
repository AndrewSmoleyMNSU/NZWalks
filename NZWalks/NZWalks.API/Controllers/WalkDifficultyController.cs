using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

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
        public async Task<IActionResult> GetAllWalkDiffultyAsync()
        {
            var walkDifficulty = await walkDifficultyRepository.GetAllAsync();
            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
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
        public async Task<IActionResult> AddWalkDifficultyAsyn(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
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
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
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
    }
}
