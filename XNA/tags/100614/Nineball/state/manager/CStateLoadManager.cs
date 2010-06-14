////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library
//		Copyright (c) 2008-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using danmaq.nineball.data;
using danmaq.nineball.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace danmaq.nineball.state.manager
{

	// TODO : object以外も返せるようにしなきゃ

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>ローディング画面の基底クラスです。</summary>
	public abstract class CStateLoadManager : CState
	{

		//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
		/// <summary>読み込み対象情報を格納する構造体です。</summary>
		private struct SData
		{

			//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
			//* constants ──────────────────────────────-*

			/// <summary>アセット名。</summary>
			public readonly string asset;

			/// <summary>アセットに対応する型情報。</summary>
			public readonly Type type;

			/// <summary>読み込んだ情報の格納先。</summary>
			public readonly CValue<object> dst;

			//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
			//* constructor & destructor ───────────────────────*

			//* -----------------------------------------------------------------------*
			/// <summary>コンストラクタ。</summary>
			/// 
			/// <param name="strAsset">アセット名。</param>
			/// <param name="type">アセットに対応する型情報。</param>
			/// <param name="dst">読み込んだ情報の格納先。</param>
			/// <exception cref="System.ArgumentNullException">
			/// アセット名及び、その型情報に<c>null</c>を設定した場合。
			/// </exception>
			public SData(string strAsset, Type type, CValue<object> dst)
			{
				if(strAsset == null)
				{
					throw new ArgumentNullException("strAsset");
				}
				if(type == null)
				{
					throw new ArgumentNullException("type");
				}
				if(dst == null)
				{
					dst = new object();
				}
				this.asset = strAsset;
				this.type = type;
				this.dst = dst;
			}
		}

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>読み込みのためのコンテンツ マネージャ。</summary>
		protected readonly ContentManager contentManager;

		/// <summary>モデル アセット用の既定のローダ。</summary>
		protected readonly Func<string, object> defaultLoaderModel;

		/// <summary>エフェクト アセット用の既定のローダ。</summary>
		protected readonly Func<string, object> defaultLoaderEffect;

		/// <summary>スプライト フォント アセット用の既定のローダ。</summary>
		protected readonly Func<string, object> defaultLoaderSpriteFont;

		/// <summary>テクスチャ アセット用の既定のローダ。</summary>
		protected readonly Func<string, object> defaultLoaderTexture;

		/// <summary>2Dテクスチャ アセット用の既定のローダ。</summary>
		protected readonly Func<string, object> defaultLoaderTexture2D;

		/// <summary>3Dテクスチャ アセット用の既定のローダ。</summary>
		protected readonly Func<string, object> defaultLoaderTexture3D;

		/// <summary>2Dテクスチャの6面セット アセット用の既定のローダ。</summary>
		protected readonly Func<string, object> defaultLoaderTextureCube;

		/// <summary>
		/// <para>辞書化されたローダの一覧。</para>
		/// <para>
		/// アセットを読み込む際、この一覧から最適なローダを検索して、読み込みを行います。
		/// </para>
		/// <para>
		/// また、この一覧にローダを追加することで、カスタム形式の読み込みも可能となります。
		/// </para>
		/// </summary>
		/// <remarks>
		/// このクラスの読み込み専用フィールドに置かれているローダは、
		/// 全てデフォルトでこのローダ一覧にプリセットされています。
		/// </remarks>
		protected readonly Dictionary<Type, Func<string, object>> loaderList =
			new Dictionary<Type, Func<string, object>>();

		/// <summary>読み込み待ちのアセット情報一覧。</summary>
		private readonly Queue<SData> loadQueueList = new Queue<SData>();

		//* ───-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* fields ────────────────────────────────*

		/// <summary>読み込みターンの待機フレーム数。</summary>
		private ushort m_wInterval = 1;

		/// <summary>1読み込みターン毎に読み込むファイル数。</summary>
		private ushort m_wLoadPerFrame = 1;

		//* ────────────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* constructor & destructor ───────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>コンストラクタ。</summary>
		/// 
		/// <param name="contentManager">
		/// 読み込みのためのコンテンツ マネージャ。
		/// </param>
		/// <exception cref="System.ArgumentNullException">
		/// 読み込みのためのコンテンツ マネージャに、<c>null</c>を設定しようとした場合。
		/// </exception>
		protected CStateLoadManager(ContentManager contentManager)
		{
			if(contentManager == null)
			{
				throw new ArgumentNullException("contentManager");
			}
			this.contentManager = contentManager;
			defaultLoaderModel = asset => contentManager.Load<Model>(asset);
			defaultLoaderEffect = asset => contentManager.Load<Effect>(asset);
			defaultLoaderSpriteFont = asset => contentManager.Load<SpriteFont>(asset);
			defaultLoaderTexture = asset => contentManager.Load<Texture>(asset);
			defaultLoaderTexture2D = asset => contentManager.Load<Texture2D>(asset);
			defaultLoaderTexture3D = asset => contentManager.Load<Texture3D>(asset);
			defaultLoaderTextureCube = asset => contentManager.Load<TextureCube>(asset);
			loaderList.Add(typeof(Model), defaultLoaderModel);
			loaderList.Add(typeof(Effect), defaultLoaderEffect);
			loaderList.Add(typeof(SpriteFont), defaultLoaderSpriteFont);
			loaderList.Add(typeof(Texture), defaultLoaderTexture);
			loaderList.Add(typeof(Texture2D), defaultLoaderTexture2D);
			loaderList.Add(typeof(Texture3D), defaultLoaderTexture3D);
			loaderList.Add(typeof(TextureCube), defaultLoaderTextureCube);
		}

		//* ─────-＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿*
		//* properties ──────────────────────────────*

		//* -----------------------------------------------------------------------*
		/// <summary>合計予約数を取得します。</summary>
		/// 
		/// <value>合計予約数。</value>
		public int totalReserved
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>合計読み込み済み数を取得します。</summary>
		/// 
		/// <value>合計読み込み済み数。</value>
		public int totalLoaded
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>簡易フレーム カウンタを取得します。</summary>
		/// 
		/// <value>簡易フレーム カウンタ。</value>
		public int counter
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>読み込みが完了したかどうかを取得します。</summary>
		/// 
		/// <value>読み込みが完了した場合、<c>true</c>。</value>
		public bool loaded
		{
			get;
			private set;
		}

		//* -----------------------------------------------------------------------*
		/// <summary>進捗率を取得します。</summary>
		/// 
		/// <value>進捗率(0.0f～1.0fの範囲)。</value>
		public float progress
		{
			get
			{
				return (float)totalLoaded / (float)totalReserved;
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>読み込みターンの待機フレーム数を設定/取得します。</summary>
		/// 
		/// <value>読み込みターンの待機フレーム数。</value>
		protected ushort interval
		{
			get
			{
				return m_wInterval;
			}
			set
			{
				m_wInterval = Math.Max((ushort)1, value);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>1読み込みターン毎に読み込むファイル数を設定/取得します。</summary>
		/// 
		/// <value>1読み込みターン毎に読み込むファイル数。</value>
		protected ushort loadPerFrame
		{
			get
			{
				return m_wLoadPerFrame;
			}
			set
			{
				m_wLoadPerFrame = Math.Max((ushort)1, value);
			}
		}

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>1フレーム分の更新処理を実行します。</para>
		/// <para>ここではループ中にアセットを遅延読み込みします。</para>
		/// <para>オーバーライドする場合、必ず呼び出してください。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="gameTime">前フレームが開始してからの経過時間。</param>
		public override void update(IEntity entity, object privateMembers, GameTime gameTime)
		{
			if(counter % interval == interval - 1)
			{
				for(int i = loadPerFrame; --i >= 0 && loadQueueList.Count > 0; )
				{
					SData data = loadQueueList.Dequeue();
					Func<string, object> loader;
					if(loaderList.TryGetValue(data.type, out loader))
					{
						data.dst.value = loader(data.asset);
						totalLoaded++;
					}
					else
					{
						throw new ContentLoadException(data.type.FullName +
							"型のアセットを読み込むためのローダが定義されていません。");
					}
				}
				if(loadQueueList.Count == 0 && !loaded)
				{
					loaded = true;
					onLoaded(entity, privateMembers);
				}
			}
			counter++;
			base.update(entity, privateMembers, gameTime);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>オブジェクトが別の状態へ移行する時に呼び出されます。</para>
		/// <para>このメソッドは、遷移先の<c>setup</c>よりも先に呼び出されます。</para>
		/// </summary>
		/// 
		/// <param name="entity">この状態を終了したオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		/// <param name="nextState">オブジェクトが次に適用する状態。</param>
		public override void teardown(IEntity entity, object privateMembers, IState nextState)
		{
			loadQueueList.Clear();
			totalReserved = 0;
			totalLoaded = 0;
			counter = 0;
			loaded = false;
			base.teardown(entity, privateMembers, nextState);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>継承先で読み込み終えた際の処理を記述してください。</summary>
		/// 
		/// <param name="entity">この状態を適用されているオブジェクト。</param>
		/// <param name="privateMembers">
		/// オブジェクトと状態クラスのみがアクセス可能なフィールド。
		/// </param>
		protected abstract void onLoaded(IEntity entity, object privateMembers);

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>アセット読み込みの予約を追加します。</para>
		/// <para>
		/// 読み込まれたアセットは<c>ContentManager</c>のキャッシュとして保持されます。
		/// </para>
		/// </summary>
		/// 
		/// <typeparam name="_T">アセットに対応する型情報。</typeparam>
		/// <param name="strAsset">アセット名。</param>
		protected void addReserve<_T>(string strAsset) where _T : class
		{
			addReserve<_T>(strAsset, null);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>一斉にアセット読み込みの予約を追加します。</para>
		/// <para>
		/// 読み込まれたアセットは<c>ContentManager</c>のキャッシュとして保持されます。
		/// </para>
		/// </summary>
		/// 
		/// <typeparam name="_T">アセットに対応する型情報。</typeparam>
		/// <param name="assetList">アセット名の一覧。</param>
		protected void addReserve<_T>(IEnumerable<string> assetList) where _T : class
		{
			foreach(string strAsset in assetList)
			{
				addReserve<_T>(strAsset);
			}
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アセット読み込みの予約を追加します。</summary>
		/// 
		/// <typeparam name="_T">アセットに対応する型情報。</typeparam>
		/// <param name="strAsset">アセット名。</param>
		/// <param name="dst">読み込んだアセットを格納する場所。</param>
		protected void addReserve<_T>(string strAsset, CValue<object> dst) where _T : class
		{
			addReserve(strAsset, typeof(_T), dst);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>
		/// <para>アセット読み込みの予約を追加します。</para>
		/// <para>
		/// 読み込まれたアセットは<c>ContentManager</c>のキャッシュとして保持されます。
		/// </para>
		/// </summary>
		/// 
		/// <param name="strAsset">アセット名。</param>
		/// <param name="type">アセットに対応する型情報。</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// 読み込んだアセットを格納先として、値型を設定しようとした場合。
		/// </exception>
		protected void addReserve(string strAsset, Type type)
		{
			addReserve(strAsset, type, null);
		}

		//* -----------------------------------------------------------------------*
		/// <summary>アセット読み込みの予約を追加します。</summary>
		/// 
		/// <param name="strAsset">アセット名。</param>
		/// <param name="type">アセットに対応する型情報。</param>
		/// <param name="dst">読み込んだアセットを格納先。</param>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// アセットに対応する型情報に、値型を設定しようとした場合。
		/// </exception>
		protected void addReserve(string strAsset, Type type, CValue<object> dst)
		{
			// TODO : typeとdstが同一型または継承型かどうかを検証するコードを入れる
			if(!type.IsClass)
			{
				throw new ArgumentOutOfRangeException(
					"type", "アセットに対応する型は参照型である必要があります。");
			}
			loadQueueList.Enqueue(new SData(strAsset, type, dst));
			totalReserved++;
			loaded = false;
		}
	}
}
