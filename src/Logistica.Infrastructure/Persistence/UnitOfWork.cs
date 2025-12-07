using Joseco.DDD.Core.Abstractions;
using Logistica.Infrastructure.Persistence.DomainModel;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Logistica.Infrastructure.Persistence;

internal class UnitOfWork : IUnitOfWork
{
    private readonly DomainDbContext _db;
    private readonly IMediator _mediator;

    public UnitOfWork(DomainDbContext db, IMediator mediator)
    {
        _db = db;
        _mediator = mediator;
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        
        await _db.SaveChangesAsync(cancellationToken);

        
        var aggregates = _db.ChangeTracker
            .Entries<AggregateRoot>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        
        aggregates.ForEach(a => a.ClearDomainEvents());

        
        foreach (var @event in events)
            await _mediator.Publish(@event, cancellationToken);
    }
}
