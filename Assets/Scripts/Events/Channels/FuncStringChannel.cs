﻿using System;
using UnityEngine;

namespace Events.Channels
{
	[CreateAssetMenu(menuName = "Event Channels/Data Channels/Func<String>", fileName = "FuncStringChannel")]
	public class FuncStringChannel : ChannelSo<Func<string>>
	{
	}
}
