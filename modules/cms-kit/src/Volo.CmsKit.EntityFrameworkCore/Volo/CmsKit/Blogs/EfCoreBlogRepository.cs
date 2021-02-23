﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.EntityFrameworkCore;

namespace Volo.CmsKit.Blogs
{
    public class EfCoreBlogRepository : EfCoreRepository<ICmsKitDbContext, Blog, Guid>, IBlogRepository
    {
        public EfCoreBlogRepository(IDbContextProvider<ICmsKitDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> ExistsAsync(Guid blogId, CancellationToken cancellationToken = default)
        {
            return await (await GetQueryableAsync()).AnyAsync(x => x.Id == blogId, cancellationToken);
        }

        public virtual async Task<bool> HasPostsAsync(Guid blogId, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return await dbContext.BlogPosts.AnyAsync(x => x.BlogId == blogId, cancellationToken);
        }

        public virtual Task<Blog> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return GetAsync(x => x.Slug == slug, cancellationToken: cancellationToken);
        }
    }
}
