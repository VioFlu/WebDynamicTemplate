using BlogData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogData
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogContext _context;
        private readonly ILogger<BlogRepository> _logger;

        public BlogRepository(BlogContext context, ILogger<BlogRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public IEnumerable<NavBarEntity> GetAllNavBarEntities()
        {
            try
            {
                _logger.LogInformation("Start GetAllNavBarEntities...");
                return _context.NavBarEntities
                               .Include(n => n.NavBarEntityItems)
                               .OrderBy(e => e.SeqNo)
                               .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to GetAllNavBarEntities: {ex}");
                return null;
            }
        }

        public IEnumerable<NavBarEntityItem> GetAllNavBarItemEntities()
        {
            try
            {
                _logger.LogInformation("Start GetAllNavBarEntities...");
                return _context.NavBarEntityItems.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to GetAllNavBarEntities: {ex}");
                return null;
            }
        }

        public IEnumerable<NavBarEntityItem> GetNavBarEntityItemsByNavBar(Guid GuidNavBarEntity)
        {
            try
            {
                _logger.LogInformation("Start GetNavBarEntityItemsByNavBar...");
                return _context.NavBarEntityItems
                                .Where(n => n.NavBarEntityGUID == GuidNavBarEntity)
                                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"failed to GetNavBarEntityItemsByNavBar: {ex}");
                return null;

            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
