////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM
//	ƒTƒ“ƒvƒ‹ƒQ[ƒ€—p ƒRƒ“ƒeƒ“ƒc ƒvƒƒZƒbƒT
//		Copyright (c) 2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace danmaq.common_cpe.ball
{

	//* „ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª„ª *
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
	[ContentProcessor(DisplayName = "Ô‚¢‹Ê Â‚¢‹Ê ‹£‘–ƒQ[ƒ€ ƒXƒvƒ‰ƒCƒg ƒtƒHƒ“ƒg ƒvƒƒZƒbƒT")]
	public sealed class CCPEBallFont : FontTextureProcessor
	{

		//* „Ÿ„Ÿ„Ÿ„Ÿ„ŸQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ_*
		//* constants „Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ-*

		/// <summary>•¶šˆê——B</summary>
		private const string charList =
			" ()-/.0123456789:ACEFHIMOPRS" +
			"acdeghilmnqrstv‚O‚P‚Q‚R‚S‚T‚U" +
			"‚V‚W‚XBI[‚¢‚­‚¯‚³‚µ‚¾‚¿‚Ä‚ğ" +
			"ƒEƒLƒQƒXƒ}ƒ€ˆÕ‹£‹ÊŸÂÔ¶‘I‘–" +
			"‘Å‘ğ“x“ï•‰˜A";

		//* „Ÿ„Ÿ„Ÿ„ŸQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQQ_*
		//* methods „Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ„Ÿ-*

		//* -----------------------------------------------------------------------*
		/// <summary>ƒOƒŠƒt ƒCƒ“ƒfƒbƒNƒX‚ğ•¶š‚Éƒ}ƒbƒv‚µ‚Ü‚·B</summary>
		/// 
		/// <param name="index">ƒOƒŠƒt ƒCƒ“ƒfƒbƒNƒXB</param>
		/// <returns>w’è‚³‚ê‚½ƒOƒŠƒt ƒCƒ“ƒfƒbƒNƒX‚Å•\‚³‚ê‚½•¶šB</returns>
		/// <exception cref="System.ArgumentOutOfRangeException">
		/// <paramref name="index"/>‚ªAg—p‰Â”\‚È•¶š‚Ì”ÍˆÍŠO‚Å‚·B
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
