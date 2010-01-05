////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////
//
//	danmaq Nineball-Library SAMPLE PROGRAM #1
//	赤い玉 青い玉 競走ゲーム
//		Copyright (c) 1994-2010 danmaq all rights reserved.
//
////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////////////////////////////////////////////

using danmaq.ball.Properties;
using danmaq.ball.state.scene;
using danmaq.nineball;
using danmaq.nineball.util;
using danmaq.nineball.state.misc;

namespace danmaq.ball.core {

	//* ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━ *
	/// <summary>起動 クラス。</summary>
	static class CBallStarter {

		//* -----------------------------------------------------------------------*
		/// <summary>ここからプログラムが開始されます。</summary>
		/// 
		/// <param name="args">プログラムへ渡される引数</param>
		static void Main( string[] args ) {
			CStateCapsXNA xnastate = CStateCapsXNA.instance;
			xnastate.nextState = CStateInitialize.instance;
			CStarter.scene.nextState = xnastate;
			CLogger.outFile = Resources.FILE_BOOTLOG;
			CLogger.add( xnastate.report );
			CStarter.startNineball();
		}
	}
}
