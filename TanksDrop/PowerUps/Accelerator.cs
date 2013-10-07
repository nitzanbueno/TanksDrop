using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using TanksDrop.Projectiles;

namespace TanksDrop.PowerUps
{
	/// <summary>
	/// An Appearing Power-up that causes a tank that steps on it to go 20x its speed for 0.1 seconds.
	/// </summary>
	class Accelerator : AppearingPowerUp
	{
		/// <summary>
		/// The accelerator's texture.
		/// </summary>
		Texture2D tex;

		/// <summary>
		/// The accelerator's position.
		/// </summary>
		Vector2 Position;

		/// <summary>
		/// The tanks that are accelerated at the current tick.
		/// </summary>
		HashSet<Tuple<TankObject, TimeSpan>> TanksToAffect;

		/// <summary>
		/// Constructs a new accelerator object.
		/// </summary>
		/// <param name="gameTime">The current game time.</param>
		public Accelerator( TimeSpan gameTime )
			: base( gameTime, 10000 )
		{
			TanksToAffect = new HashSet<Tuple<TankObject, TimeSpan>>();
		}

		public override void LoadTex( ContentManager Content )
		{
			tex = Content.Load<Texture2D>( "Sprites\\Accelerator" );
		}

		public override bool Update( TimeSpan gameTime, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences )
		{
			if ( Position == Vector2.Zero )
			{
				Position = GameTools.GetRandomPos( Projectiles );
			}
			foreach ( TankObject t in Tanks )
			{
				var T = new Tuple<TankObject, TimeSpan>( t, gameTime );
				bool i = TanksToAffect.Any<Tuple<TankObject, TimeSpan>>
					( x =>
					{
						return x.Item1 == t;
					}
					);
				if ( Vector2.Distance( t.Position, Position ) < 16 * Scale && !i )
				{
					T.Item1.Speed = T.Item1.OSP * 10;
					TanksToAffect.Add( T );
				}
			}
			HashSet<Tuple<TankObject, TimeSpan>> TanksToRemove = new HashSet<Tuple<TankObject, TimeSpan>>();
			foreach ( var t in TanksToAffect )
			{
				if ( ( gameTime - t.Item2 ).TotalMilliseconds > 100 )
				{
					t.Item1.Speed = t.Item1.OSP;
					TanksToRemove.Add( t );
				}
			}
			foreach ( var t in TanksToRemove )
			{
				TanksToAffect.Remove( t );
			}

			if ( base.Update( gameTime, Tanks, Projectiles, Fences ) )
			{
				return true;
			}
			return false;
		}

		public override void Draw( SpriteBatch spriteBatch, TimeSpan gameTime )
		{
			spriteBatch.Draw( tex, Position, null, Color.White, 0, new Vector2( 16, 16 ), 1, SpriteEffects.None, 0.5F );
		}

		public override void Stop()
		{
			foreach ( var t in TanksToAffect )
			{
				t.Item1.Speed = t.Item1.OSP;
			}
		}
	}
}
