using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository: IWalkDifficultyRepository
    {
        private readonly NZWalksDBContext nzWalksDBContext;

        public WalkDifficultyRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nzWalksDBContext = nZWalksDBContext;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nzWalksDBContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nzWalksDBContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nzWalksDBContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nzWalksDBContext.SaveChangesAsync();

            return walkDifficulty;
        }
        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWD = nzWalksDBContext.WalkDifficulty.FirstOrDefault(x => x.Id == id);

            if (existingWD == null)
            {
                return null;
            }

            existingWD.Code = walkDifficulty.Code;

            await nzWalksDBContext.SaveChangesAsync();

            return existingWD;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWD = nzWalksDBContext.WalkDifficulty.FirstOrDefault(x => x.Id == id);

            if (existingWD == null)
            {
                return null;
            }

            // Delete
            nzWalksDBContext.Remove(existingWD);
            await nzWalksDBContext.SaveChangesAsync();


            return existingWD;
        }

    }
}
