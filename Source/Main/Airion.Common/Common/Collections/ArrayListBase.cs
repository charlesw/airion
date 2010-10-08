// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airion.Common.Collections
{
    public abstract class ArrayListBase<T> : ListBase<T>, ICloneable
    {
    	public ArrayListBase()
    		: this(10)
    	{    		
    	}
    	
    	public ArrayListBase(int capacity)
    	{    		
    		Guard.RequireGreaterThan("capcity", capacity, 0, true);
    		this.array = new T[capacity];
    		this.firstIndex = 0;
    		this.lastIndex = 0;
    	}
    	
    	public ArrayListBase(IEnumerable<T> enumerable)
    	{
    		if(enumerable is ICollection<T>) {
    			ICollection<T> collection = (ICollection<T>)enumerable;
    			this.array = new T[collection.Count];
    		}
    		foreach(T item in enumerable) {
    			Add(item);
    		}
    	}
    	
		#region Methods 

		// Public Methods 
		
		public object Clone()
		{
			ArrayListBase<T> clone = (ArrayListBase<T>)this.MemberwiseClone();
			TransferMembers(clone);
			return clone;
		}
		
		protected virtual void TransferMembers(ArrayListBase<T> destinationClone)
		{
			destinationClone.array = (T[])array.Clone();
		}

        public override void Clear()
        {
        	UpdateVersion();
            if (firstIndex != lastIndex) {
                ArrayUtilities.FillRange(array, firstIndex, lastIndex-1, default(T));
                firstIndex = lastIndex = 0;
                modCount++;
            }
        }

        public override void Insert(int index, T item)
        {
        	UpdateVersion();
            int size = lastIndex - firstIndex;

            if (0 < index && index < size) {
                if (firstIndex == 0 && lastIndex == array.Length) {
                    GrowForInsert(index, 1);
                } else if ((index < size / 2 && firstIndex > 0) || lastIndex == array.Length) {
                    Array.Copy(array, firstIndex, array, --firstIndex, index);
                } else {
                    int actualIndex = index + firstIndex;
                    Array.Copy(array, actualIndex, array, index + 1, size - index);
                    lastIndex++;
                }
                array[index + firstIndex] = item;
            } else if (index == 0) {
                if (firstIndex == 0) {
                    GrowAtFront(1);
                }
                array[--firstIndex] = item;
            } else if (index == size) {
                if (lastIndex == array.Length) {
                    GrowAtEnd(1);
                }
                array[lastIndex++] = item;
            } else {
                throw new ArgumentOutOfRangeException("index", String.Format("Argument 'index': must be between {0} and {1}, but is {2}.", 0, size, index));
            }

            modCount++;
        }

        public override void RemoveAt(int index)
        {
        	UpdateVersion();
            int size = lastIndex - firstIndex;
            Guard.RequireBetween("index", index, 0, size, true, false);

            if (index == size - 1) {
                array[--lastIndex] = default(T);
            } else if (index == 0) {
                array[firstIndex++] = default(T);
            } else {
                int elementIndex = firstIndex + index;
                if (index < size / 2) {
                    Array.Copy(array, firstIndex, array, firstIndex + 1, index);
                    array[firstIndex++] = default(T);
                } else {
                    Array.Copy(array, elementIndex + 1, array, elementIndex, size - index - 1);
                    array[--lastIndex] = default(T);
                }
            }

            modCount++;
        }

		// Private Methods 

        private void GrowAtEnd(int required)
        {
            int size = lastIndex - firstIndex;
            if (firstIndex >= required - (array.Length - lastIndex)) {
                int newLast = lastIndex - firstIndex;
                if (size > 0) {
                    Array.Copy(array, firstIndex, array, 0, size);
                    int start = newLast < firstIndex ? firstIndex : newLast;
                    ArrayUtilities.FillRange(array, start, array.Length-start, default(T));
                }
                firstIndex = 0;
                lastIndex = newLast;
            } else {
                int increment = size / 2;
                if (required > increment) {
                    increment = required;
                }
                if (increment < 12) {
                    increment = 12;
                }
                T[] newArray = new T[size + increment];
                if (size > 0) {
                    Array.Copy(array, firstIndex, newArray, 0, size);
                    firstIndex = 0;
                    lastIndex = size;
                }
                array = newArray;
            }
        }

        private void GrowAtFront(int required)
        {
            int size = lastIndex - firstIndex;
            if (array.Length - lastIndex + firstIndex >= required) {
                int newFirst = array.Length - size;
                if (size > 0) {
                    Array.Copy(array, firstIndex, array, newFirst, size);
                    int length = firstIndex + size > newFirst ? newFirst : firstIndex + size;
                    ArrayUtilities.FillRange(array, firstIndex, length, default(T));
                }
                firstIndex = newFirst;
                lastIndex = array.Length;
            } else {
                int increment = size / 2;
                if (required > increment) {
                    increment = required;
                }
                if (increment < 12) {
                    increment = 12;
                }
                T[] newArray = new T[size + increment];
                if (size > 0) {
                    Array.Copy(array, firstIndex, newArray, newArray.Length - size, size);
                }
                firstIndex = newArray.Length - size;
                lastIndex = newArray.Length;
                array = newArray;
            }
        }

        private void GrowForInsert(int location, int required)
        {
            int size = lastIndex - firstIndex;
            int increment = size / 2;
            if (required > increment) {
                increment = required;
            }
            if (increment < 12) {
                increment = 12;
            }

            T[] newArray = new T[size + increment];
            if (location < size / 2) {
                int newFirst = newArray.Length - (size + required);
                Array.Copy(array, location, newArray, location + increment, size - location);
                Array.Copy(array, firstIndex, newArray, newFirst, location);
                firstIndex = newFirst;
                lastIndex = newArray.Length;
            } else {
                Array.Copy(array, firstIndex, newArray, 0, location);
                Array.Copy(array, location, newArray, location + required, size - location);
                firstIndex = 0;
                lastIndex += required;
            }
            array = newArray;
        }


		#endregion Methods 

		#region Properties 
		
		public int Capacity {
			get { return array.Length; }
		}
		
        public override int Count
        {
            get { return lastIndex - firstIndex; }
        }

        public override T this[int index]
        {
            get
            {
                Guard.RequireBetween("index", index, 0, lastIndex - firstIndex, true, false);

                return array[firstIndex + index];
            }
            set
            {
                Guard.RequireBetween("index", index, 0, lastIndex - firstIndex, true, false);
                UpdateVersion();
        	
                array[firstIndex + index] = value;
            }
        }

		#endregion Properties 

		#region Fields 

        private T[] array;
        private int firstIndex;
        private int lastIndex;
        private int modCount = 0;

		#endregion Fields 
    }
}
