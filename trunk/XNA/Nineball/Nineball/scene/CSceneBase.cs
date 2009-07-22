////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library / Copyright (c) 2008 danmaq all rights reserved.
//		──シーン本体の基底クラス
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using Microsoft.Xna.Framework;
using danmaq.Nineball.core.manager;
using danmaq.Nineball.core.raw;

namespace danmaq.Nineball.scene {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// <para>シーン本体の基底クラスです。</para>
	/// <para>
	/// シーン管理クラスCSceneManagerに登録するタスクを作成するためには、
	/// このクラスを継承するか、ISceneを実装します。
	/// </para>
	/// </summary>
	public abstract class CSceneBase : IScene {

		public IScene nextScene {
			get { return null; }
			set { }
		}

		public void Dispose() { }

		public bool update( GameTime gameTime ) { return false; }

		public void draw( GameTime gameTime, CSprite sprite ) { }

	}
}
