////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2013 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

#if XBOX360

using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using danmaq.nineball.entity;
using danmaq.nineball.util;

namespace danmaq.nineball.state.manager {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>XBOX360ガイド 補助クラス。</summary>
	public sealed class CStateGuideHelper : CState {

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>ガイド表示情報の共通基底クラス。</summary>
		private abstract class CGuideInfoBase {

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>XBOX360ガイド操作が終了すると呼び出されるメソッド。</summary>
			protected readonly AsyncCallback callback;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="callback">XBOX360ガイド表示用デリゲート</param>
			protected CGuideInfoBase( AsyncCallback callback ) { this.callback = callback; }

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
			private readonly string[] BUTTONS = new string[] { "OK" };

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
						Guide.BeginShowMessageBox( PlayerIndex.One, "エラーが発生しました。",
							MESSAGE, BUTTONS, 0, MessageBoxIcon.Error, callback, null );
						bResult = true;
					}
					catch( Exception e ) { CLogger.add( e ); }
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
						Guide.BeginShowStorageDeviceSelector( callback, null );
						bResult = true;
					}
					catch( Exception e ) { CLogger.add( e ); }
				}
				return bResult;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>クラス オブジェクト。</summary>
		public static readonly CStateGuideHelper instance = new CStateGuideHelper();

		/// <summary>XBOX360ガイド表示のキュー。</summary>
		private readonly Queue<CGuideInfoBase> queueCallback = new Queue<CGuideInfoBase>();

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		private CStateGuideHelper() { }

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

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
			else { queueCallback.Enqueue( new CGuideInfoSelectDevice( callback ) ); }
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
			else { queueCallback.Enqueue( new CGuideInfoMessageBox( callback, strText ) ); }
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>1フレーム分の更新処理を実行します。</para>
		/// <para>ここでは、予約されているガイドの表示を試みます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update( IEntity entity, object privateMembers, GameTime gameTime ) {
			if( queueCallback.Count > 0 && !Guide.IsVisible ) {
				if( queueCallback.Peek().start() ) {
					queueCallback.Dequeue();
				}
			}
			base.update( entity, privateMembers, gameTime );
		}
	}

}

#endif
