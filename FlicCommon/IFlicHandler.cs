using System;
using System.Threading.Tasks;

namespace FlicCommon
{
    public interface IFlicHandler
    {
        Task HandleFlicRequestAsync(FlicRequest request);
    }
}
