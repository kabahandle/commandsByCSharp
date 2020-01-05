using Gnu.Getopt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace safetee
{
    class Program
    {
        static void Main(string[] args)
        {
            Getopt g = new Getopt("columnconv", args, "?a:cf:su");
            int c;
            string arg;
            string inputFileName = string.Empty;
            string appendFileName = string.Empty;
            bool isInputIsStdin = true;
            bool isCopyToClipboard = false;
            string lang = "Shift_JIS";
            while ((c = g.getopt()) != -1)
            {
                switch (c)
                {
                    case 'c':
                        // クリップボード
                        isCopyToClipboard = true;
                        break;

                    case 's':
                        // SJISモード
                        lang = "Shift_JIS";
                        break;

                    case 'u':
                        // UTF-8モード
                        lang = "UTF-8";
                        break;



                    case 'a':
                        // 追加書込出力先ファイル名
                        // 拡張用（未実装）
                        arg = g.Optarg;
                        appendFileName = ((arg != null) ? arg : string.Empty);
                        break;

                    case 'f':
                        // 入力先ファイル名
                        arg = g.Optarg;
                        inputFileName = ((arg != null) ? arg : string.Empty);
                        if (!string.IsNullOrEmpty(inputFileName))
                        {
                            isInputIsStdin = false;
                        }
                        break;

                    default:
                    case '?':
                    case 'h':
                        Console.WriteLine("c:\\>safetee [-c] [-a <追加書込ファイル>] [-f <読込先ファイル>] [< text.txt] ");
                        Console.WriteLine("  -c : クリップボードにコピー。・");
                        return;
                        //break; // getopt() already printed an error
                }
            }


            // 参考URL：https://www.atmarkit.co.jp/fdotnet/dotnettips/681stdin/stdin.html
            TextReader input;
            if (isInputIsStdin || string.IsNullOrEmpty(inputFileName))
            {
                // 読み込み元は標準入力
                input = Console.In;
            }
            else
            {
                // 読み込み元はファイル
                input = new StreamReader(inputFileName,
                          System.Text.Encoding.GetEncoding(lang));
            }

            FileStream fs = null;
            StreamWriter w = null;
            string line = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                try
                {
                    fs = File.Open(appendFileName, FileMode.Append);
                }
                catch (Exception ex)
                {

                    fs = null;
                }

                if (fs != null) w = new StreamWriter(fs, System.Text.Encoding.GetEncoding(lang));

                while ((line = input.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                    if (w != null)
                    {
                        w.WriteLine(line);
                        w.Flush();
                    }
                    if (sb != null)
                    {
                        sb.Append(line + "\n");
                    }
                }

                if (isCopyToClipboard)
                {
                    Clipboard.SetText(sb.ToString());
                }
            }
            catch (Exception excp_)
            {
            }
            finally
            {
                input.Dispose();
                if (fs != null) fs.Dispose();
            }
        }
            
    }
}
