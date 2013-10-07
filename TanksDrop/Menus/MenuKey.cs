using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TanksDrop.Menus
{
	class MenuKey
	{
		public Keys Key;
		public Action<TanksDrop> OnPress;
		public MenuKey( Keys Key, Action<TanksDrop> OnPress )
		{
			this.Key = Key;
			this.OnPress = OnPress;
		}
	}
}
