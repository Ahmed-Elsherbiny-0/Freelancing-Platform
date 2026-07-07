using Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class JobSpecParms
    {
        private int MaxPageSize { get; } = 30;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = Math.Min(value, MaxPageSize); }
        }

        private string _search = string.Empty;

        public string Search
        {
            get { return _search; }
            set { _search = value.ToLower(); }
        }

        public int? Budget { get; set; }
        public JobStatus? Status { get; set; } = null;
    }
}
