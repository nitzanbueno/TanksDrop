using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanksDrop.PowerUps;
using Microsoft.Xna.Framework;
using TanksDrop.Projectiles;

namespace TanksDrop.SuddenDeaths
{
	/// <summary>
	/// A 'sudden death power-up' that's only used with the <see cref="SuperNoveyDeath"/> to create an unavoidable circle of bullets.
	/// </summary>
	class SuperNoveyBlackHole : BlackHole
	{
		public SuperNoveyBlackHole( TimeSpan gameTime ) : base( gameTime )
		{
		}

		public override bool Update( TimeSpan gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			if ( position == Vector2.Zero )
			{
				position = GameTools.GetWH( Projectiles ) / 2;
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
				if ( areAllProjesInside )
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
			float ang = 0;
			foreach ( ProjectileObject Proj in suckedProjectiles )
			{
				Proj.Twist( ang );
				ang += 0.5F;
			}
		}
	}
}
