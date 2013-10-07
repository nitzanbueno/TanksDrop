using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// A timed power-up that creates a blue aura around its taker for 5 seconds, keeping it alive and deflecting basic bullets.
	/// </summary>
	/// <seealso cref="ExtraLife"/>
	class ForceField : TimedPowerUp
	{
		Texture2D Aura;

		public ForceField( GameTime gameTime )
			: base( gameTime, 5000 )
		{
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			/*Texture2D n*/Aura = Content.Load<Texture2D>( "Sprites\\ForceField" );
			/*Color[] c = new Color[ nHalo.Width * nHalo.Height ];
			Color[] nc = new Color[ nHalo.Width * nHalo.Height ];
			nHalo.GetData<Color>( c );
			Halo = nHalo;
			for ( int i = 0; i < c.Length; i++ )
			{
				Color cc = c[ i ];
				if ( cc.A > 0 ) cc.A = 10;
				nc[ i ] = cc;
			}
			Halo.SetData<Color>( nc );*/
			return Aura;
		}

		public override void DoPickup( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			//
		}

		public override void Revert()
		{
			//
		}

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			return base.Update( gameTime, Tanks, Projectiles, Fences );
		}

		public override bool BulletHit( ProjectileObject Proj, TankObject Requestor )
		{
			if ( Requestor == Owner )
			{
				Proj.Turn( 180 );
				return false;
			}
			else
			{
				Requestor.IsInGame = false;
				return true;
			}
		}

		public override bool Hit( TankObject Hitten )
		{
			return false;
		}

		public override void Draw( SpriteBatch spriteBatch, GameTime gameTime )
		{
			Color c = Color.White;
			if ( Taken )
			{
				spriteBatch.Draw( Aura, Owner.Position, null, c, 0, new Vector2( 16, 16 ), Owner.Scale * 1.5F, SpriteEffects.None, 0.9F );
			}
		}
	}
}
