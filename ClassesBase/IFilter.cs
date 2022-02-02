using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesBase
{
    public class IFilter
    {
        public int? Page
        {
            get
            {
                if (Page.HasValue)
                {
                    return Page.Value;
                }
                else
                {
                    return 1;
                }
            }
            set
            {
                Page = value;
            }
        }
        public int? RecordsPerPage
        {
            get
            {
                if (RecordsPerPage.HasValue)
                {
                    return RecordsPerPage;
                }
                else
                {
                    return 10;
                }
            }
            set
            {
                RecordsPerPage = value;
            }
        }
    }
}
