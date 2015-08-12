using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextmagicRest.Model
{
    public class BaseModelList: BaseModel
    {
        /// <summary>
        /// Current page number
        /// </summary>
        public int Page { set; get; }

        /// <summary>
        /// How many results per page
        /// </summary>
        public int Limit { set; get; }

        /// <summary>
        /// Total page count
        /// </summary>
        public int PageCount { set; get; }
    }
}
