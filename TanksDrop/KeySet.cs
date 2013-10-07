using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TanksDrop
{
	/// <summary>
	/// A Class that has 6 keys - used with tanks to create a set of keys the tank can read.
	/// </summary>
	class KeySet
	{
		public Keys KeyUp;
		public Keys KeyDown;
		public Keys KeyLeft;
		public Keys KeyRight;
		public Keys KeyPlace;
		public Keys KeyShoot;

		public KeySet( Keys up, Keys down, Keys left, Keys right, Keys place, Keys shoot )
		{
			KeyUp = up;
			KeyDown = down;
			KeyLeft = left;
			KeyRight = right;
			KeyPlace = place;
			KeyShoot = shoot;
		}
	}
}
