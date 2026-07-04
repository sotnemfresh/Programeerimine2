using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.BlazorWasm
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<Toode>>> List(int page, int pageSize);
        Task<OperationResult> Save(Toode list);
        Task<OperationResult> Delete(int id);
    }
}