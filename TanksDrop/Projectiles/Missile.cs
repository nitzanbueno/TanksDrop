using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TanksDrop.Projectiles
{
	/// <summary>
	/// A projectile that explodes when despawning. An explosion kills any tank getting close to it.
	/// </summary>
	class Missile : ProjectileObject
	{
		bool isExploded;
		Texture2D BoomTex;
		TimeSpan explosionTime;
		float BoomScale;
		Random random;

		public Missile( Vector2 position, float angle, GameTime gameTime, int width, int height, float factor, TankObject owner )
			: base( position, angle, gameTime, width, height, factor, owner )
		{
			random = new Random();
			if ( random.Next( 100 ) < 10 )
			{
				Explode( gameTime );
			}
		}

		public Missile()
			: base()
		{

		}

		public Missile( Vector2 position, float angle, GameTime gameTime, int width, int height, float factor, TankObject owner, bool toExplode )
			: base( position, angle, gameTime, width, height, factor, owner )
		{
			random = new Random();
			if ( toExplode )
			{
				Explode( gameTime );
			}
		}

		public override float Scale
		{
			get { return 0.5F; }
		}

		public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
		{
			if ( isExploded )
			{
				spriteBatch.Draw( BoomTex, Position, null, Color.White, 0, new Vector2( 32, 32 ), BoomScale, SpriteEffects.None, 0 );
			}
			else
			{
				spriteBatch.Draw( tex, Position, null, Color.White, MathHelper.ToRadians( angle + 90 ), new Vector2( 16, 16 ), Scale, SpriteEffects.None, 0.1F );
			}
		}

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups )
		{
			bool shouldBeDestroyed = false;
			if ( !isExploded )
			{
				shouldBeDestroyed = UpdatePhysics( gameTime, Tanks, Fences, false );
			}
			else
			{
				foreach ( TankObject Tank in Tanks )
				{
					if ( Vector2.Distance( Tank.Position, Position ) < 32 * BoomScale )
					{
						Tank.IsInGame = false;
					}
				}
			}
			shouldBeDestroyed = shouldBeDestroyed ? true : base.Update( gameTime, Tanks, Fences, Pickups );
			if ( shouldBeDestroyed && !isExploded )
			{
				Explode( gameTime );
			}

			return isExploded && ( gameTime.TotalGameTime - explosionTime ).TotalMilliseconds > 2000;
		}

		/// <summary>
		/// Sets the blast radius.
		/// </summary>
		/// <param name="newBlastRadius">The new blast radius.</param>
		public void changeBlastRadius( float newBlastRadius )
		{
			BoomScale = newBlastRadius;
		}

		/// <summary>
		/// Explodes. The size of the explosion is set by the BlastRadius setting.
		/// </summary>
		/// <param name="gameTime">The current game time.</param>
		private void Explode( GameTime gameTime )
		{
			isExploded = true;
			explosionTime = gameTime.TotalGameTime;
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			tex = Content.Load<Texture2D>( "Sprites\\Missile" );
			BoomTex = Content.Load<Texture2D>( "Sprites\\Boom" );
			return tex;
		}
	}
}
