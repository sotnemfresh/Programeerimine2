using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tellimused
{
    public class List
    {
        public class Query : IRequest<List<Tellimus>> { }

        public class Handler : IRequestHandler<Query, List<Tellimus>>
        {
            private readonly ApplicationDbContext _db;

            public Handler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<List<Tellimus>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _db.Tellimused
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
        }
    }
}