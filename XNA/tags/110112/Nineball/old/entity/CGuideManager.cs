////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2011 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if XBOX360

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using danmaq.nineball.entity;
using danmaq.nineball.entity.component;
using danmaq.nineball.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace danmaq.nineball.old.entity {

	// TODO : どのように使うかの仕様が作りかけ

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ガイド 補助クラス。</summary>
	public sealed class CGuideManager : CGameComponent {

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>ガイド表示情報の共通基底クラス。</summary>
		private abstract class CGuideInfoBase {

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>XBOX360ガイド操作が終了すると呼び出されるメソッド。</summary>
			protected readonly AsyncCallback CALLBACK;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="callback">XBOX360ガイド表示用デリゲート</param>
			protected CGuideInfoBase( AsyncCallback callback ) { CALLBACK = callback; }

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>XBOX360ガイドを開始します。</summary>
			public abstract bool start();
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>メッセージボックス ガイド表示情報クラス。</summary>
		private sealed class CGuideInfoMessageBox : CGuideInfoBase {

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>ボタン一覧。</summary>
			private readonly ReadOnlyCollection<string> BUTTONS =
				new List<string> { "OK" }.AsReadOnly();

			/// <summary>メッセージ テキスト文字列。</summary>
			private readonly string MESSAGE;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="callback">XBOX360ガイド表示用デリゲート</param>
			/// <param name="strMessage">メッセージ テキスト文字列</param>
			public CGuideInfoMessageBox( AsyncCallback callback, string strMessage )
				: base( callback ) {
				MESSAGE = strMessage;
			}

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>XBOX360ガイドを開始します。</summary>
			public override bool start() {
				bool bResult = false;
				if( !Guide.IsVisible ) {
					try {
						Guide.BeginShowMessageBox( PlayerIndex.One, Resources.ERR_MESSAGE,
							MESSAGE, BUTTONS, 0, MessageBoxIcon.Error, CALLBACK, null );
						bResult = true;
					}
					catch( Exception ) { }
				}
				return bResult;
			}
		}

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>メッセージボックス ガイド表示情報クラス。</summary>
		private sealed class CGuideInfoSelectDevice : CGuideInfoBase {

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="callback">XBOX360ガイド表示用デリゲート</param>
			public CGuideInfoSelectDevice( AsyncCallback callback ) : base( callback ) { }

			//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* methods ───────────────────────────────-*

			//* -----------------------------------------------------------------------*
			/// <summary>XBOX360ガイドを開始します。</summary>
			public override bool start() {
				bool bResult = false;
				if( !Guide.IsVisible ) {
					try {
						Guide.BeginShowStorageDeviceSelector( CALLBACK, null );
						bResult = true;
					}
					catch( Exception ) { }
				}
				return bResult;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>XBOX360ガイド表示のキュー。</summary>
		private readonly Queue<CGuideInfoBase> QUEUE_CALLBACK = new Queue<CGuideInfoBase>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="game">ゲーム コンポーネントをアタッチするゲーム。</param>
		/// <param name="bDirectRegist">ゲーム コンポーネントを直接登録するかどうか。</param>
		public CGuideManager( Game game, bool bDirectRegist ) :
			base( game, new CEntity(), bDirectRegist ) { }

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>予約されているガイドの表示を試みます。</summary>
		public override void Update( GameTime gameTime ) {
			if( QUEUE_CALLBACK.Count > 0 && !Guide.IsVisible ) {
				if( QUEUE_CALLBACK.Peek().start() ) {
					QUEUE_CALLBACK.Dequeue();
				}
			}
			base.Update( gameTime );
		}

		//* -----------------------------------------------------------------------*
		/// <summary>補助記憶装置選択ガイド表示を予約します。</summary>
		/// 
		/// <param name="callback">
		/// XBOX360ガイド操作が終了すると呼び出されるメソッド
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullが渡された場合。
		/// </exception>
		public void reserveSelectDevice( AsyncCallback callback ) {
			if( callback == null ) { throw new ArgumentNullException( "callback" ); }
			else { QUEUE_CALLBACK.Enqueue( new CGuideInfoSelectDevice( callback ) ); }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>メッセージボックス ガイド表示を予約します。</summary>
		/// 
		/// <param name="callback">
		/// XBOX360ガイド操作が終了すると呼び出されるメソッド
		/// </param>
		/// <param name="strText">メッセージ テキスト文字列</param>
		/// <exception cref="System.ArgumentNullException">
		/// 引数にnullが渡された場合。
		/// </exception>
		public void reserveMessage( AsyncCallback callback, string strText ) {
			if( callback == null ) { throw new ArgumentNullException( "callback" ); }
			else if( strText == null ) { throw new ArgumentNullException( "strText" ); }
			else { QUEUE_CALLBACK.Enqueue( new CGuideInfoMessageBox( callback, strText ) ); }
		}
	}
}

#endif
