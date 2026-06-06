using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Tooted
{
    public class List
    {
        public class Query : IRequest<List<Toode>> { }

        public class Handler : IRequestHandler<Query, List<Toode>>
        {
            private readonly ApplicationDbContext _db;

            public Handler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<List<Toode>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _db.Tooted
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
        }
    }
}