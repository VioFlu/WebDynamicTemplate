using BlogData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogVF.SharedObjects
{
    public interface IHeaderTemplate
    {
        IEnumerable<NavBarEntity> NavBarEntities { get; set; }
    }
}
