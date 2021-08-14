namespace ConformityCheck.Web.ViewModels
{
    using System;

    using ConformityCheck.Common;

    public class PagingViewModel
    {
        public PagingViewModel()
        {
            this.ItemsPerPage = 12;
            this.IntervalOfPagesToShow = 2;
            this.PageNumber = 1;
        }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int NextPageNumber => this.PageNumber + 1;

        public int IntervalOfPagesToShow { get; private set; }

        public bool HasPreviousInterval => this.PageNumber - this.IntervalOfPagesToShow > 1;

        public bool HasNextInterval => this.PageNumber + this.IntervalOfPagesToShow < this.PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)this.ItemsCount / this.ItemsPerPage);

        public int ItemsCount { get; set; }

        public int ItemsPerPage { get; set; }

        public string PagingControllerActionCallName { get; set; }

        public string CurrentSearchInput { get; set; }

        public string CurrentSortOrder { get; set; }

        public string CreatedOnSortParam => string.IsNullOrEmpty(this.CurrentSortOrder) ?
            GlobalConstants.CreatedOnSortParam : string.Empty;

        public string CurrentSortDirection { get; set; }
    }
}
