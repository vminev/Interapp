﻿namespace Interapp.Web.ViewModels.Universities
{
    using Data.Models;
    using Infrastructure.Mapping;

    public class CountryViewModel : IMapFrom<Country>
    {
        public string Name { get; set; }
    }
}