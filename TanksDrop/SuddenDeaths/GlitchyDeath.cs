using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TanksDrop.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TanksDrop.SuddenDeaths
{
	/// <summary>
	/// A sudden death that has the tanks see their trails then generate many 'awesome faces', eventually filling the board with them.
	/// </summary>
	class GlitchyDeath : SuddenDeath
	{
		List<Vector2> awesomeFacePositions;
		List<Color> awesomeFaceColors;
		List<float> xAdvances;
		List<float> yAdvances;
		Random random;
		Texture2D awesomeFace;

		public GlitchyDeath()
		{
			Construct();
		}

		private void Construct()
		{
			random = new Random();
			awesomeFacePositions = new List<Vector2>();
			awesomeFaceColors = new List<Color>();
			xAdvances = new List<float>();
			yAdvances = new List<float>();
		}

		public override bool Draw( SpriteBatch spriteBatch, TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, GameTime gameTime )
		{
			for ( int i = 0; i < awesomeFacePositions.Count; i++ )
			{
				spriteBatch.Draw( awesomeFace, awesomeFacePositions[ i ], null, awesomeFaceColors[ i ], 0, Vector2.Zero, 1, SpriteEffects.None, 1 );
				awesomeFacePositions[ i ] += new Vector2( xAdvances[ i ], yAdvances[ i ] );
			}
			return false;
		}

		public override bool Update( TankObject[] Tanks, HashSet<ProjectileObject> Projectiles, HashSet<FenceObject> Fences, HashSet<Pickup> Pickups, GameTime gameTime )
		{
			double millisecs = ( gameTime.TotalGameTime - startTime ).TotalMilliseconds;
			if ( millisecs > 1000 * awesomeFacePositions.Count && millisecs < 10000 && millisecs > 1000 )
			{
				awesomeFacePositions.Add( new Vector2( random.Next( 1000 ), random.Next( 1000 ) ) );
				int PosNeg = random.Next( -10, 10 );
				xAdvances.Add( ( float )( random.NextDouble() * PosNeg ) );
				yAdvances.Add( ( float )( random.NextDouble() * PosNeg ) );
				awesomeFaceColors.Add( new Color( random.Next( 256 ), random.Next( 256 ), random.Next( 256 ) ) );
			}
			else if ( millisecs > 10000 )
			{
				Add( 10 );
			}
			if ( ( gameTime.TotalGameTime - startTime ).TotalMilliseconds > 12000 )
			{
				foreach ( TankObject Tank in Tanks )
				{
					Tank.Kill();
				}
			}
			return true;
		}

		private void Add( int repeat )
		{
			for ( int i = 0; i < repeat; i++ )
			{
				awesomeFacePositions.Add( new Vector2( random.Next( -64, 1000 ), random.Next( -64, 1000 ) ) );
				xAdvances.Add( 0 );
				yAdvances.Add( 0 );
				awesomeFaceColors.Add( Color.White );
			}
		}

		public override void Initialize( GameTime gameTime, ContentManager Content )
		{
			base.Initialize( gameTime, Content );
			awesomeFace = Content.Load<Texture2D>( "Sprites\\Face" );
			Construct();
		}
	}
}
