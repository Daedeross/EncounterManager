namespace Foundation.ServiceFabric
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.ServiceFabric.Actors;

    //copied and repurposed from: https://github.com/Microsoft/referencesource/blob/master/System.Core/System/Linq/Enumerable.cs
    internal class ActorStateSet<TState, TActor>
        where TState : ActorState<TActor>
        where TActor : IActor
    {
        private int[] _buckets;
        private Slot[] _slots;
        private int _count;
        private int _freeList;
        private readonly IEqualityComparer<ActorReference> _comparer;

        public ActorStateSet() : this(null) { }

        public ActorStateSet(IEqualityComparer<ActorReference> comparer)
        {
            if (comparer == null) comparer = ActorReferenceEqualityComparer.Default;
            _comparer = comparer;
            _buckets = new int[7];
            _slots = new Slot[7];
            _freeList = -1;
        }

        public bool Add(TState value)
        {
            var hashCode = InternalGetHashCode(value.Reference);
            var context = FindInternal(value.Reference, hashCode);
            if (!context.Found)
            {
                InsertSlot(context.Bucket, hashCode, value);
                return true;
            }
            return false;
        }

        // If value is not in set, add it and return true; otherwise return false
        public async Task<TState> AddAsync(TActor value, Func<TActor, Task<TState>> factory)
        {
            var reference = value.GetActorReference();
            var hashCode = InternalGetHashCode(reference);
            var context = FindInternal(reference, hashCode);
            if (!context.Found && factory != null)
            {
                return InsertSlot(context.Bucket, hashCode, await factory(value));
            }
            return null;
        }

        // Check whether value is in set
        public bool Contains(TActor value)
        {
            var reference = value.GetActorReference();
            var hashCode = InternalGetHashCode(reference);
            return FindInternal(reference, hashCode).Found;
        }

        // If value is in set, remove it and return true; otherwise return false
        public TState Remove(TActor value)
        {
            var reference = value.GetActorReference();
            int hashCode = InternalGetHashCode(reference);
            var context = FindInternal(reference, hashCode);
            if (!context.Found) return null;

            var slot = _slots[context.Index];
            if (context.LastIndex < 0)
            {
                _buckets[context.Bucket] = slot.Next + 1;
            }
            else
            {
                _slots[context.LastIndex].Next = slot.Next;
            }

            var removed = slot.Value;

            slot.HashCode = -1;
            slot.Value = default(TState);
            slot.Next = _freeList;
            _freeList = context.Index;
            return removed;
        }

        private FindContext FindInternal(ActorReference reference, int hashCode)
        {
            var context = new FindContext
            {
                Bucket = hashCode % _buckets.Length,
                LastIndex = -1,
                Index = _buckets[hashCode % _buckets.Length] - 1
            };
            while (context.Index >= 0)
            {
                var slot = _slots[context.Index];

                if (slot.HashCode == hashCode && _comparer.Equals(slot.Value.Reference, reference))
                {
                    context.Found = true;
                    break;
                }

                context.LastIndex = context.Index;
                context.Index = _slots[context.Index].Next;
            }
            return context;
        }

        private TState InsertSlot(int bucket, int hashCode, TState value)
        {
            int index;
            if (_freeList >= 0)
            {
                index = _freeList;
                _freeList = _slots[index].Next;
            }
            else
            {
                if (_count == _slots.Length) Resize();
                index = _count;
                _count++;
            }
            _slots[index].HashCode = hashCode;
            _slots[index].Value = value;
            _slots[index].Next = _buckets[bucket] - 1;
            _buckets[bucket] = index + 1;
            return value;
        }

        void Resize()
        {
            int newSize = checked(_count * 2 + 1);
            int[] newBuckets = new int[newSize];
            Slot[] newSlots = new Slot[newSize];
            Array.Copy(_slots, 0, newSlots, 0, _count);
            for (int i = 0; i < _count; i++)
            {
                int bucket = newSlots[i].HashCode % newSize;
                newSlots[i].Next = newBuckets[bucket] - 1;
                newBuckets[bucket] = i + 1;
            }
            _buckets = newBuckets;
            _slots = newSlots;
        }

        internal int InternalGetHashCode(ActorReference reference)
        {
            //Microsoft DevDivBugs 171937. work around comparer implementations that throw when passed null
            return (reference == null) ? 0 : _comparer.GetHashCode(reference) & 0x7FFFFFFF;
        }

        internal struct Slot
        {
            internal int HashCode;
            internal TState Value;
            internal int Next;
        }

        internal struct FindContext
        {
            internal int Bucket;
            internal int LastIndex;
            internal int Index;
            internal bool Found;
        }
    }
}