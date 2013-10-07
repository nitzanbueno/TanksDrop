using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TanksDrop
{
	/// <summary>
	/// A game object that can be placed by tanks, doesn't move and has the shape of a line.
	/// </summary>
	abstract class FenceObject
	{
		public Vector2 Point1;
		public Vector2 Point2;
		protected float Width;
		public TankObject Owner;
		protected Color color;
		protected TimeSpan gt;
		public float angle;
		protected Texture2D blank;

		public FenceObject( Vector2 p1, Vector2 p2, float width, TankObject owner, TimeSpan gameTime, GraphicsDevice gd )
		{
			Point1 = p1;
			Point2 = p2;
			Width = width;
			Owner = owner;
			color = Owner.RGBColor;
			gt = gameTime;
			angle = ( MathHelper.ToDegrees( Owner.Rotation ) + 90 ) % 360;
			blank = new Texture2D( gd, 2, 2, false, SurfaceFormat.Color );
			blank.SetData( Enumerable.Repeat<Color>( Color.White, 4 ).ToArray<Color>() );
		}

		/// <summary>
		/// Event to occur whenever a tank collides.
		/// </summary>
		/// <param name="tank">The tank that collides.</param>
		/// <param name="factor">The distance the tank moves - used in order to have the tank pass through the fence by returning it.</param>
		/// <returns>The distance the tank should move - for instance, 0 for an impenetrable wall.</returns>
		public virtual float OnTankCollision( TankObject tank, float factor )
		{
			if ( tank.powerUp != null )
			{
				if ( tank.powerUp.DoesGoThruFence( this ) ) return factor;
				else return 0;
			}
			return 0;
		}

		/// <summary>
		/// Update the fence. Usually unnessecary to change, it just removes itself when its time is up.
		/// </summary>
		/// <param name="time">The current game time.</param>
		/// <returns>If true, remove this fence, otherwise false.</returns>
		public virtual bool Update( TimeSpan time )
		{
			if ( ( ( time - gt ).TotalMilliseconds > Owner.FenceTime && Owner.FenceTime >= 0 ) )
			{
				Owner.PlacedFences--;
				return true;
			}
			return false;
		}

		public abstract void Draw( SpriteBatch spriteBatch );
	}
}
