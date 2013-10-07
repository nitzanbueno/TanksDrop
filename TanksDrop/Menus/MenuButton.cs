using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TanksDrop.Menus
{
	class MenuButton
	{
		public Rectangle ButtonRectangle;
		public string Text;
		public MenuButton( Rectangle rect, string text )
		{
			ButtonRectangle = rect;
			Text = text;
		}
	}
}
