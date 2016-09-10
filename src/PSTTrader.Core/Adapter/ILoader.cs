using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSecuritiesTrading.PSTTrader.Core.Adapter
{
    public interface ILoader
    {
        IAdapter Create(ProSecuritiesTrading.PSTTrader.Core.Base.Connection connection);
    }
}
