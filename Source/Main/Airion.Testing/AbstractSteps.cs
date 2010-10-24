// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Common;

namespace Airion.Testing
{
	public class AbstractSteps : LightDisposableBase
	{
		public AbstractSteps()
		{
			BeforeScenario();
		}
		
		protected override void Dispose(bool disposing)
		{
			if(disposing) {
				AfterScenario();
			}
			base.Dispose(disposing);
		}
		
		protected virtual void BeforeScenario()
		{			
		}
		
		protected virtual void AfterScenario()
		{
			
		}
	}
}
