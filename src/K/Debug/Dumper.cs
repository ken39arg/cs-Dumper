using System;
using System.Collections;
using System.Reflection;
namespace K.Debug 
{
    /// <summary>
    /// Object Dumper.
    /// like perl Data::Dumper, php ver_dump
    /// </summary>
    /// <example>
    /// <code>
    /// MyClass myObj = new MyClass();
    /// // any actions...
    /// Console.WriteLine(K.Debug.Dumper.Dump(myObj));
    /// // if you develop by unity
    /// UnityEngine.Debug.Log(K.Debug.Dumper.Dump(myObj));
    /// // if you want simple dump
    /// K.Debug.Dumper.Verbose = false;
    /// Console.WriteLine(K.Debug.Dumper.Dump(myObj));
    /// // or
    /// Console.WriteLine(K.Debug.Dumper.Dump(myObj, false));
    /// // If you want to narrow the indentation
    /// K.Debug.Dumper.Indent = 2;
    /// Console.WriteLine(K.Debug.Dumper.Dump(myObj));
    /// // or
    /// Console.WriteLine(K.Debug.Dumper.Dump(myObj, 2));
    /// </code>
    /// </example>
    public class Dumper
    {
        /// <summary>
        /// size of indentation. (default: 4)
        /// </summary>
        public static int Indent   = 4;

        /// <summary>
        /// Flags for adding type information (default: true)
        /// </summary>
        public static bool Verbose = true;

        /// <summary>
        /// Can display up to a maximum depth nested objects (default: 5)
        /// </summary>
        public static int MaxDepth = 5;

        /// <summary>
        /// Dump object
        /// </summary>
        /// <param name="d">object data</param>
        /// <returns>string of dumped object</returns>
        public static string Dump(object d)
        {
            return Dump(d, Indent, Verbose, MaxDepth);
        }

        /// <summary>
        /// Dump object
        /// </summary>
        /// <param name="d">object data</param>
        /// <param name="indent">size of indentation</param>
        /// <returns>string of dumped object</returns>
        public static string Dump(object d, int indent)
        {
            return Dump(d, indent, Verbose, MaxDepth);
        }

        /// <summary>
        /// Dump object
        /// </summary>
        /// <param name="d">object data</param>
        /// <param name="verbose">Flags for adding type information</param>
        /// <returns>string of dumped object</returns>
        public static string Dump(object d, bool verbose)
        {
            return Dump(d, Indent, verbose, MaxDepth);
        }

        /// <summary>
        /// Dump object
        /// </summary>
        /// <param name="d">object data</param>
        /// <param name="indent">size of indentation</param>
        /// <param name="verbose">Flags for adding type information</param>
        /// <returns>string of dumped object</returns>
        public static string Dump(object d, int indent, bool verbose, int maxDepth)
        {
            DumpObject dmp = new DumpObject(d, indent, verbose, maxDepth);
            return dmp.Dump();
        }
    }

    class DumpObject
    {
        const string SPACE = " ";
        const string CRLF  = "\r\n";
        static Type  StringType = typeof(string);
        static Type  BoolType   = typeof(bool);

        int indent   = 4;
        bool verbose = true;
        int maxDepth = 5;

        object data;

        public DumpObject(object d)
        {
            this.data = d;
            this.indent   = Dumper.Indent;
            this.verbose  = Dumper.Verbose;
            this.maxDepth = Dumper.MaxDepth;
        }

        public DumpObject(object d, int indent, bool verbose, int maxDepth)
        {
            this.data     = d;
            this.indent   = indent;
            this.verbose  = verbose;
            this.maxDepth = maxDepth;
        }

        public string Dump()
        {
            return _Dump(data, 0, true);
        }

        private string _Dump(object d, int level, bool displayType)
        {
            if (d == null) {
                return "Null";
            }

            string dumpText = "";
            Type dType = d.GetType();

            Type[] argTypes = dType.GetGenericArguments();

            if (displayType && verbose)
            {
                dumpText += dType.Name;
                if (0 < argTypes.Length)
                {
                    dumpText += "<";
                    for (int i=0; i<argTypes.Length; i++)
                    {
                        if (i == 0)
                        {
                            dumpText += argTypes[i].Name;
                        }
                        else
                        {
                            dumpText += ", " + argTypes[i].Name;
                        }
                    }
                    dumpText += ">";
                }
            }

            if (dType.IsPrimitive) 
            {
                if (displayType && verbose)
                    dumpText = "(" + dumpText + ") "; 
                dumpText += d.ToString();
            } 
            else if (dType == StringType) 
            {
                if (displayType && verbose)
                    dumpText = "(" + dumpText + ") "; 
                dumpText += "\"" + ((string) d) + "\"";
            } 
            else if (dType == BoolType) 
            {
                if (displayType && verbose)
                    dumpText = "(" + dumpText + ") "; 
                 dumpText += ((bool) d ? "True" : "False");
            }
            else if (maxDepth < level)
            {
                dumpText += " ...";
            }
            else if (d is IDictionary)
            {
                bool valueVerbose = ValueVerboseType(argTypes[1]);
                dumpText += " {" + CRLF;
                IDictionary dict = d as IDictionary;
                foreach (object key in dict.Keys)
                {
                    dumpText += BuildIndent(level + 1);
                    dumpText += key.ToString() + " : ";
                    dumpText += _Dump(dict[key], level + 1, valueVerbose);
                    dumpText += "," + CRLF;
                }
                dumpText += BuildIndent(level);
                dumpText += "}";
            }
            else if (d is IList)
            {
                bool valueVerbose = 0 < argTypes.Length && ValueVerboseType(argTypes[0]);
                dumpText += " [" + CRLF;
                IList list = d as IList;
                foreach (object item in list)
                {
                    dumpText += BuildIndent(level + 1);
                    dumpText += _Dump(item, level + 1, valueVerbose);
                    dumpText += "," + CRLF;
                }
                dumpText += BuildIndent(level);
                dumpText += "]";
            }
            else
            {
                dumpText += " (" + CRLF;
                foreach (FieldInfo dFieldInfo in dType.GetFields()) 
                {
                    object val = dFieldInfo.GetValue(d);
                    Type   fType = dFieldInfo.FieldType;
                    bool valueVerbose = ValueVerboseType(fType);
                    dumpText += BuildIndent(level + 1);
                    if (verbose)
                    {
                        dumpText += "(" +fType.Name+") ";
                    }
                    dumpText += dFieldInfo.Name + " = ";
                    dumpText += _Dump(val, level + 1, valueVerbose);
                    dumpText += ";" + CRLF;
                }
                foreach (PropertyInfo dPropertyInfo in dType.GetProperties()) 
                {
                    Type   fType = dPropertyInfo.PropertyType;
                    object val  = dPropertyInfo.GetValue(d, null);
                    bool valueVerbose = ValueVerboseType(fType);
                    dumpText += BuildIndent(level + 1);
                    if (verbose)
                    {
                        dumpText += "(" +fType.Name+") ";
                    }
                    dumpText += dPropertyInfo.Name + " = ";
                    dumpText += _Dump(val, level + 1, valueVerbose);
                    dumpText += ";" + CRLF;
                }
                dumpText += BuildIndent(level);
                dumpText += ")";
            }
            return dumpText;
        }

        private string BuildIndent(int level)
        {
            string indentStr = "";

            for (int i = 0; i < level * indent; i++)
            {
                indentStr += SPACE;
            }

            return indentStr;
        }

        private bool ValueVerboseType( Type t )
        {
            return  !t.IsPrimitive && t != StringType && t != BoolType;
        }
    }
}
