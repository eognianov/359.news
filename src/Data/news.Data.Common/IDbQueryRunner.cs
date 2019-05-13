using System;
using System.Collections.Generic;
using System.Text;

namespace news.Data.Common
{
    using System;
    using System.Threading.Tasks;

    public interface IDbQueryRunner : IDisposable
    {
        Task RunQueryAsync(string query, params object[] parameters);
    }
}
