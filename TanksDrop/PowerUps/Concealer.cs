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
	/// A timed power-up that causes its taker to turn into a 'clone' of another living tank, and either switches them or not.
	/// When a tank is killed, it is unknown which one it was until the power-up's effects end.
	/// </summary>
	class Concealer : TimedPowerUp
	{
		bool didSwitch;
		bool toSwitch;
		Colors origColor;
		KeySet origKeys;
		TankObject SwitchedTank;

		public Concealer( TimeSpan gameTime )
			: base( gameTime, 10000 )
		{
			toSwitch = random.Next( 2 ) == 0;
			if ( Owner != null )
			{
				SwitchedTank = Owner;
				origColor = Owner.TankColor;
				origKeys = Owner.Keys;
			}
		}

		public override Texture2D LoadTex( ContentManager Content )
		{
			return Content.Load<Texture2D>( "Sprites\\Concealer" );
		}

		public override void DoPickup( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			if ( SwitchedTank == null )
			{
				SwitchedTank = Owner;
				origColor = Owner.TankColor;
				origKeys = Owner.Keys;
			}
			if ( !didSwitch && Tanks.Count<TankObject>(x => x.IsInGame) > 1)
			{
				HashSet<TankObject> TankSet = new HashSet<TankObject>( Tanks );
				foreach ( TankObject tank in Tanks )
				{
					if ( !tank.IsInGame || tank == Owner )
					{
						TankSet.Remove( tank );
					}
				}
				SwitchedTank = ( TankSet.ToArray<TankObject>() )[ random.Next( TankSet.Count ) ];
				if ( toSwitch )
				{
					Vector2 OwnerPos = Owner.Position;
					float OwnerRot = Owner.Rotation;
					float OwnerScale = Owner.Scale;
					Owner.Position = SwitchedTank.Position;
					Owner.Rotation = SwitchedTank.Rotation;
					Owner.Scale = SwitchedTank.Scale;
					SwitchedTank.Position = OwnerPos;
					SwitchedTank.Rotation = OwnerRot;
					SwitchedTank.Scale = OwnerScale;
				}
				didSwitch = true;
			}
			Owner.TankColor = SwitchedTank.TankColor;
			Owner.Keys = SwitchedTank.Keys;
		}

		public override void Revert()
		{
			Owner.TankColor = origColor;
			Owner.Keys = origKeys;
		}
	}
}