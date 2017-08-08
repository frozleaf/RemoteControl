namespace Newtonsoft.Json.Serialization
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    internal struct ResolverContractKey : IEquatable<ResolverContractKey>
    {
        private readonly Type _resolverType;
        private readonly Type _contractType;
        public ResolverContractKey(Type resolverType, Type contractType)
        {
            this._resolverType = resolverType;
            this._contractType = contractType;
        }

        public override int GetHashCode()
        {
            return (this._resolverType.GetHashCode() ^ this._contractType.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            return ((obj is ResolverContractKey) && this.Equals((ResolverContractKey) obj));
        }

        public bool Equals(ResolverContractKey other)
        {
            return ((this._resolverType == other._resolverType) && (this._contractType == other._contractType));
        }
    }
}

