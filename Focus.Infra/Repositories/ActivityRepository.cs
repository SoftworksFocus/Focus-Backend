using Microsoft.EntityFrameworkCore;

namespace Focus.Infra.Repositories;
using Focus.Domain.Entities;

public class ActivityRepository
{
    private readonly FocusDbContext _context;

    public ActivityRepository(FocusDbContext context)
    {
        _context = context;
    }

    public async Task<Activity> GetById(int id)
    {
        var  activity = await _context.Activities.FindAsync(id);

        if (activity == null)
        {
            throw new KeyNotFoundException($"Activity with {id} not found");
        }

        return activity;
    }

    public async Task<IEnumerable<Activity>> GetAllAsync()
    {
        var  activities = await _context.Activities.ToListAsync();

        if (activities == null || !activities.Any())
        {
            throw new KeyNotFoundException("No activities found");
        }
        
        return activities;
    }
    
    public async Task AddAsync(Activity entity)
    {
        //Adicionar algum método de verificação para o usuário que enviou/postou a atividade
        //Adicionar algum método de verificação para o grupo que a atividade foi enviada (considerar que o grupo pode ser nulo)
        
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Activity cannot be null.");
        }
        
        await _context.Activities.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Activity entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Activity cannot be null.");
        }
        
        var existinActivity = await _context.Activities.FindAsync(entity.Id);
        
        if (existinActivity == null)
        {
            throw new KeyNotFoundException($"Activity with {entity.Id} not found.");
        }
        
        //Mostrar de alguma forma como a atividade foi alterada e quando ela foi alterada.
        
        _context.Activities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await  _context.Activities.FindAsync(id);
        
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Activity cannot be null.");
        }

        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }
}