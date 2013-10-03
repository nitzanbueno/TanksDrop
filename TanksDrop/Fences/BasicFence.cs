using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TanksDrop
{
	class BasicFence : FenceObject
	{
		public BasicFence( Vector2 p1, Vector2 p2, float width, TankObject owner, GameTime gameTime, GraphicsDevice gd ) : base( p1, p2, width, owner, gameTime, gd )
		{
		}

		public override void Draw( SpriteBatch spriteBatch )
		{
			float angle = ( float )Math.Atan2( Point2.Y - Point1.Y, Point2.X - Point1.X );
			float length = Vector2.Distance( Point1, Point2 );

			spriteBatch.Draw( blank, Point1, null, color,
					   angle, new Vector2( 0, 1 ), new Vector2( length/2, Width/2 ),
					   SpriteEffects.None, 0 );
		}
	}
}
