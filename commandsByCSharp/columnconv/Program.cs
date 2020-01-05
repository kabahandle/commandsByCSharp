using Gnu.Getopt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace columnconv
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //Getopt g = new Getopt("colconv", args, "ab:c::d");
            //int c;
            //string arg;
            //while ((c = g.getopt()) != -1)
            //{
            //    switch (c)
            //    {
            //        case 'a':
            //        case 'd':
            //            Console.WriteLine("You picked " + (char)c);
            //            break;

            //        case 'b':
            //        case 'c':
            //            arg = g.Optarg;
            //            Console.WriteLine("You picked " + (char)c +
            //            " with an argument of " +
            //            ((arg != null) ? arg : "null"));
            //            break;

            //        case '?':
            //            break; // getopt() already printed an error

            //        default:
            //            Console.WriteLine("getopt() returned " + c);
            //            break;
            //    }
            //}
            Getopt g = new Getopt("colconv.dll", args, "?d:e:h");
            int c;
            string arg;
            string output_delim = " ";
            string regexp = string.Empty;
            while ((c = g.getopt()) != -1)
            {
                switch (c)
                {
                    case 'd':
                        // 出力時のデリミタ
                        arg = g.Optarg;
                        output_delim = (string.IsNullOrEmpty(arg) ? " " : arg );
                        //int tabcount = CountOf(output_delim, new string[] { "tab", "TAB" });
                        //if (tabcount > 0)
                        //{
                        //    output_delim = string.Empty;
                        //    for (int i = 0; i < tabcount; i++)
                        //    {
                        //        output_delim += "\t";
                        //    }
                        //}
                        //if( "tab".Equals(output_delim) 
                        //    || "TAB".Equals(output_delim) ) {
                        //    output_delim = "\t";
                        //}
                        tabCount(ref output_delim);
                        crlfCount(ref output_delim);
                        break;

                    case 'e':
                        // 分割する文字の正規表現
                        // 拡張用（未実装）
                        arg = g.Optarg;
                        regexp = ((arg != null) ? arg : string.Empty);
                        break;

                    default:
                    case '?':
                    case 'h':
                        Console.WriteLine("c:\\>columnconv -d <出力時デリミタ> < text.txt ");
                        Console.WriteLine("<特殊出力時デリミタ>: tab cr");
                        return;
                        //break; // getopt() already printed an error
                }
            }

            // ReferenceURL: https://www.atmarkit.co.jp/fdotnet/dotnettips/681stdin/stdin.html
            TextReader input = Console.In;
            string line = string.Empty;
            string ret = string.Empty;

            while ((line = input.ReadLine()) != null)
            {
                ret = colconv(line, output_delim);
                Console.WriteLine(ret);
            }
        }

        // 単純実装版
        static string colconv(string line, string column_delim = " ")
        {
            string ret = string.Empty;
            string[] tmp = null;

            tmp = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            ret = string.Join(column_delim, tmp);

            return ret;

        }

        /// <summary>
        /// 指定した文字列がいくつあるか
        /// 参考URL：http://kan-kikuchi.hatenablog.com/entry/CountOf
        /// </summary>
        public static int CountOf(string target, params string[] strArray)
        {
            int count = 0;

            foreach (string str in strArray)
            {
                int index = target.IndexOf(str, 0);
                while (index != -1)
                {
                    count++;
                    index = target.IndexOf(str, index + str.Length);
                }
            }

            return count;
        }

        private static void tabCount(ref string output_delim)
        {
            string ret = string.Empty;
            int tabcount = CountOf(output_delim, new string[] { "tab", "TAB" });
            if (tabcount > 0)
            {
                output_delim = string.Empty;
                for (int i = 0; i < tabcount; i++)
                {
                    output_delim += "\t";
                }
            }
        }
        private static void crlfCount(ref string output_delim)
        {
            string ret = string.Empty;
            int tabcount = CountOf(output_delim, new string[] { "cr", "CR", "lf", "LF" });
            if (tabcount > 0)
            {
                output_delim = string.Empty;
                for (int i = 0; i < tabcount; i++)
                {
                    output_delim += "\n";
                }
            }
        }
    }
}
