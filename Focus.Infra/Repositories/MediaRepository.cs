using Focus.Domain.Entities;
using Focus.Infra.Repositories.Interfaces;

namespace Focus.Infra.Repositories;

public class MediaRepository : IMediaRepository
{
    private readonly FocusDbContext _context;
    
    public MediaRepository(FocusDbContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(Media media)
    {
        _context.Media.Add(media);
        await _context.SaveChangesAsync();
    }
}