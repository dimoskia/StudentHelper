using AutoMapper;
using AutoMapper.QueryableExtensions;
using StudentHelper.Extensions;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace StudentHelper.Models.Pagination
{
    public class Pagination
    {
        private static Page<TReturn> CreatePageUtil<T, TReturn>(IQueryable<T> queryable, int page, int pageSize)
        {
            var totalNumberOfRecords = queryable.Count();
            var mod = totalNumberOfRecords % pageSize;
            var totalPageCount = (totalNumberOfRecords / pageSize) + (mod == 0 ? 0 : 1);
            return new Page<TReturn>
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalPages = totalPageCount,
                TotalRecords = totalNumberOfRecords
            };
        }

        public static Page<TReturn> CreateMappedPage<T, TReturn>(IQueryable<T> queryable, int page, int pageSize, string orderBy, bool ascending)
        {
            Page<TReturn> resultsPage = CreatePageUtil<T, TReturn>(queryable, page, pageSize);
            
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<T, TReturn>());

            var skipAmount = pageSize * (page - 1);

            resultsPage.Results = queryable
                .OrderByPropertyOrField(orderBy, ascending)
                .Skip(skipAmount)
                .Take(pageSize)
                .ProjectTo<TReturn>(configuration)
                .ToList();

            return resultsPage;
        }

        public static Page<T> CreatePage<T>(IQueryable<T> queryable, int page,
            int pageSize, string orderBy, bool ascending, HttpRequestMessage request)
        {
            Page<T> resultsPage = CreatePageUtil<T, T>(queryable, page, pageSize);

            var skipAmount = pageSize * (page - 1);

            resultsPage.Results = queryable
                .OrderByPropertyOrField(orderBy, ascending)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToList();

            resultsPage.NextPageUrl = null;
            if (page < resultsPage.TotalPages)
            {
                resultsPage.NextPageUrl = CreateNextPageUrl(request, page);
            }

            return resultsPage;
        }

        private static string CreateNextPageUrl(HttpRequestMessage request, int currentPage)
        {
            int nextPage = 2;
            var queryMap = HttpUtility.ParseQueryString(request.RequestUri.Query);
            if(queryMap["page"] != null)
            {
                nextPage = currentPage + 1;
            }
            queryMap["page"] = nextPage.ToString();
            return string.Format("http://{0}:{1}{2}?{3}",
                request.RequestUri.Host, request.RequestUri.Port, request.RequestUri.AbsolutePath, queryMap.ToString());
        }
    }
}