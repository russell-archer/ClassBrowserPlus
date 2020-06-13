using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ClassBrowserPlus.DataModel
{
    public class ClassInfo
    {
        public const string Aqn = ", Version=255.255.255.255, Culture=neutral, PublicKeyToken=null, ContentType=WindowsRuntime";

        // --- Private fields -------------------------------------------------

        private readonly TypeInfo _typeInfo;
        private readonly string _className;
        private readonly string _assemblyQualifiedName;
        private readonly string _classSummary;
        private readonly List<string> _classHierarchy;

        // --- Ctor -----------------------------------------------------------

        /// <summary>
        /// This parameterless constructor is meant only to be used for the creation of design-time data in the XAML editor
        /// </summary>
        public ClassInfo() : this("Windows.Data.Json.JsonArray", "Windows.Data.Json.JsonArray" + Aqn, "This is design-time mock data")
        {
        }

        public ClassInfo(string className, string assemblyQualifiedName, string classSummary)
        {
            _className = className;
            _assemblyQualifiedName = assemblyQualifiedName;
            _classSummary = classSummary;
            _classHierarchy = new List<string>();

            var t = Type.GetType(_assemblyQualifiedName);
            if (t == null)
                return;

            _typeInfo = t.GetTypeInfo();
            BuildClassHierarchy(t);
        }

        // --- Properties -----------------------------------------------------

        public bool IsClass                     { get { return _typeInfo.IsClass; }}
        public bool IsInterface                 { get { return _typeInfo.IsInterface; }}
        public bool HasBaseClass                { get { return _typeInfo.BaseType != null; }}
        public bool HasProperties               { get { return Properties.Count > 0; } }
        public bool HasMethods                  { get { return Methods.Count > 0; } }
        public bool HasEvents                   { get { return Events.Count > 0; } }
        public string ClassName                 { get { return _className; } }
        public string AssemblyQualifiedName     { get { return _assemblyQualifiedName; } }
        public string ClassSummary              { get { return _classSummary; }}
        public string BaseClass                 { get { return HasBaseClass ? _typeInfo.BaseType.Name : "(none)"; }}
        public string Classification            { get { return _typeInfo.IsClass ? "Class" : (_typeInfo.IsInterface ? "Interface" : "Other"); }}
        public string ClassHierarchyString      { get { return BuildCollectionString(_classHierarchy, " > ", ""); }}
        public string ClassPropertiesString     { get { return BuildCollectionString(Properties, ", ", ""); }}
        public string ClassMethodsString        { get { return BuildCollectionString(Methods, "(), ", "()"); }}
        public string ClassEventsString         { get { return BuildCollectionString(Events, ", ", ""); }}
        public string Namespace                 { get { return _typeInfo.Namespace; } }
        public List<string> Properties          { get { return _typeInfo.DeclaredProperties.Select(pi => pi.Name).ToList(); } }
        public List<string> Events              { get { return _typeInfo.DeclaredEvents.Select(ei => ei.Name).ToList(); }}
        public List<string> ClassHierarchy      { get { return _classHierarchy; }}

        public List<string> Methods
        {
            get
            {
                // Find all public methods that are not properties ("get_xyz"/"put_xyz") or events ("add_xyz"/"put_xyz"),
                // which is what the IsSpecialName flag identifies
                var tmpList = (from mi in _typeInfo.DeclaredMethods 
                               where mi.IsPublic && !mi.IsSpecialName 
                               select mi.Name).ToList();

                return tmpList.Distinct().ToList();  // Use distinct to remove overloaded functions
            }
        }

        private void BuildClassHierarchy(Type type)
        {
            var typeInfo = type.GetTypeInfo();  // GetTypeInfo() is an extension method
            _classHierarchy.Add(typeInfo.Name);

            if (typeInfo.BaseType != null)
                BuildClassHierarchy(typeInfo.BaseType);
        }

        private static string BuildCollectionString(List<string> list, string separator, string finalSeparator)
        {
            var s = new StringBuilder();

            if (list != null && list.Count > 0)
            {
                for (var i = 0; i < (list.Count - 1); i++)
                    s.Append(list[i] + separator);

                s.Append(list[list.Count - 1]);
                s.Append(finalSeparator);
            }

            return s.ToString();
        }

    }
}
