﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Helpers
{
    public class PagedList<T> : List<T>
    {
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }
        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            try
            {
                var count = source.Count();
                var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }
            catch (NullReferenceException ex)
            {
                return new PagedList<T>(new List<T>(), 0, pageNumber, pageSize);
            }

        }
    }
}
