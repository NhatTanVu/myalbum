using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAlbum.Services.Photo.API.Core;
using MyAlbum.Services.Photo.API.Core.Exceptions;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace MyAlbum.Services.Photo.API.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyAlbumDbContext context;
        public UnitOfWork(MyAlbumDbContext context)
        {
            this.context = context;

        }

        private (string Provider, string ExternalId)? TryGetDuplicateExternalPhoto()
        {
            var entry = context.ChangeTracker
                .Entries<Core.Models.Photo>()
                .FirstOrDefault(e => e.State == EntityState.Added);

            if (entry == null)
                return null;

            var provider = entry.Property(p => p.ExternalProvider).CurrentValue;
            var externalId = entry.Property(p => p.ExternalId).CurrentValue;

            if (string.IsNullOrWhiteSpace(provider) ||
                string.IsNullOrWhiteSpace(externalId))
            {
                return null;
            }

            return (provider, externalId);
        }

        private bool IsDuplicateExternalPhoto(DbUpdateException ex)
        {
            return ex.InnerException is SqlException sqlEx &&
                (sqlEx.Number == 2601 || sqlEx.Number == 2627);
        }

        public async Task CompleteAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsDuplicateExternalPhoto(ex))
            {
                var info = TryGetDuplicateExternalPhoto();

                if (info != null)
                {
                    throw new DuplicateExternalPhotoException(info.Value.Provider, info.Value.ExternalId);
                }

                throw;
            }
        }
    }
}