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
	/// The basic black bullet that gets deflected off of fences and kills the first tank it touches then despawns.
	/// Also despawns after 10 seconds (or the ShotTimeLimit setting in milliseconds).
	/// </summary>
	class BasicBullet : ProjectileObject
	{
		/*public Vector2 Position;
		protected float xAdvance;
		protected float yAdvance;
		protected TimeSpan originalTime;
		protected int width;
		protected int height;
		protected float angle;
		public float Scale;
		protected int bWidth
		{
			get
			{
				return ( int )( 32 * Scale );
			}
		}
		protected int bHeight
		{
			get
			{
				return ( int )( 32 * Scale );
			}
		}
		protected float factor;
		public TankObject Owner;*/

		public override float Scale
		{
			get { return 0.25F; }
		}

		public BasicBullet( Vector2 position, float angle, GameTime gameTime, int width, int height, float factor, TankObject owner )
			: base( position, angle, gameTime, width, height, factor, owner )
		{
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			tex = Content.Load<Texture2D>( "Sprites\\Bullet" );
			return tex;
		}

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups )
		{
			return UpdatePhysics( gameTime, Tanks, Fences ) ? true : base.Update( gameTime, Tanks, Fences, Pickups );
		}

		public override void Draw( GameTime gameTime, SpriteBatch spriteBatch )
		{
			spriteBatch.Draw( tex, Position, null, Color.Black, 0F, new Vector2( 16, 16 ), Scale, SpriteEffects.None, 0 );
		}
	}
}
