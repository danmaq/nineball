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

namespace danmaq.nineball.entity.component {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲームコンポーネントと状態を持つオブジェクトのインターフェイス。</summary>
	public interface IGameComponentWithEntity : IGameComponent, IUpdateable, IEntity, IDisposable {
	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲームコンポーネントと状態を持つオブジェクトのインターフェイス。</summary>
	public interface IGameComponentWithEntity<_T> : IGameComponentWithEntity where _T : IEntity {
	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲームコンポーネントと状態を持つオブジェクトのインターフェイス。</summary>
	public interface IDrawableGameComponentWithEntity : IGameComponentWithEntity, IDrawable {
	}

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ゲームコンポーネントと状態を持つオブジェクトのインターフェイス。</summary>
	public interface IDrawableGameComponentWithEntity<_T> :
		IGameComponentWithEntity<_T>, IDrawableGameComponentWithEntity where _T : IEntity
	{ }
}
