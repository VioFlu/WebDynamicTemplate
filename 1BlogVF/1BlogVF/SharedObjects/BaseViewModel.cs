using BlogData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogVF.SharedObjects
{
    public class BaseViewModel<T> : IHeaderTemplate
    {
        //public BaseViewModel(IEnumerable<NavBarEntity> _navBarEntities, T _objView)
        //{
        //    NavBarEntities = _navBarEntities;
        //    ObjView = _objView;
        //}
        public IEnumerable<NavBarEntity> NavBarEntities { get; set; }
        public T ObjView { get; set; }
    }
}
