using ExpenseAlly.Domain.Entities;
using ExpenseAlly.Infrastructure.Identity;
using ExpenseAlly.Infrastructure.Persistence.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ExpenseAlly.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    public string? CurrentUserId { get; set; }

    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator, ICurrentUserService currentUserService,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }

    /// <summary>
    /// DB SETS 
    /// </summary>

    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<TransactionCategory> TransactionCategories => Set<TransactionCategory>();
    public DbSet<SavingGoal> SavingGoals => Set<SavingGoal>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(BaseAuditableEntity).IsAssignableFrom(entityType.ClrType))
            {
                ApplyQueryFilter(builder, entityType.ClrType);
            }
        }

        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }
    /*
     public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userId = _currentUserService.UserId;

        foreach (var entry in ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = new Guid(userId); // Populate CreatedBy
                entry.Entity.CreatedOn = DateTime.UtcNow; // Populate CreatedOn
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = new Guid(userId); // Populate LastModifiedBy
                entry.Entity.LastModifiedOn = DateTime.UtcNow; // Populate LastModifiedOn
            }
        }

        await _mediator.DispatchDomainEvents(this);

        return await base.SaveChangesAsync(cancellationToken);
    }

     */
    private void ApplyQueryFilter(ModelBuilder modelBuilder, Type entityType)
    {
        // Dynamically set the query filter
        var method = typeof(ApplicationDbContext)
            .GetMethod(nameof(ApplyQueryFilterForType), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.MakeGenericMethod(entityType);

        method?.Invoke(this, new object[] { modelBuilder });
    }

    private void ApplyQueryFilterForType<T>(ModelBuilder modelBuilder)
    where T : BaseAuditableEntity
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => e.CreatedBy == new Guid(_currentUserService.UserId));
    }
}