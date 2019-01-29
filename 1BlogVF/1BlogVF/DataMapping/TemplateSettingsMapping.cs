using AutoMapper;
using BlogData.Entities;
using BlogVF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogVF.DataMapping
{
   public class SettingsNavBarMapping : Profile
    {
        public SettingsNavBarMapping()
        {
            CreateMap<NavBarEntity, TemplateViewModel>()
                    .ReverseMap();
        }
    }
}
