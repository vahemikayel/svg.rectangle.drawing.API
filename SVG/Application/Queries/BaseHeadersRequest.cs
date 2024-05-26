namespace SVG.API.Application.Queries
{
    public class BaseHeadersRequest
    {
        public Guid _RequestId { get; private set; }

        public Guid _UserId { get; private set; }

        public void SetRequestId(Guid requestId) => _RequestId = requestId;

        internal void SetUserId(Guid userId)
        {
            _UserId = userId;
        }
    }
}
