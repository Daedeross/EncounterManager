namespace Foundation.ServiceFabric
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// This class abstract provides common indexing behavior for a referential payload instance.
    /// The wrapped reference must be DataContract serializable or the indexable type will not be serializable (obviously)
    /// </summary>
    /// <typeparam name="TIndexableType">The type of the indexable derivative class, to enable self-referential IEquatable and IComparable support</typeparam>
    /// <typeparam name="TProxyType">The type of the referenced payload</typeparam>
    /// <seealso cref="System.IEquatable{TIndexableType}" />
    /// <seealso cref="System.IComparable{TIndexableType}" />
    [DataContract]
    public abstract class Indexable<TIndexableType, TProxyType> : IEquatable<TIndexableType>, IComparable<TIndexableType>
        where TIndexableType : Indexable<TIndexableType, TProxyType>
    {
        [DataMember(Name = "Reference", IsRequired = true, EmitDefaultValue = false)]
        private TProxyType _reference;

        private volatile string _stringRepresentation;

        [IgnoreDataMember]
        public TProxyType Reference => _reference;

        [IgnoreDataMember]
        public string StringRepresentation
        {
            get
            {
                if (_stringRepresentation == null)
                {
                    _stringRepresentation = BuildRepresentation();
                }
                return _stringRepresentation;
            }
        }

        protected Indexable(TProxyType reference)
        {
            if (reference == null)
            {
                throw new ArgumentNullException(nameof(reference));
            }
            _reference = reference;
        }

        public bool Equals(TIndexableType other)
        {
            return ReferenceEquals(this, other) ||
                   !ReferenceEquals(other, null)
                   && Equals(StringRepresentation, other.StringRepresentation);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TIndexableType);
        }

        public int CompareTo(TIndexableType other)
        {
            if (other == null) return 1;
            if (ReferenceEquals(this, other)) return 0;
            return string.Compare(StringRepresentation, other.StringRepresentation, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return StringRepresentation.GetHashCode();
        }

        public override string ToString()
        {
            return StringRepresentation;
        }

        protected abstract string BuildRepresentation();
    }
}