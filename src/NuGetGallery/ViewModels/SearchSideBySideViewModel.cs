// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NuGetGallery
{
    public class SearchSideBySideViewModel
    {
        public SearchSideBySideViewModel()
        {
            SearchTerm = string.Empty;
            OldItems = new List<ListPackageItemViewModel>();
            NewItems = new List<ListPackageItemViewModel>();
        }

        public SearchSideBySideViewModel(
            string searchTerm,
            IReadOnlyList<ListPackageItemViewModel> oldItems,
            IReadOnlyList<ListPackageItemViewModel> newItems)
        {
            SearchTerm = searchTerm;
            OldItems = oldItems;
            NewItems = newItems;
        }

        public string SearchTerm { get; set; }
        public IReadOnlyList<ListPackageItemViewModel> OldItems { get; }
        public IReadOnlyList<ListPackageItemViewModel> NewItems { get; }

        [Display(Name = "Which results are better?")]
        public string BetterSide { get; set; }

        [Display(Name = "What was the most relevant package?")]
        public string MostRelevantPackage { get; set; }

        [Display(Name = "Name at least one package you were expecting to see.")]
        public string ExpectedPackages { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "Email (optional, provide only if you want us to be able to follow up)")]
        public string EmailAddress { get; set; }
    }
}