﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SuperBasic.Utilities.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class DiagnosticsResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DiagnosticsResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SuperBasic.Utilities.Resources.DiagnosticsResources", typeof(DiagnosticsResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to I was expecting to see &apos;{0}&apos; after this..
        /// </summary>
        public static string UnexpectedEndOfStream {
            get {
                return ResourceManager.GetString("UnexpectedEndOfStream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This statement should go on a new line..
        /// </summary>
        public static string UnexpectedStatementInsteadOfNewLine {
            get {
                return ResourceManager.GetString("UnexpectedStatementInsteadOfNewLine", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I didn&apos;t expect to see &apos;{0}&apos; here. I was expecting &apos;{1}&apos; instead..
        /// </summary>
        public static string UnexpectedTokenFound {
            get {
                return ResourceManager.GetString("UnexpectedTokenFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I didn&apos;t expect to see &apos;{0}&apos; here. I was expecting the start of a new statement..
        /// </summary>
        public static string UnexpectedTokenInsteadOfStatement {
            get {
                return ResourceManager.GetString("UnexpectedTokenInsteadOfStatement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I don&apos;t understand this character &apos;{0}&apos;..
        /// </summary>
        public static string UnrecognizedCharacter {
            get {
                return ResourceManager.GetString("UnrecognizedCharacter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This string is missing its right double quotes..
        /// </summary>
        public static string UnterminatedStringLiteral {
            get {
                return ResourceManager.GetString("UnterminatedStringLiteral", resourceCulture);
            }
        }
    }
}
