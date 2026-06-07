﻿using System.Collections.Generic;
using System;

namespace KooliProjekt.Application.Infrastructure.Paging
{
    public class PagedResult<T>
    {
        // PROPERTID, mida kasutab PagingExtensions.cs
        public List<T> Results { get; set; } 
        public int CurrentPage { get; set; } 
        public int RowCount { get; set; } 
        public int PageSize { get; set; }
        public int PageCount { get; set; } 
    }
}