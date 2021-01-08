namespace ConformityCheck.Web.ViewModels
{
    using System;

    public class PagingViewModel
    {
        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int NextPageNumber => this.PageNumber + 1;

        public int IntervalOfPagesToShow { get; set; }

        public bool HasPreviousInterval => this.PageNumber - this.IntervalOfPagesToShow > 1;

        public bool HasNextInterval => this.PageNumber + this.IntervalOfPagesToShow < this.PagesCount;

        public int PagesCount => (int)Math.Ceiling((double)this.ItemsCount / this.ItemsPerPage);

        public int ItemsCount { get; set; }

        public int ItemsPerPage { get; set; }

        public string PagingAspAction { get; set; }
    }
}
