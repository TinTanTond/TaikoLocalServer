using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Application.Interfaces;

public interface ITaikoDbContext
{
    public DbSet<Card> Cards { get; set; }
    public DbSet<Credential> Credentials { get; set; } 
    public DbSet<SongBestDatum> SongBestData { get; set; }
    public DbSet<SongPlayDatum> SongPlayData { get; set; }
    public DbSet<UserDatum> UserData { get; set; } 
    
    public DbSet<DanScoreDatum> DanScoreData { get; set; } 
    public DbSet<DanStageScoreDatum> DanStageScoreData { get; set; }
    public DbSet<AiScoreDatum> AiScoreData { get; set; }
    public DbSet<AiSectionScoreDatum> AiSectionScoreData { get; set; }
    public DbSet<Token> Tokens { get; set; } 
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    EntityEntry<TEntity> Update<TEntity>(TEntity entity) where TEntity : class;
}