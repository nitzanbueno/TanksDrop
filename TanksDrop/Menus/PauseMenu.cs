using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace TanksDrop.Menus
{
	class PauseMenu : Menu
	{
		MenuButton pauseButton;

		SpriteFont font;

		public PauseMenu( TanksDrop Game, ContentManager Content )
			: base( Game, Content )
		{
			pauseButton = new MenuButton( new Rectangle( Game.width / 2, Game.height - 100, 100, 50 ), "Resume" );
			font = Content.Load<SpriteFont>( "Score" );
		}

		public override bool Update( TimeSpan gameTime, KeyboardState Key, MouseState Mouse )
		{
			if ( new Rectangle( Mouse.X, Mouse.Y, 1, 1 ).Intersects( pauseButton.ButtonRectangle ) && Mouse.LeftButton == ButtonState.Pressed )
			{
				Game.currentMenu = null;
				return true;
			}

			prevKey = Key;
			prevMouse = Mouse;
			return false;
		}

		public override void Draw( TimeSpan gameTime, GraphicsDevice device, SpriteBatch spriteBatch )
		{
			Texture2D LightMap = new Texture2D( device, 3, 3, false, SurfaceFormat.Color );
			Color lmDark = Color.DarkSlateGray;
			Color lmBright = Color.White;
			LightMap.SetData<Color>( new[] { lmDark, lmDark, lmDark, lmDark, lmBright, lmDark, lmDark, lmDark, lmDark } );
			spriteBatch.Draw( LightMap, new Rectangle( 0, 0, Game.width, Game.height ), Color.White );
			spriteBatch.End();
			spriteBatch.Begin();
			spriteBatch.DrawString( font, pauseButton.Text, new Vector2( pauseButton.ButtonRectangle.X, pauseButton.ButtonRectangle.Y ), Color.White );
		}
	}
}
