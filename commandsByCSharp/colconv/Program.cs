using Gnu.Getopt;
using System;
using System.IO;

namespace colconv
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
            Getopt g = new Getopt("colconv.dll", args, "d:e:h");
            int c;
            string arg;
            string output_delim = string.Empty;
            while ((c = g.getopt()) != -1)
            {
                switch (c)
                {
                    case 'd':
                        // 出力時のデリミタ
                        arg = g.Optarg;
                        output_delim = ((arg != null) ? arg : string.Empty);
                        break;

                    case 'e':
                        // 分割する文字の正規表現
                        // 拡張用（未実装）
                        arg = g.Optarg;
                        output_delim = ((arg != null) ? arg : string.Empty);
                        break;

                    default:
                    case '?':
                    case 'h':
                        Console.WriteLine("c:\\>colcut -d <出力時デリミタ> < text.txt ");
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
                ret = colconv(line);
                Console.WriteLine(ret);
            }
        }

        // 単純実装版
        static string colconv(string line)
        {
            string ret = string.Empty;
            string[] tmp = null;

            tmp = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            ret = string.Join(' ', tmp);

            return ret;
         
        }
    }
}
