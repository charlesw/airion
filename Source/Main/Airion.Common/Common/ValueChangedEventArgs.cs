// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Airion.Common
{
    public class ValueChangedEventArgs<T> : EventArgs
    {

        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T NewValue { get; private set; }

        public T OldValue { get; private set; }
    }
}
