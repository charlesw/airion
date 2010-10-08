// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airion.Common.Collections
{
	public abstract class ImposterListBase<TImposter, TItem> : ListBase<TImposter>
	{
		#region Constructors (2)

		public ImposterListBase(IEnumerable<TImposter> items)
			: base()
		{
			foreach (TImposter item in items) {
				this.Add(item);
			}
		}

		public ImposterListBase()
			: base()
		{
		}

		#endregion Constructors

		#region Methods (11)

		// Public Methods (3)

		public override void Clear()
		{
			UpdateVersion();
			if (firstIndex != lastIndex) {
				ArrayUtilities.FillRange(array, firstIndex, lastIndex, default(TItem));
				firstIndex = lastIndex = 0;
				modCount++;
			}
		}

		public override void Insert(int index, TImposter item)
		{
			Insert(index, Convert(item));
		}

		public override void RemoveAt(int index)
		{
			int size = lastIndex - firstIndex;
			Guard.RequireBetween("index", index, 0, size, true, false);

			if (index == size - 1) {
				array[lastIndex] = default(TItem);
			} else if (index == 0) {
				array[firstIndex++] = default(TItem);
			} else {
				int elementIndex = firstIndex + index;
				if (index < size / 2) {
					Array.Copy(array, firstIndex, array, firstIndex + 1, index);
					array[firstIndex++] = default(TItem);
				} else {
					Array.Copy(array, elementIndex + 1, array, elementIndex, size - index - 1);
					array[--lastIndex] = default(TItem);
				}
			}

			modCount++;
		}
		// Protected Methods (5)

		protected abstract TImposter Convert(TItem item);

		protected abstract TItem Convert(TImposter imposter);

		protected TItem GetItem(int index)
		{
			Guard.RequireBetween("index", index, 0, lastIndex - firstIndex, true, false);

			return array[firstIndex + index];
		}

		protected void Insert(int index, TItem item)
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

		protected void SetItem(int index, TItem value)
		{
			Guard.RequireBetween("index", index, 0, lastIndex - firstIndex, true, false);
			UpdateVersion();
			
			array[firstIndex + index] = value;
		}
		// Private Methods (3)

		private void GrowAtEnd(int required)
		{
			int size = lastIndex - firstIndex;
			if (firstIndex >= required - (array.Length - lastIndex)) {
				int newLast = lastIndex - firstIndex;
				if (size > 0) {
					Array.Copy(array, firstIndex, array, 0, size);
					int start = newLast < firstIndex ? firstIndex : newLast;
					ArrayUtilities.FillRange(array, start, array.Length, default(TItem));
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
				TItem[] newArray = new TItem[size + increment];
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
					ArrayUtilities.FillRange(array, firstIndex, length, default(TItem));
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
				TItem[] newArray = new TItem[size + increment];
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

			TItem[] newArray = new TItem[size + increment];
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

		#region Properties (3)

		public int Capacity
		{
			get { return array.Length - firstIndex; }
			set
			{
				Guard.RequireGreaterThan("value", value, Count);
				
				GrowAtEnd(value - array.Length - firstIndex);
			}
		}

		public override int Count
		{
			get { return lastIndex - firstIndex; }
		}

		public override TImposter this[int index]
		{
			get
			{
				return Convert(GetItem(index));
			}
			set
			{
				SetItem(index, Convert(value));
			}
		}

		#endregion Properties

		#region Fields (4)

		private TItem[] array;
		private int firstIndex;
		private int lastIndex;
		private int modCount = 0;

		#endregion Fields
	}
}
