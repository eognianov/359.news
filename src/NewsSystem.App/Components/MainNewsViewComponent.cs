﻿using Microsoft.AspNetCore.Mvc;
using NewsSystem.Data.Common.Repositories;
using NewsSystem.Data.Models;
using NewsSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using NewsSystem.Mappings;
using System.Threading.Tasks;

namespace NewsSystem.App.Components
{
    [ViewComponent(Name="MainNews")]
    public class MainNewsViewComponent:ViewComponent
    {
        private readonly IDeletableEntityRepository<MainNews> mainNewsRepository;

        public MainNewsViewComponent(IDeletableEntityRepository<MainNews> mainNewsRepository)
        {
            this.mainNewsRepository = mainNewsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var news = this.mainNewsRepository.All().Where(x => !x.Source.IsDeleted).GroupBy(
                    x => x.SourceId,
                    (key, g) => g.OrderByDescending(e => e.Id).FirstOrDefault()).OrderByDescending(x => x.CreatedOn)
                .To<MainNewsViewModel>().ToList();
            var viewModel = new MainNewsComponentViewModel { MainNews = news };
            return  this.View(viewModel);
        }
    }
}
