﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:2.0.50727.3603
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace danmaq.nineball.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("danmaq.nineball.Properties.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   予期しない不具合が発生した為、ゲームを強制終了します。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ERR_EXCEPTION {
            get {
                return ResourceManager.GetString("ERR_EXCEPTION", resourceCulture);
            }
        }
        
        /// <summary>
        ///   エラーが発生しました。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ERR_MESSAGE {
            get {
                return ResourceManager.GetString("ERR_MESSAGE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   {0}オブジェクトを、2つ以上作ることはできません。 に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string ERR_SINGLETON {
            get {
                return ResourceManager.GetString("ERR_SINGLETON", resourceCulture);
            }
        }
        
        /// <summary>
        ///   danmaq Nineball Library に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string NAME {
            get {
                return ResourceManager.GetString("NAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   null に類似しているローカライズされた文字列を検索します。
        /// </summary>
        public static string NULL {
            get {
                return ResourceManager.GetString("NULL", resourceCulture);
            }
        }
    }
}
