using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TanksDrop.Menu
{
	class MenuButton
	{
		public Rectangle ButtonRectangle;
		public bool isHovering;
		public Action<Game, MenuButton> OnClick;
		public Action<SpriteBatch, MenuButton> Draw;
		public MenuButton( Rectangle rect, Action<Game, MenuButton> click, Action<SpriteBatch, MenuButton> draw )
		{
			ButtonRectangle = rect;
			OnClick = click;
			Draw = draw;
		}
	}
}
