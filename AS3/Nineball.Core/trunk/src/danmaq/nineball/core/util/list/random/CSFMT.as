package danmaq.nineball.core.util.list.random
{
	
	/**
	 * SFMT法(改良版Mersenne twister)を用いた疑似乱数ジェネレータ。
	 *
	 * <pre>
	 *	Copyright (c) 2006,2007 Mutsuo Saito, Makoto Matsumoto and Hiroshima
	 *	University. All rights reserved.
	 *
	 *	Redistribution and use in source and binary forms, with or without
	 *	modification, are permitted provided that the following conditions are
	 *	met:
	 *
	 *	    * Redistributions of source code must retain the above copyright
	 *	      notice, this list of conditions and the following disclaimer.
	 *	    * Redistributions in binary form must reproduce the above
	 *	      copyright notice, this list of conditions and the following
	 *	      disclaimer in the documentation and/or other materials provided
	 *	      with the distribution.
	 *	    * Neither the name of the Hiroshima University nor the names of
	 *	      its contributors may be used to endorse or promote products
	 *	      derived from this software without specific prior written
	 *	      permission.
	 *
	 *	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
	 *	"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
	 *	LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
	 *	A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
	 *	OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	 *	SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
	 *	LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
	 *	DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
	 *	THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
	 *	(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
	 *	OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
	 * </pre>
	 *
	 * @author Mc(danmaq)
	 * @see http://www.math.sci.hiroshima-u.ac.jp/~m-mat/MT/SFMT/
	 */
	public final class CSFMT extends CRandom
	{
		
		//* constants ──────────────────────────────-*

		/** 擬似乱数を計算するために使用する数値テーブル。 */
		private const table:Vector.<uint> = new Vector.<uint>(624);

		//* fields ────────────────────────────────*
		
		/** 数値テーブルのインデックス。 */
		private var _index:int;
		
		//* constructor & destructor ───────────────────────*
		
		/**
		 * コンストラクタ。
		 * 初期シード値に負数を指定した場合、システム依存値が設定されます。
		 *
		 * @param seed シード値。
		 */
		public function CSFMT(seed:int=int.MIN_VALUE)
		{
			super(seed);
		}
		
		//* instance properties ─────────────────────────-*
		
		/**
		 * 最大値を取得します。
		 *
		 * @return 最大値。
		 */
		override public function get max():uint
		{
			return uint.MAX_VALUE;
		}
		
		/**
		 * 0からmaxプロパティまでの範囲内の擬似乱数を取得します。
		 *
		 * @return 乱数値。
		 */
		override public function get next():uint
		{
			addCounter();
			if(_index == table.length)
			{
				generate();
				_index = 0;
			}
			return table[_index++];
		}
		
		//* instance methods ───────────────────────────*
		
		/**
		 * @inheritDoc
		 */
		override public function reset(seed:int=int.MIN_VALUE):void
		{
			super.reset(seed);
			seed = this.seed;
			table[0] = seed;
			var length:int = table.length;
			for(var i:int = 0; i < length; i++)
			{
				seed = 1812433253 * (seed ^ (seed >>> 30)) + i;
				table[i] = seed;
			}
			periodCertification();
		}

		/**
		 * 乱数テーブルを生成します。
		 */
		private function generate():void
		{
			var a:int = 0;
			var b:int = 488;
			var c:int = 616;
			var d:int = 620;
			var y:int;
			var p:Vector.<uint> = table;
			var length:int = p.length;
			
			do
			{
				y = p[a + 3] ^ (p[a + 3] << 8) ^ (p[a + 2] >>> 24) ^ ((p[b + 3] >>> 11) & 0xbffffff6);
				p[a + 3] = y ^ (p[c + 3] >>> 8) ^ (p[d + 3] << 18);
				y = p[a + 2] ^ (p[a + 2] << 8) ^ (p[a + 1] >>> 24) ^ ((p[b + 2] >>> 11) & 0xbffaffff);
				p[a + 2] = y ^ ((p[c + 2] >>> 8) | (p[c + 3] << 24)) ^ (p[d + 2] << 18);
				y = p[a + 1] ^ (p[a + 1] << 8) ^ (p[a] >>> 24) ^ ((p[b + 1] >>> 11) & 0xddfecb7f);
				p[a + 1] = y ^ ((p[c + 1] >>> 8) | (p[c + 2] << 24)) ^ (p[d + 1] << 18);
				y = p[a] ^ (p[a] << 8) ^ ((p[b] >>> 11) & 0xdfffffef);
				p[a] = y ^ ((p[c] >>> 8) | (p[c + 1] << 24)) ^ (p[d] << 18);
				c = d;
				d = a;
				a += 4;
				b += 4;
				if (b == length)
				{
					b = 0;
				}
			}
			while (a != length);
		}
		
		private function periodCertification():void
		{
			var work:int;
			var inner:int = 0;
			var i:int;
			var j:int;
			var parity:Vector.<uint> = new Vector.<uint>(4);
			parity.push(0x00000001);
			parity.push(0x00000000);
			parity.push(0x00000000);
			parity.push(0x13c9e684);
			_index = table.length;
			
			for (i = 0; i < 4; i++)
			{
				inner ^= table[i] & parity[i];
			}
			for (i = 16; i > 0; i >>>= 1)
			{
				inner ^= inner >>> i;
			}
			inner &= 1;
			if (inner != 1)
			{
				for (i = 0; i < 4; i++)
				{
					for (j = 0, work = 1; j < 32; j++, work <<= 1 )
					{
						if ((work & parity[i]) != 0)
						{
							table[i] ^= work;
							return;
						}
					}
				}
			}
		}
	}
}
