using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Kliendid
{
    public class List
    {
        public class Query : IRequest<List<Klient>> { }

        public class Handler : IRequestHandler<Query, List<Klient>>
        {
            private readonly ApplicationDbContext _db;

            public Handler(ApplicationDbContext db)
            {
                _db = db;
            }

            public async Task<List<Klient>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _db.Kliendid
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
        }
    }
}