﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.261
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace danmaq.Nineball.Core {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Text {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Text() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("danmaq.Nineball.Core.Text", typeof(Text).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   厳密に型指定されたこのリソース クラスを使用して、すべての検索リソースに対し、
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   失敗 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string BOOL_FAILED {
            get {
                return ResourceManager.GetString("BOOL_FAILED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   成功 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string BOOL_SUCCEEDED {
            get {
                return ResourceManager.GetString("BOOL_SUCCEEDED", resourceCulture);
            }
        }
        
        /// <summary>
        ///   多重起動されました。
        ///このアプリケーションは多重起動に対応しておりません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string ERR_IO_MUTEX {
            get {
                return ResourceManager.GetString("ERR_IO_MUTEX", resourceCulture);
            }
        }
        
        /// <summary>
        ///   danmaq Nineball Library に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string NAME {
            get {
                return ResourceManager.GetString("NAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   null に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string NULL {
            get {
                return ResourceManager.GetString("NULL", resourceCulture);
            }
        }
    }
}
