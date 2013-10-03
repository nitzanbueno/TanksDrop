using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TanksDrop.PowerUps;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework;

namespace TanksDrop.PowerUps
{
	class Switcher : InstantPowerUp
	{
		public Switcher( GameTime gameTime ) : base( gameTime ) { }

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\Switcher" );
		}

		public override bool Update( GameTime gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			return base.Update( gameTime, Tanks, Projectiles, Fences );
		}

		public override void Use( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			TankObject ClosestTank = Owner;
			float ShortestDistance = float.PositiveInfinity;
			foreach ( TankObject Tank in Tanks )
			{
				float dist = Vector2.Distance( Owner.Position, Tank.Position );
				if ( dist < ShortestDistance && !Tank.Equals( Owner ) && Tank.IsInGame )
				{
					ShortestDistance = dist;
					ClosestTank = Tank;
				}
			}
			Vector2 OwnerPos = Owner.Position;
			float OwnerRot = Owner.Rotation;
			bool isOwnerInGame = Owner.IsInGame;
			float OwnerScale = Owner.Scale;
			TimedPowerUp p = Owner.powerUp;
			TimedPowerUp n = ClosestTank.powerUp;
			Owner.Position = ClosestTank.Position;
			Owner.Rotation = ClosestTank.Rotation;
			if ( Owner.powerUp != null )
			{
				Owner.powerUp.Revert();
				Owner.powerUp = null;
			}
			Owner.Scale = ClosestTank.Scale;
			Owner.IsInGame = ClosestTank.IsInGame;
			if ( n != null )
			{
				Owner.AppendPowerUp( n, gameTime );
			}
			ClosestTank.Position = OwnerPos;
			ClosestTank.Rotation = OwnerRot;
			if ( ClosestTank.powerUp != null )
			{
				ClosestTank.powerUp.Revert();
				ClosestTank.powerUp = null;
			}
			if ( p != null )
			{
				ClosestTank.AppendPowerUp( p, gameTime );
			}
			ClosestTank.Scale = OwnerScale;
			ClosestTank.IsInGame = isOwnerInGame;
		}
	}
}
