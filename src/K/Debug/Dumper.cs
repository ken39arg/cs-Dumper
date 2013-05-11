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
        const string SPACE = " ";
        const string CRLF  = "\r\n";

        /// <summary>
        /// size of indentation. (default: 4)
        /// </summary>
        public static int Indent   = 4;

        /// <summary>
        /// Flags for adding type information (default: true)
        /// </summary>
        public static bool Verbose = true;

        /// <summary>
        /// Dump object
        /// </summary>
        /// <param name="d">object data</param>
        /// <returns>string of dumped object</returns>
        public static string Dump(object d)
        {
            return Dump(d, Indent, Verbose);
        }

        /// <summary>
        /// Dump object
        /// </summary>
        /// <param name="d">object data</param>
        /// <param name="indent">size of indentation</param>
        /// <returns>string of dumped object</returns>
        public static string Dump(object d, int indent)
        {
            return Dump(d, indent, Verbose);
        }

        /// <summary>
        /// Dump object
        /// </summary>
        /// <param name="d">object data</param>
        /// <param name="verbose">Flags for adding type information</param>
        /// <returns>string of dumped object</returns>
        public static string Dump(object d, bool verbose)
        {
            return Dump(d, Indent, verbose);
        }

        /// <summary>
        /// Dump object
        /// </summary>
        /// <param name="d">object data</param>
        /// <param name="indent">size of indentation</param>
        /// <param name="verbose">Flags for adding type information</param>
        /// <returns>string of dumped object</returns>
        public static string Dump(object d, int indent, bool verbose)
        {
            return _Dump(d, 0, indent, verbose);
        }

        private static string _Dump(object d, int level, int indent, bool verbose)
        {
            if (d == null) {
                return "Null";
            }

            Type dType = d.GetType();

            if (dType.IsPrimitive) 
            {
                return d.ToString();
            } 
            else if (dType == typeof(string)) 
            {
                return "\"" + ((string) d) + "\"";
            } 
            else if (dType == typeof(bool)) 
            {
                 return ((bool) d ? "True" : "False");
            }
            else if (d is IDictionary)
            {
                string dumpText = "";
                if (verbose) {
                    Type[] argTypes = dType.GetGenericArguments();
                    Type kType = argTypes[0];
                    Type vType = argTypes[1];
                    dumpText += dType.Name + "<" + kType.Name + "," + vType.Name + "> ";
                }
                dumpText += "{" + CRLF;
                IDictionary dict = d as IDictionary;
                foreach (object key in dict.Keys)
                {
                    dumpText += BuildIndent(level + 1, indent);
                    dumpText += key.ToString() + " : ";
                    dumpText += _Dump(dict[key], level + 1, indent, verbose) + ",";
                    dumpText += CRLF;
                }
                dumpText += BuildIndent(level, indent) + "}";
                return dumpText;
            }
            else if (d is IList)
            {
                string dumpText = "";
                if (verbose) {
                    Type[] argTypes = dType.GetGenericArguments();
                    Type vType = argTypes[0];
                    dumpText = dType.Name + "<" + vType.Name + "> ";
                }
                dumpText += "[" + CRLF;
                IList list = d as IList;
                foreach (object item in list)
                {
                    dumpText += BuildIndent(level + 1, indent);
                    dumpText += _Dump(item, level + 1, indent, verbose) + ",";
                    dumpText += CRLF;
                }
                dumpText += BuildIndent(level, indent) + "]";
                return dumpText;
            }
            else
            {
                string dumpText = dType.Name + " (";
                dumpText += CRLF;
                foreach (FieldInfo dFieldInfo in dType.GetFields()) 
                {
                    object val = dFieldInfo.GetValue(d);
                    Type   fType = dFieldInfo.FieldType;
                    dumpText += BuildIndent(level + 1, indent);
                    if (verbose) {
                        dumpText += "(" +fType.Name+") ";
                    }
                    dumpText += dFieldInfo.Name + " = ";
                    dumpText += _Dump(val, level + 1, indent, verbose) + ";";
                    dumpText += CRLF;
                }
                foreach (PropertyInfo dPropertyInfo in dType.GetProperties()) 
                {
                    Type   fType = dPropertyInfo.PropertyType;
                    object val  = dPropertyInfo.GetValue(d, null);
                    dumpText += BuildIndent(level + 1, indent);
                    if (verbose) {
                        dumpText += "(" +fType.Name+") ";
                    }
                    dumpText += dPropertyInfo.Name + " = ";
                    dumpText += _Dump(val, level + 1, indent, verbose) + ";";
                    dumpText += CRLF;
                }
                dumpText += BuildIndent(level, indent) + ")";
                return dumpText;
            }
        }

        private static string BuildIndent(int level, int indentNum)
        {
            string indent = "";

            for (int i = 0; i < level * indentNum; i++)
            {
                indent += SPACE;
            }

            return indent;
        }
    }
}
