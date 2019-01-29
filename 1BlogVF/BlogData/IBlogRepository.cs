using BlogData.Entities;
using System.Collections.Generic;

namespace BlogData
{
    public interface IBlogRepository
    {
        IEnumerable<NavBarEntity> GetAllNavBarEntities();
        IEnumerable<NavBarEntityItem> GetAllNavBarItemEntities();
        bool SaveAll();
        void AddEntity(object model);
    }
}
