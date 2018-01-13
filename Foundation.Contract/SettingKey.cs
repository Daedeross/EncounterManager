namespace Foundation
{
    using System;
    public sealed class SettingKey<T> : IEquatable<SettingKey<T>>
    {
        private int? _hashCode;
        private int HashCode
        {
            get
            {
                if (!_hashCode.HasValue)
                {
                    unchecked
                    {
                        var hashCode = Configuration.GetHashCode();
                        hashCode = (hashCode * 397) ^ Section.GetHashCode();
                        hashCode = (hashCode * 397) ^ Parameter.GetHashCode();
                        _hashCode = hashCode;
                    }
                }
                return _hashCode.Value;
            }
        }

        public string Configuration { get; }
        public string Section { get; }
        public string Parameter { get; }

        public SettingKey(string section, string parameter)
            : this(null, section, parameter)
        {
        }

        public SettingKey(string configuration, string section, string parameter)
        {
            Configuration = string.IsNullOrEmpty(configuration) ? "Config" : configuration;
            Section = section;
            Parameter = parameter;
        }

        public bool Equals(SettingKey<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Configuration, other.Configuration, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(Section, other.Section, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(Parameter, other.Parameter, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SettingKey<T>);
        }

        public override int GetHashCode()
        {
            return HashCode;
        }
    }
}