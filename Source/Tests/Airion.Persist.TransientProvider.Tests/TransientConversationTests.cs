﻿// Copyright (c) Charles Weld
// This code is distributed under the GNU LGPL (for details please see ~\Documentation\license.txt)

using System;
using Airion.Persist.Tests.Contracts;
using MbUnit.Framework;

namespace Airion.Persist.TransientProvider.Tests
{
	[TestFixture]
	public class TransientConversationTests : ConversationTests
	{
		protected override IConfiguration BuildConfiguration()
		{
			return new TransientConfiguration();
		}
	}
}
