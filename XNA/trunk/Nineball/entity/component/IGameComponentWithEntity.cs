////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework;

namespace danmaq.nineball.entity.component {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲームコンポーネントと状態を持つオブジェクトのインターフェイス。</summary>
	public interface IGameComponentWithEntity : IGameComponent, IUpdateable, IEntity, IDisposable {
	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲームコンポーネントと状態を持つオブジェクトのインターフェイス。</summary>
	public interface IDrawableGameComponentWithEntity : IGameComponentWithEntity, IDrawable {
	}
}
