using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.Menus
{
	public abstract class Menu
	{
		protected TanksDrop Game;

		public Menu( TanksDrop Game, ContentManager Content )
		{
			this.Game = Game;
		}

		protected MouseState prevMouse;
		protected KeyboardState prevKey;
		public abstract bool Update( TimeSpan gameTime, KeyboardState Key, MouseState Mouse );

		public abstract void Draw( TimeSpan gameTime, GraphicsDevice device, SpriteBatch spriteBatch );
	}
}
