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
            Getopt g = new Getopt("columnconv", args, "?d:e:hn");
            int c;
            string arg;
            string output_delim = " ";      // 規定の区切り文字は「半角空白」
            string regexp = string.Empty;
            while ((c = g.getopt()) != -1)
            {
                switch (c)
                {
                    case 'd':
                        // 出力時のデリミタ（区切り文字）
                        arg = g.Optarg;
                        // もし-dオプションにデリミタ（区切り文字）が指定されていた場合、
                        // それを、出力のデリミタ（区切り文字）とする。
                        // 指定されていなかった場合、「半角空白」をデリミタ（区切り文字）とする。
                        output_delim = (string.IsNullOrEmpty(arg) ? " " : arg );
                        tabCount(ref output_delim);
                        crlfCount(ref output_delim);
                        break;

                    case 'n':
                        // デリミタなし->空文字""をデリミタ（区切り文字）とする。
                        output_delim = string.Empty;
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
                        Console.WriteLine("c:\\>columnconv [-n] -d <出力時デリミタ> < text.txt ");
                        Console.WriteLine("  入力用デリミタ　　: 半角空白、又はタブ、又はそれらの連続した文字列");
                        Console.WriteLine("  特殊出力時デリミタ: tab cr");
                        Console.WriteLine("  -n 　　　　　　　 : 出力時デリミタを空文字\"\"とする。");
                        Console.WriteLine("  -e 　　　　　　　 : （未実装）入力を分割するデリミタの正規表現を指定する。");
                        return;
                        //break; // getopt() already printed an error
                }
            }

            // ReferenceURL: https://www.atmarkit.co.jp/fdotnet/dotnettips/681stdin/stdin.html
            TextReader input = Console.In;
            string line = string.Empty;
            string ret = string.Empty;

            while ((line = input.ReadLine()) != null)   // 標準入力から1行ずつ読み込む
            {   
                // 1行を入力デリミタで分割し、出力デリミタで結合する。
                ret = colconv(line, output_delim);
                // 1行を出力する。
                Console.WriteLine(ret);
            }
        }

        // 単純実装版
        static string colconv(string line, string column_delim = " ")
        {
            string ret = string.Empty;
            string[] tmp = null;

            // 1行を「半角空白」または「タブ文字」またはそれらの連続した文字列で、区切って、
            // 文字列の配列にする。
            tmp = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // 配列に分割した一行を、-dオプションで指定したデリミタ（区切り文字）で区切って、再度1行にする。
            ret = string.Join(column_delim, tmp);

            // 1行を返す。
            return ret;

        }

        /// <summary>
        /// 指定した文字列がいくつあるか数える。
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

        /// <summary>
        /// -dオプション指定デリミタ（区切り文字）中の文字「tab」の個数分、出力デリミタに'\t'（タブ）を追加
        /// </summary>
        /// <param name="output_delim">出力デリミタ</param>
        private static void tabCount(ref string output_delim)
        {
            // 出力デリミタ文字列中の「tab」の数を数える。
            int tabcount = CountOf(output_delim, new string[] { "tab", "TAB" });

            // 「tab」の数分ループ
            if (tabcount > 0)
            {
                output_delim = string.Empty;
                for (int i = 0; i < tabcount; i++)
                {
                    // 「tab」の個数分、出力デリミタに'\t'を加える
                    output_delim += "\t";
                }
            }
        }
        /// <summary>
        /// -dオプション指定デリミタ（区切り文字）中の文字「cr」または「lf」の個数分、出力デリミタに'\n'（改行）を追加
        /// </summary>
        /// <param name="output_delim">出力デリミタ</param>
        private static void crlfCount(ref string output_delim)
        {
            // 出力デリミタ文字列中の「cr」と「lf」の数を数える。
            int crcount = CountOf(output_delim, new string[] { "cr", "CR", "lf", "LF" });

            // 「cr」や「lf」の数分ループ
            if (crcount > 0)
            {
                output_delim = string.Empty;
                for (int i = 0; i < crcount; i++)
                {
                    // 「cr」または「lf」の個数分、出力デリミタに'\n'を加える
                    output_delim += "\n";
                }
            }
        }
    }
}
