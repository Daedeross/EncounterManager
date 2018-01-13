namespace Foundation
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Name = "QueryResponse")]
    public class QueryResponse<T> : Response
    {
        [DataMember(Name = "Results", IsRequired = true, EmitDefaultValue = true, Order = 2)]
        public IEnumerable<T> Results { get; set; }

        [DataMember(Name = "Offset", IsRequired = true, EmitDefaultValue = true, Order = 3)]
        public long Offset { get; set; }

        [DataMember(Name = "Total", IsRequired = true, EmitDefaultValue = true, Order = 4)]
        public long Total { get; set; }

        public QueryResponse()
            : this(ResponseState.Ok, null, null)
        {
        }

        public QueryResponse(IEnumerable<T> results)
            : this(ResponseState.Ok, null, results)
        {
        }

        public QueryResponse(ResponseState state, string message)
            : this(state, message, null)
        {
        }

        public QueryResponse(ResponseState state, string message, IEnumerable<T> results)
            : base(state, message)
        {
            if (results != null)
            {
                var list = results.ToList();
                Total = list.Count;
                Results = list;
            }
            else
            {
                Results = new List<T>();
            }
        }

        public override void Accept(IResponseVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
