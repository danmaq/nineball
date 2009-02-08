=====================================================================
    danmaq Nineball-Library
        (c)2008-2009 danmaq All rights reserved.
=====================================================================

_____________________________________________________________________
■目次

・概要
・システム要件
・使用方法
・FAQ
・注意
・使用実績
・今後の製作予定
・著作権情報
・更新履歴

_____________________________________________________________________
■概要

主にゲーム向けのフレームワークです。本来XNA(現在非公開)用に開発した
ものをFLEX用として移植したものです。ちょっと頑張ればFlash CS3でも
使えるかもしれません。別に内部でなんか凄い処理を行っている
わけではなく、それなりの規模のゲームを作るためには最低限必要
だろうと思われるアルゴリズムをまとめた程度の能力の物体です。

ぶっちゃけ海の向こうのお偉いさんが作ったでっかい
フレームワークがよくわかんないくらいのあたまが⑨な
プログラマ一なんでこんなのつくってみたってだけだったり～。

_____________________________________________________________________
■システム要件

・Flex SDK 3.2.0以降(3.3.0以降を推奨)
・Flash Player 10.0.12以降(開発時はデバッグ版を推奨)
・モノづくりのネタ

_____________________________________________________________________
■使用方法

1.あなたのFlexプロジェクトにNineball.swcをリンクしてください。
  (プロパティ→ビルドパス→ライブラリパス)
2.コンパイルオプションに-target-player=10.0.12を追加してください。
3.クラスdanmaq.nineball.struct.CInitializeDataをnewし、
　必要な初期化データを食わせてください。
4.あとは煮るなり焼くなりお好きにどうぞ。

_____________________________________________________________________
■FAQ

○ライブラリ名の由来を教えてください。
おバカなプログラマが開発したライブラリなので、このような名前となって
しまいまいた！『⑨』と言えば分かる人には分かるでしょう。分からない人も
このキーワードでぐぐれば分かるかと思います。あと本当に賢い人なら、
この程度のライブラリくらいは自前で作ってるだろ的な意味も、少しだけ
込められています。ちなみに当初は2006年夏に作った弾幕風フレームワーク
『FLAN』の次世代版と言うことで予定名称が『FLAN-II』だったのですが、
FLANとは全く異なる操作性な上、今回弾幕風はサポート対象から
外れるので敢えて全く別物な名称を付けるに至りました。

○バグみっけた！orこんな機能がほしい！
danmaqサイト掲示板のサポートスレかメールで受け付けております。
http://danmaq.com/feedback/test/read.cgi/board/1228028892/l50

_____________________________________________________________________
■注意

danmaq Nineball-Libraryを「ほんのちょっとした」Flashに使用することは
余りお勧め出来ません。このフレームワークはライブラリ単体でも
443KBあります(リンクされたSWFは最終的に500KBを超えるでしょう)ので、
例えば低速回線向けのFlashや早さが命の広告Flash、または画面の片隅で
アクセント的な役割をするFlashムービーには、残念ながら不得手です。
(フレッツISDN回線だとどう頑張っても表示までに25秒かかります)

_____________________________________________________________________
■使用実績

下記のFlashムービーはこのライブラリを使って出来ました。

・タイトー「スペースパズルボブル」 Web体験版Flash
・タイトー「スペースインベーダー エクストリーム 2」 公式サイトFlash

_____________________________________________________________________
■今後の製作予定

今後の予定としては、Flex(ActionScript3)版以外にも
C#.NET3.5版、ObjC2版も製作・頒布予定です。
.NET3.5+XNA3.0版……
　2008晩夏製作開始も大人の事情で2009年夏まで公開延期
Objective-C2.0版……
　2009年早春製作開始予定

_____________________________________________________________________
■著作権情報

Copyright (c) 2008-2009 danmaq All rights reserved.

以下に定める条件に従い、本ソフトウェアおよび関連文書のファイ
ル（以下「ソフトウェア」）の複製を取得するすべての人に対し、
ソフトウェアを無制限に扱うことを無償で許可します。これには、
ソフトウェアの複製を使用、複写、変更、結合、掲載、頒布、サブ
ライセンス、および／または販売する権利、およびソフトウェアを
提供する相手に同じことを許可する権利も無制限に含まれます。 

上記の著作権表示および本許諾表示を、ソフトウェアの
すべての複製または重要な部分に記載するものとします。 

ソフトウェアは「現状のまま」で、明示であるか暗黙であるかを問わず、
何らの保証もなく提供されます。ここでいう保証とは、商品性、特定の目
的への適合性、および権利非侵害についての保証も含みますが、それに限
定されるものではありません。作者または著作権者は、契約行為、不法行
為、またはそれ以外であろうと、ソフトウェアに起因または関連し、ある
いはソフトウェアの使用またはその他の扱いによって生じる一切の請求、
損害、その他の義務について何らの責任も負わないものとします。 

Permission is hereby granted, free of charge, to any person
obtaining a copy of this software and associated documentation
files (the "Software"), to deal in the Software without restriction,
including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Software,
and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

danmaq代表 Mc(まく) <info@danmaq.com> http://danmaq.com/

_____________________________________________________________________
■更新履歴

HISTORY.txtを参照してください。

