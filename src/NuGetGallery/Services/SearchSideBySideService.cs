// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NuGet.Services.Entities;
using NuGetGallery.OData;

namespace NuGetGallery
{
    public class SearchSideBySideService : ISearchSideBySideService
    {
        private readonly ISearchService _oldSearchService;
        private readonly ISearchService _newSearchService;

        public SearchSideBySideService(
            ISearchService oldSearchService,
            ISearchService newSearchService)
        {
            _oldSearchService = oldSearchService ?? throw new ArgumentNullException(nameof(oldSearchService));
            _newSearchService = newSearchService ?? throw new ArgumentNullException(nameof(newSearchService));
        }

        public async Task<SearchSideBySideViewModel> SearchAsync(string q, User currentUser)
        {
            SearchSideBySideViewModel viewModel;
            if (!string.IsNullOrWhiteSpace(q))
            {
                q = (q ?? string.Empty).Trim();

                var oldItemsTask = SearchAsync(_oldSearchService, q, currentUser);
                var newItemsTasks = SearchAsync(_newSearchService, q, currentUser);

                await Task.WhenAll(oldItemsTask, newItemsTasks);

                viewModel = new SearchSideBySideViewModel(
                    q,
                    oldItemsTask.Result,
                    newItemsTasks.Result);
            }
            else
            {
                viewModel = new SearchSideBySideViewModel();
            }

            viewModel.EmailAddress = currentUser?.EmailAddress;

            return viewModel;
        }

        private async Task<IReadOnlyList<ListPackageItemViewModel>> SearchAsync(ISearchService searchService, string q, User currentUser)
        {
            var searchFilter = SearchAdaptor.GetSearchFilter(
                q,
                page: 1,
                includePrerelease: true,
                sortOrder: null,
                context: SearchFilter.UISearchContext,
                semVerLevel: SemVerLevelKey.SemVerLevel2);

            searchFilter.Take = 10;

            var result = await searchService.Search(searchFilter);

            return result
                .Data
                .Select(x => new ListPackageItemViewModel(x, currentUser))
                .ToList();
        }
    }
}