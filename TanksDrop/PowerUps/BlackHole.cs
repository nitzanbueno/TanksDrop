using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// An appearing power-up that sucks all projectiles on board (with a limit of 50 projectiles) and shoots them at random directions.
	/// </summary>
	class BlackHole : AppearingPowerUp
	{
		protected Vector2 position;
		protected bool didExplode;
		protected Texture2D tex;
		protected float speed;
		protected HashSet<ProjectileObject> suckedProjectiles;

		public BlackHole( TimeSpan gameTime )
			: base( gameTime, -1 )
		{
			speed = 0.1F;
			suckedProjectiles = new HashSet<ProjectileObject>();
		}

		public override void LoadTex( ContentManager Content )
		{
			tex = Content.Load<Texture2D>( "Sprites\\BlackHole" );
		}

		public override void Draw( SpriteBatch spriteBatch, TimeSpan gameTime )
		{
			spriteBatch.Draw( tex, position, null, Color.White, 0, new Vector2( 16, 16 ), 1, SpriteEffects.None, 0 );
		}

		public override bool Update( TimeSpan gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			if ( position == Vector2.Zero )
			{
				position = GameTools.GetRandomPos( Projectiles );
				//position = GetWH( Projectiles ) / 2;
			}
			return didExplode;
		}

		public override bool UpdateProjectiles( TimeSpan gameTime, HashSet<ProjectileObject> Projectiles )
		{
			if ( speed < 30 )
			{
				speed += 0.1F;
			}
			if ( !didExplode )
			{
				bool areAllProjesInside = true;
				foreach ( ProjectileObject Proj in Projectiles )
				{
					if ( Vector2.Distance( Proj.Position, position ) > 16 && !( Proj is AProj ) )
					{
						float ang = ( ( ( float )Math.Atan2( position.Y - Proj.Position.Y, position.X - Proj.Position.X ) * 180 / ( float )Math.PI ) ) % 360;
						Proj.Position = GameTools.ReturnMove( speed, ang, Proj.Position );
						areAllProjesInside = false;
					}
					else if ( !( Proj is AProj ) )
					{
						suckedProjectiles.Add( Proj );
						suckedProjectiles.Add( Proj );
						suckedProjectiles.Add( Proj );
						Projectiles.Add( Proj );
						Projectiles.Add( Proj );
						Proj.Position = position;
					}
				}
				if ( areAllProjesInside || suckedProjectiles.Count > 50 )
				{
					Explode( gameTime, Projectiles );
				}
				return false;
			}
			return true;
		}

		private void Explode( TimeSpan gameTime, HashSet<ProjectileObject> Projectiles )
		{
			didExplode = true;
			foreach ( ProjectileObject Proj in suckedProjectiles )
			{
				Proj.Twist( random.Next( 360 ) );
			}
		}

	}
}
