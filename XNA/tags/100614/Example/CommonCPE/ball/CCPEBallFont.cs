////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM
//	サンプルゲーム用 コンテンツ プロセッサ
//		Copyright (c) 2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace danmaq.common_cpe.ball
{

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>
	/// This class will be instantiated by the XNA Framework Content Pipeline
	/// to apply custom processing to content data, converting an object of
	/// type TInput to TOutput. The input and output types may be the same if
	/// the processor wishes to alter data without changing its type.
	///
	/// This should be part of a Content Pipeline Extension Library project.
	///
	/// TODO: change the ContentProcessor attribute to specify the correct
	/// display name for this processor.
	/// </summary>
	[ContentProcessor(DisplayName = "赤い玉 青い玉 競走ゲーム スプライト フォント プロセッサ")]
	public sealed class CCPEBallFont : FontTextureProcessor
	{

		//* ─────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* constants ──────────────────────────────-*

		/// <summary>文字一覧。</summary>
		private const string charList =
			" ()-/.0123456789:ACEFHIMOPRS" +
			"acdeghilmnqrstv０１２３４５６" +
			"７８９。！ーいくけさしだちてを" +
			"ウキゲスマム易競玉勝青赤左選走" +
			"打択度難負連";

		//* ────＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿＿_*
		//* methods ───────────────────────────────-*

		//* -----------------------------------------------------------------------*
		/// <summary>グリフ インデックスを文字にマップします。</summary>
		/// 
		/// <param name="index">グリフ インデックス。</param>
		/// <returns>指定されたグリフ インデックスで表された文字。</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="index"/>が、使用可能な文字の範囲外です。
		/// </exception>
		protected override char GetCharacterForIndex(int index)
		{
			if(index >= charList.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			return charList[index];
		}
	}
}
