using MediatR;
using Newtonsoft.Json;

namespace SVG.API.Application.Queries
{
    public abstract class BaseRequest<TResponse> : BaseHeadersRequest, IRequest<TResponse>
    {

    }
}
