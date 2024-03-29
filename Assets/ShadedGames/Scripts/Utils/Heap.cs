﻿using System;

namespace ShadedGames.Scripts.Utils
{
    public interface IHeapItem<T> : IComparable<T>
    {
        /// <summary>
        /// Item index inside the heap.
        /// </summary>
        int HeapIndex { get; set; }
    }
    public class Heap<T> where T : IHeapItem<T>
    {

        private readonly T[] _items;

        public int Count { get; private set; }

        public Heap(int maxHeapSize)
        {
            _items = new T[maxHeapSize];
        }


public void Add(T item)
        {
            item.HeapIndex = Count;
            _items[Count] = item;
            SortUp(item);
            Count++;
        }

        /// <summary>
        /// Get the first item from the heap.
        /// </summary>
        /// <returns>The first item in the heap.</returns>
        public T GetFirst()
        {
            return _items[0];
        }

        /// <summary>
        /// Remove the first item from the heap.
        /// </summary>
        public void RemoveFirst()
        {
            Count--;
            _items[0] = _items[Count];
            _items[0].HeapIndex = 0;
            SortDown(_items[0]);
        }

        /// <summary>
        /// Updates an item in the heap.
        /// </summary>
        /// <param name="item">Item to update.</param>
        public void UpdateItem(T item)
        {
            SortUp(item);
        }

        /// <summary>
        /// Sort item down in heap.
        /// </summary>
        /// <param name="item">Item to sort.</param>
        private void SortDown(T item)
        {
            while (true)
            {
                var childIndexLeft = item.HeapIndex * 2 + 1;
                var childIndexRight = item.HeapIndex * 2 + 2;

                if (childIndexLeft < Count)
                {
                    var swapIndex = childIndexLeft;

                    if (childIndexRight < Count)
                        if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                            swapIndex = childIndexRight;

                    if (item.CompareTo(_items[swapIndex]) < 0)
                        Swap(item, _items[swapIndex]);
                    else
                        return;
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Sort item up in heap.
        /// </summary>
        /// <param name="item">Item to sort.</param>
        private void SortUp(T item)
        {
            var parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                var parentItem = _items[parentIndex];
                if (item.CompareTo(parentItem) > 0)
                    Swap(item, parentItem);
                else
                    break;

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        /// <summary>
        /// Swap to items in the heap.
        /// </summary>
        /// <param name="itemA">Item A.</param>
        /// <param name="itemB">Item B.</param>
        private void Swap(T itemA, T itemB)
        {
            _items[itemA.HeapIndex] = itemB;
            _items[itemB.HeapIndex] = itemA;
            var itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
        
    }
}