////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;
using danmaq.nineball.Properties;
using danmaq.nineball.entity;

namespace danmaq.nineball.core {

	public sealed class CMain : DrawableGameComponent {

		public readonly CEntity entity = new CEntity();

		public CMain( Game game ) : base( game ) {
			if( instance != null ) {
				throw new InvalidOperationException( Resources.ERR_SINGLETON );
			}
			instance = this;
			game.Components.Add( this );
		}

		public static CMain instance { get; private set; }

		public override void Update( GameTime gameTime ) {
			entity.update( gameTime );
			base.Update( gameTime );
		}

		public override void Draw( GameTime gameTime ) {
			entity.draw( gameTime );
			base.Draw( gameTime );
		}
	}
}
