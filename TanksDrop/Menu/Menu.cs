using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TanksDrop.Menu
{
	class Menu
	{
		MenuButton[] Buttons;
		MenuKey[] Keys;
		TanksDrop Game;

		public Menu( TanksDrop Game )
		{
			this.Game = Game;
		}

		MouseState prevMouse;
		KeyboardState prevKey;
		public void Update( GameTime gameTime, KeyboardState Key, MouseState Mouse )
		{
			if ( Buttons.Length > 0 )
			{
				foreach ( MenuButton Button in Buttons )
				{
					Button.isHovering = new Rectangle( Mouse.X, Mouse.Y, 1, 1 ).Intersects( Button.ButtonRectangle );
					if ( Button.isHovering && Mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released )
					{
						Button.OnClick( Game, Button );
					}
				}
			}
			prevMouse = Mouse;
			prevKey = Key;
		}
	}
}
