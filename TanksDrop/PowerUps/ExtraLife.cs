using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TanksDrop.Projectiles;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// A timed power-up inspired by Fun Run's extra life that creates a spinning red halo around its taker that, when the tank should be killed, disappears and keeps its taker alive.
	/// If the taker got hit by a basic bullet, it turns it 180 degrees.
	/// It also lasts indefinetly so it is not really 'timed'.
	/// </summary>
	/// <seealso cref="ForceField"/>
	class ExtraLife : TimedPowerUp
	{
		bool didHit;
		Texture2D Halo;
		float rot;

		public ExtraLife( GameTime gameTime )
			: base( gameTime, int.MaxValue )
		{
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			Halo = Content.Load<Texture2D>( "Sprites\\ExtraAlive" );
			return Content.Load<Texture2D>( "Sprites\\ExtraLife" );
		}

		public override void DoPickup( TankObject[] Tanks, HashSet<Projectiles.ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			//
		}

		public override void Revert()
		{
			//
		}

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<Projectiles.ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			rot += MathHelper.ToRadians( 10 );
			base.Update( gameTime, Tanks, Projectiles, Fences );
			return didHit;
		}

		public override bool BulletHit( ProjectileObject Proj, TankObject Requestor )
		{
			if ( Requestor == Owner )
			{
				didHit = true;
				if ( Proj is BasicBullet )
				{
					Proj.Turn( 180 );
					Proj.Position = GameTools.ReturnMove( 10, Proj.Angle, Proj.Position );
				}
				else
				{
					return true;
				}
				return false;
			}
			else
			{
				Requestor.IsInGame = false;
				return true;
			}
		}

		public override void Draw( SpriteBatch spriteBatch, GameTime gameTime )
		{
			if ( Taken )
			{
				spriteBatch.Draw( Halo, Owner.Position, null, Color.White, rot, new Vector2( 16, 16 ), Owner.Scale * 1.5F, SpriteEffects.None, 1 );
			}
		}

		public override bool Hit( TankObject Hitten )
		{
			if ( !didHit )
			{
				didHit = true;
				return false;
			}
			else return true;
			
		}
	}
}
