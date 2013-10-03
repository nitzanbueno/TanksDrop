using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.Projectiles
{
	class HomingBullet : ProjectileObject
	{
		public override float Scale
		{
			get
			{
				return 0.25F;
			}
		}

		public HomingBullet( Vector2 position, float angle, GameTime gameTime, int width, int height, float factor, TankObject owner )
			: base( position, angle, gameTime, width, height, factor, owner )
		{
		}

		public HomingBullet() : base() { }

		public override Texture2D LoadTex( ContentManager Content )
		{
			tex = Content.Load<Texture2D>( "Sprites\\RedBullet" );
			return tex;
		}

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups )
		{
			if ( ( gameTime.TotalGameTime - originalTime ).TotalMilliseconds > 2000 )
			{
				TankObject ClosestTank = null;
				float dist = float.PositiveInfinity;
				foreach ( TankObject Tank in Tanks )
				{
					float d = Vector2.Distance( Tank.Position, Position );
					if ( d < dist && Tank.IsInGame )
					{
						dist = d;
						ClosestTank = Tank;
					}
				}
				if ( ClosestTank != null )
				{
					float ang = ( ( ( float )Math.Atan2( Position.Y - ClosestTank.Position.Y, Position.X - ClosestTank.Position.X ) * 180 / ( float )Math.PI ) + 180 ) % 360;
					angle = ang;
				}
			}

			return UpdatePhysics( gameTime, Tanks, Fences ) ? true : base.Update( gameTime, Tanks, Fences, Pickups );
		}

		public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
		{
			spriteBatch.Draw( tex, Position, null, Color.White, 0F, new Vector2( 16, 16 ), Scale, SpriteEffects.None, 0 );
		}
	}
}
