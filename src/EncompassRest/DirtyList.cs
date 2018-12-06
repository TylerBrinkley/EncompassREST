﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using EncompassRest.Loans;
using EncompassRest.Utilities;
using Newtonsoft.Json;

namespace EncompassRest
{
    /// <summary>
    /// Collection to use for dirty checking.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [JsonConverter(typeof(DirtyListConverter<>))]
    internal sealed class DirtyList<T> : IList<T>, IReadOnlyList<T>, IDirtyList
    {
        private static readonly bool s_tIsIIdentifiable = TypeData<T>.TypeInfo.IsSubclassOf(TypeData<DirtyExtensibleObject>.Type);
        private static readonly string s_idPropertyName = s_tIsIIdentifiable ? DirtyExtensibleObject.GetIdPropertyName(TypeData<T>.TypeInfo) : string.Empty;

        internal readonly List<DirtyValue<T>> _list;
        private readonly Dictionary<string, WrapperObject<int>> _dictionary;

        public T this[int index]
        {
            get => _list[index];
            set
            {
                var existing = _list[index]._value;
                if (s_tIsIIdentifiable)
                {
                    var existingObj = (DirtyExtensibleObject)(object)existing;
                    if (existingObj != null)
                    {
                        var existingId = ((IIdentifiable)existingObj).Id;
                        if (!string.IsNullOrEmpty(existingId))
                        {
                            _dictionary.Remove(existingId);
                        }
                        existingObj.PropertyChanged -= PropertyChangedHandler;
                    }
                    var newObj = (DirtyExtensibleObject)(object)value;
                    if (newObj != null)
                    {
                        var newId = ((IIdentifiable)newObj).Id;
                        if (!string.IsNullOrEmpty(newId))
                        {
                            _dictionary[newId] = index;
                        }
                        newObj.PropertyChanged += PropertyChangedHandler;
                    }
                }
                _list[index] = value;
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, existing, index));
            }
        }

        public bool Dirty
        {
            get => _list.Any(item => item.Dirty);
            set
            {
                foreach (var dirtyValue in _list)
                {
                    dirtyValue.Dirty = value;
                }
            }
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        bool IList.IsFixedSize => ((IList)_list).IsFixedSize;

        bool ICollection.IsSynchronized => ((IList)_list).IsSynchronized;

        object ICollection.SyncRoot => ((IList)_list).SyncRoot;

        object IList.this[int index] { get => this[index]; set => this[index] = ValidateValue(value); }

        public DirtyList()
            : this(0)
        {
        }

        public DirtyList(int capacity)
        {
            _list = new List<DirtyValue<T>>(capacity);
            if (s_tIsIIdentifiable)
            {
                IEqualityComparer<string> comparer = StringComparer.OrdinalIgnoreCase;
                if (TypeData<T>.Type == TypeData<FieldLockData>.Type)
                {
                    comparer = new StringModelPathComparer();
                }
                _dictionary = new Dictionary<string, WrapperObject<int>>(comparer);
            }
        }

        public DirtyList(IEnumerable<T> collection)
            : this((collection as ICollection<T>)?.Count ?? 0)
        {
            foreach (var item in collection)
            {
                Add(item);
            }
        }

        public int IndexOf(string id) => s_tIsIIdentifiable && _dictionary.TryGetValue(id, out var wrapperObject) ? wrapperObject.Value : -1;

        public void Add(T item) => Insert(Count, item);

        public void Clear()
        {
            if (_list.Count > 0)
            {
                var list = CollectionChanged != null ? new List<T>() : null;
                if (list != null)
                {
                    foreach (var item in _list)
                    {
                        list.Add(item);
                    }
                }
                if (s_tIsIIdentifiable)
                {
                    for (var i = _list.Count - 1; i >= 0; --i)
                    {
                        RemoveAtInternal(i);
                    }
                }
                else
                {
                    _list.Clear();
                }
                if (list != null)
                {
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, list));
                }
            }
        }

        public bool Contains(T item) => IndexOf(item) >= 0;

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return item;
            }
        }

        public int IndexOf(T item)
        {
            var equalityComparer = EqualityComparer<T>.Default;
            if (s_tIsIIdentifiable && item != null)
            {
                var obj = (DirtyExtensibleObject)(object)item;
                var id = ((IIdentifiable)obj).Id;
                if (!string.IsNullOrEmpty(id))
                {
                    return _dictionary.TryGetValue(id, out var wrapperObject) && equalityComparer.Equals(_list[wrapperObject], item) ? wrapperObject.Value : -1;
                }
            }
            for (var i = 0; i < _list.Count; ++i)
            {
                if (equalityComparer.Equals(_list[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            _list.Insert(index, item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            if (s_tIsIIdentifiable)
            {
                if (index < _list.Count - 1)
                {
                    foreach (var pair in _dictionary)
                    {
                        if (pair.Value >= index)
                        {
                            ++pair.Value.Value;
                        }
                    }
                }
                var obj = (DirtyExtensibleObject)(object)item;
                if (obj != null)
                {
                    var id = ((IIdentifiable)obj)?.Id;
                    if (!string.IsNullOrEmpty(id))
                    {
                        _dictionary[id] = index;
                    }
                    obj.PropertyChanged += PropertyChangedHandler;
                }
            }
        }

        public void Move(int fromIndex, int toIndex)
        {
            if ((uint)fromIndex >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(fromIndex));
            }
            if ((uint)toIndex >= (uint)Count)
            {
                throw new ArgumentOutOfRangeException(nameof(toIndex));
            }

            if (fromIndex == toIndex)
            {
                return;
            }

            var temp = _list[fromIndex];
            var direction = fromIndex < toIndex ? 1 : -1;
            for (var i = fromIndex; i != toIndex; i += direction)
            {
                _list[i] = _list[i + direction];
            }
            _list[toIndex] = temp;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, temp, toIndex, fromIndex));
        }

        public bool Remove(T item)
        {
            var i = IndexOf(item);
            if (i >= 0)
            {
                RemoveAt(i);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            var item = RemoveAtInternal(index);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
        }

        private T RemoveAtInternal(int index)
        {
            var item = _list[index]._value;
            if (s_tIsIIdentifiable)
            {
                if (index < _list.Count - 1)
                {
                    foreach (var pair in _dictionary)
                    {
                        if (pair.Value > index)
                        {
                            --pair.Value.Value;
                        }
                    }
                }
                var obj = (DirtyExtensibleObject)(object)item;
                if (obj != null)
                {
                    var id = ((IIdentifiable)obj)?.Id;
                    if (!string.IsNullOrEmpty(id))
                    {
                        _dictionary.Remove(id);
                    }
                    obj.PropertyChanged -= PropertyChangedHandler;
                }
            }
            _list.RemoveAt(index);
            return item;
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == s_idPropertyName)
            {
                var eventArgs = (ValuePropertyChangedEventArgs)e;
                int? index = null;
                var priorId = (string)eventArgs.PriorValue;
                if (!string.IsNullOrEmpty(priorId))
                {
                    if (_dictionary.TryGetValue(priorId, out var wrapperObject))
                    {
                        index = wrapperObject;
                        _dictionary.Remove(priorId);
                    }
                }
                var newId = (string)eventArgs.NewValue;
                if (!string.IsNullOrEmpty(newId))
                {
                    if (!index.HasValue)
                    {
                        for (var i = 0; i < _list.Count; ++i)
                        {
                            if (_list[i] == sender)
                            {
                                index = i;
                                break;
                            }
                        }
                    }
                    _dictionary[newId] = index.GetValueOrDefault();
                }
            }
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            Preconditions.NotNull(array, nameof(array));
            Preconditions.LessThan(arrayIndex, nameof(arrayIndex), array.Length, $"{nameof(array)}.Length");
            Preconditions.GreaterThanOrEquals(array.Length - arrayIndex, $"{nameof(array)}.Length - {nameof(arrayIndex)}", Count, nameof(Count));

            for (var i = 0; i < _list.Count; ++i)
            {
                array[arrayIndex + i] = _list[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        int IList.Add(object value)
        {
            Add(ValidateValue(value));
            return Count - 1;
        }

        bool IList.Contains(object value) => Contains(ValidateValue(value));

        int IList.IndexOf(object value) => IndexOf(ValidateValue(value));

        void IList.Insert(int index, object value) => Insert(index, ValidateValue(value));

        void IList.Remove(object value) => Remove(ValidateValue(value));

        void ICollection.CopyTo(Array array, int index)
        {
            Preconditions.NotNull(array, nameof(array));
            Preconditions.GreaterThanOrEquals(index, nameof(index), 0);
            Preconditions.LessThanOrEquals(index + Count, $"{nameof(index)} + {nameof(Count)}", array.Length, $"{nameof(array)}.{nameof(array.Length)}");

            for (var i = 0; i < _list.Count; ++i)
            {
                array.SetValue(this[i], index + i);
            }
        }

        private T ValidateValue(object value) => value is T tValue ? tValue : throw new ArgumentException($"must be of type {TypeData<T>.Type.Name}", nameof(value));
    }

    internal sealed class DirtyListConverter<T> : JsonConverter
    {
        private static readonly bool s_serializeWholeList = (!TypeData<T>.TypeInfo.IsSubclassOf(TypeData<DirtyExtensibleObject>.Type)) || TypeData<T>.TypeInfo.GetCustomAttribute<EntityAttribute>(false)?.SerializeWholeListWhenDirty == true;
        private readonly bool _serializeWholeList;

        public DirtyListConverter()
        {
        }

        public DirtyListConverter(bool serializeWholeList)
        {
            _serializeWholeList = serializeWholeList;
        }

        public override bool CanConvert(Type objectType) => objectType == TypeData<DirtyList<T>>.Type;

        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => throw new NotSupportedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => serializer.Serialize(writer, (_serializeWholeList || s_serializeWholeList ? ((DirtyList<T>)value)._list : ((DirtyList<T>)value)._list.Where(item => item.Dirty)).Select(item => (T)item));
    }
}