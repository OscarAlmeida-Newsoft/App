using App.BLL.DataService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BLL
{
    public abstract class AppBLLBase
    {
        protected AppDataService _ds = new AppDataService();
    }
}
