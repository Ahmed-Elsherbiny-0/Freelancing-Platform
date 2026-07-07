using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OfferSpecParms
    {
        private int MaxPageSize { get; } = 30;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = Math.Min(value, MaxPageSize); }
        }
    }
}
