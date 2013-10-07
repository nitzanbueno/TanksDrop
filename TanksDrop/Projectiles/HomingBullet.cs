using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.Projectiles
{
	/// <summary>
	/// A bullet that is colored red and after 2 seconds homes onto the closest tank to it.
	/// </summary>
	class HomingBullet : ProjectileObject
	{
		public override float Scale
		{
			get
			{
				return 0.25F;
			}
		}

		public HomingBullet( Vector2 position, float angle, TimeSpan gameTime, int width, int height, float factor, TankObject owner )
			: base( position, angle, gameTime, width, height, factor, owner )
		{
		}

		public HomingBullet() : base() { }

		public override Texture2D LoadTex( ContentManager Content )
		{
			tex = Content.Load<Texture2D>( "Sprites\\RedBullet" );
			return tex;
		}

		public override bool Update( TimeSpan gameTime, TankObject[] Tanks, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups )
		{
			if ( ( gameTime - originalTime ).TotalMilliseconds > 2000 ) //If I should start homing,
			{
				TankObject ClosestTank = null; // I find the closest tank to me
				float dist = float.PositiveInfinity;
				foreach ( TankObject Tank in Tanks )
				{
					float d = Vector2.Distance( Tank.Position, Position ); // by taking the distance of every tank
					if ( d < dist && Tank.IsInGame ) // and finding the shortest.
					{
						dist = d;
						ClosestTank = Tank;
					}
				}
				if ( ClosestTank != null ) // Then if there is a tank i can home onto,
				{
					float ang = ( ( ( float )Math.Atan2( Position.Y - ClosestTank.Position.Y, Position.X - ClosestTank.Position.X ) * 180 / ( float )Math.PI ) + 180 ) % 360; // I home onto it by finding my angle from the tank
					angle = ang; // and changing my flight angle into that.
					// I know there are better ways of homing - I just don't know how to use them. 
					// If anyone could put a code that makes the homing bullet change only 5 degrees a second, that would be great.
				}
			}

			return UpdatePhysics( gameTime, Tanks, Fences ) ? true : base.Update( gameTime, Tanks, Fences, Pickups );
		}

		public override void Draw( TimeSpan gameTime, SpriteBatch spriteBatch )
		{
			spriteBatch.Draw( tex, Position, null, Color.White, 0F, new Vector2( 16, 16 ), Scale, SpriteEffects.None, 0 );
		}
	}
}
