using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Docket.Shared
{
    public class DTOFavoriteReport
    {
        public string DocketId { get; set; }
        public string Title {  get; set; }
        public string UserId {  get; set; }
        public string DateAddedd {  get; set; }
        public double SumOfFavoritesForDay {  get; set; }
    }
}
