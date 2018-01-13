namespace Foundation
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    [DataContract]
    public sealed class Identity : IEquatable<Identity>, IComparable<Identity>
    {
        public static readonly Identity None = null;
        private const int IntNon = -1;
        private const long LongNon = -1;
        private static readonly Guid GuidNon = Guid.Empty;
        private const string StringNon = null;

        [DataMember(Name = "Kind", Order = 1, IsRequired = true, EmitDefaultValue = true)]
        private readonly IdentityIdKind _kind;

        [DataMember(Name = "IntId", Order = 2, IsRequired = false, EmitDefaultValue = false)]
        private readonly int _intId;

        [DataMember(Name = "LongId", Order = 3, IsRequired = false, EmitDefaultValue = false)]
        private readonly long _longId;

        [DataMember(Name = "GuidId", Order = 4, IsRequired = false, EmitDefaultValue = false)]
        private readonly Guid _guidId;

        [DataMember(Name = "StringId", Order = 5, IsRequired = false, EmitDefaultValue = false)]
        private readonly string _stringId;

        private volatile string _stringRepresentation;

        private volatile string _storageKey;

        internal string StorageKey
        {
            get
            {
                if (_storageKey == null)
                {
                    switch (_kind)
                    {
                        case IdentityIdKind.Int:
                            _storageKey = $"{_kind}_{_intId}";
                            break;
                        case IdentityIdKind.Long:
                            _storageKey = $"{_kind}_{_longId}";
                            break;
                        case IdentityIdKind.Guid:
                            _storageKey = $"{_kind}_{_guidId}";
                            break;
                        case IdentityIdKind.String:
                            _storageKey = $"{_kind}_{_stringId}";
                            break;
                        default:
                            throw new InvalidOperationException($"Unknown IdentityIdKind:{_kind}");
                    }
                }
                return _storageKey;
            }
        }

        public IdentityIdKind Kind => _kind;

        private Identity()
        {
        }

        public Identity(int id)
        {
            if (id == IntNon) throw new ArgumentException($"id cannot be {IntNon}");
            _kind = IdentityIdKind.Int;
            _intId = id;
        }

        public Identity(long id)
        {
            if (id == LongNon) throw new ArgumentException($"id cannot be {LongNon}");
            _kind = IdentityIdKind.Long;
            _longId = id;
        }

        public Identity(Guid id)
        {
            if (id == GuidNon) throw new ArgumentException($"id cannot be {GuidNon}");
            _kind = IdentityIdKind.Guid;
            _guidId = id;
        }

        public Identity(string id)
        {
            if (id == StringNon) throw new ArgumentException($"id cannot be null");
            _kind = IdentityIdKind.String;
            _stringId = id;
        }

        public int GetIntId()
        {
            if (_kind == IdentityIdKind.Int)
            {
                return _intId;
            }
            throw new InvalidOperationException($"{nameof(GetIntId)} requires IdentityIdKind:{IdentityIdKind.Int}. Identity has kind {_kind}");
        }

        public long GetLongId()
        {
            if (_kind == IdentityIdKind.Long)
            {
                return _longId;
            }
            throw new InvalidOperationException($"{nameof(GetLongId)} requires IdentityIdKind:{IdentityIdKind.Long}. Identity has kind {_kind}");
        }

        public Guid GetGuidId()
        {
            if (_kind == IdentityIdKind.Guid)
            {
                return _guidId;
            }
            throw new InvalidOperationException($"{nameof(GetGuidId)} requires IdentityIdKind:{IdentityIdKind.Guid}. Identity has kind {_kind}");
        }

        public string GetStringId()
        {
            if (_kind == IdentityIdKind.String)
            {
                return _stringId;
            }
            throw new InvalidOperationException($"{nameof(GetStringId)} requires IdentityIdKind:{IdentityIdKind.String}. Identity has kind {_kind}");
        }

        public override string ToString()
        {
            if (_stringRepresentation != null)
            {
                return _stringRepresentation;
            }
            string result;
            switch (_kind)
            {
                case IdentityIdKind.Int:
                    result = _intId.ToString(CultureInfo.InvariantCulture);
                    break;
                case IdentityIdKind.Long:
                    result = _longId.ToString(CultureInfo.InvariantCulture);
                    break;
                case IdentityIdKind.Guid:
                    result = _guidId.ToString();
                    break;
                case IdentityIdKind.String:
                    result = _stringId;
                    break;
                default:
                    result = string.Empty;
                    break;
            }
            _stringRepresentation = result;
            return result;
        }

        public override int GetHashCode()
        {
            switch (_kind)
            {
                case IdentityIdKind.Int:
                    return _intId.GetHashCode();
                case IdentityIdKind.Long:
                    return _longId.GetHashCode();
                case IdentityIdKind.Guid:
                    return _guidId.GetHashCode();
                case IdentityIdKind.String:
                    return _stringId.GetHashCode();
                default:
                    return 0;
            }
        }

        public int CompareTo(Identity other)
        {
            return ReferenceEquals(other, None) ? 1 : CompareContents(this, other);
        }

        public bool Equals(Identity other)
        {
            return !ReferenceEquals(other, None) && EqualsContents(this, other);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Identity);
        }

        #region Explicit Cast

        public static explicit operator int(Identity identity)
        {
            return identity?.GetIntId() ?? IntNon;
        }

        public static explicit operator Identity(int id)
        {
            return id != IntNon ? new Identity(id) : None;
        }

        public static explicit operator long(Identity identity)
        {
            return identity?.GetLongId() ?? LongNon;
        }

        public static explicit operator Identity(long id)
        {
            return id != LongNon ? new Identity(id) : None;
        }

        public static explicit operator Guid(Identity identity)
        {
            return identity?.GetGuidId() ?? GuidNon;
        }

        public static explicit operator Identity(Guid id)
        {
            return id != GuidNon ? new Identity(id) : None;
        }

        public static explicit operator string(Identity identity)
        {
            return identity?.GetStringId() ?? StringNon;
        }

        public static explicit operator Identity(string id)
        {
            return id != StringNon ? new Identity(id) : None;
        }

        #endregion Explicit Cast
        #region Implicit Equality
        #region Int

        public static bool operator ==(Identity identity, int i)
        {
            return (ReferenceEquals(identity, null) && i == IntNon) ||
                   (!ReferenceEquals(identity, null) && identity._kind == IdentityIdKind.Int && identity._intId == i);
        }

        public static bool operator !=(Identity identity, int i)
        {
            return !(identity == i);
        }

        public static bool operator ==(int i, Identity identity)
        {
            return identity == i;
        }

        public static bool operator !=(int i, Identity identity)
        {
            return !(identity == i);
        }

        #endregion Int
        #region Long

        public static bool operator ==(Identity identity, long i)
        {
            return (ReferenceEquals(identity, null) && i == LongNon) ||
                   (!ReferenceEquals(identity, null) && identity._kind == IdentityIdKind.Long && identity._longId == i);
        }

        public static bool operator !=(Identity identity, long i)
        {
            return !(identity == i);
        }

        public static bool operator ==(long i, Identity identity)
        {
            return identity == i;
        }

        public static bool operator !=(long i, Identity identity)
        {
            return !(identity == i);
        }

        #endregion Long
        #region Guid

        public static bool operator ==(Identity identity, Guid i)
        {
            return (ReferenceEquals(identity, null) && i == GuidNon) ||
                   (!ReferenceEquals(identity, null) && identity._kind == IdentityIdKind.Guid && identity._guidId == i);
        }

        public static bool operator !=(Identity identity, Guid i)
        {
            return !(identity == i);
        }

        public static bool operator ==(Guid i, Identity identity)
        {
            return identity == i;
        }

        public static bool operator !=(Guid i, Identity identity)
        {
            return !(identity == i);
        }

        #endregion Guid
        #region String

        public static bool operator ==(Identity identity, string i)
        {
            return (ReferenceEquals(identity, null) && i == StringNon) ||
                   (!ReferenceEquals(identity, null) && identity._kind == IdentityIdKind.String && identity._stringId == i);
        }

        public static bool operator !=(Identity identity, string i)
        {
            return !(identity == i);
        }

        public static bool operator ==(string i, Identity identity)
        {
            return identity == i;
        }

        public static bool operator !=(string i, Identity identity)
        {
            return !(identity == i);
        }

        #endregion String
        #endregion Implicit Equality

        private static bool EqualsContents(Identity x, Identity y)
        {
            if (x._kind != y._kind)
            {
                return false;
            }
            switch (x._kind)
            {
                case IdentityIdKind.Int:
                    return x._intId == y._intId;
                case IdentityIdKind.Long:
                    return x._longId == y._longId;
                case IdentityIdKind.Guid:
                    return x._guidId == y._guidId;
                case IdentityIdKind.String:
                    return string.Equals(x._stringId, y._stringId, StringComparison.OrdinalIgnoreCase);
                default:
                    return false;
            }
        }

        private static int CompareContents(Identity x, Identity y)
        {
            if (x._kind != y._kind)
            {
                return string.Compare(x.StorageKey, y.StorageKey, StringComparison.OrdinalIgnoreCase);
            }
            switch (x._kind)
            {
                case IdentityIdKind.Int:
                    return x._intId.CompareTo(y._intId);
                case IdentityIdKind.Long:
                    return x._longId.CompareTo(y._longId);
                case IdentityIdKind.Guid:
                    return x._guidId.CompareTo(y._guidId);
                case IdentityIdKind.String:
                    return string.Compare(x._stringId, y._stringId, StringComparison.OrdinalIgnoreCase);
                default:
                    return 0;
            }
        }
    }
}
