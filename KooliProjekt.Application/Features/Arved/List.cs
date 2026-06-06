using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Arved
{
    public class List
    {
        public class Query : IRequest<List<Arve>> { }

        public class Handler : IRequestHandler<Query, List<Arve>>
        {
            private readonly ApplicationDbContext _db;

            public Handler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<List<Arve>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _db.Arved
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
        }
    }
}