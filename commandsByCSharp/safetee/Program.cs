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
        [STAThreadAttribute]    // WindowsFormsを使用する際の指定必須の属性（おまじない）
        static void Main(string[] args)
        {
            // コマンド引数解析オブジェクト
            Getopt g = new Getopt("safetee", args, "?a:cf:sun");
            int c;                                  // オプション文字
            string arg;                             // オプション引数文字
            string inputFileName = string.Empty;    // 入力ファイル名
            string appendFileName = string.Empty;   // 出力ファイル名（追加書込のみ）
            bool isInputIsStdin = true;             // 入力ファイルを標準入力とする
            bool isCopyToClipboard = false;         // 入力ファイルの内容をクリップボードにコピーする
            string lang = "Shift_JIS";              // ファイルの文字種モード（Shift_JISか、UTF-8のみ指定可能）
            bool isNotSilentMode = true;            // 標準出力に出力する（true）、しない（false）
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

                    case 'n':
                        // サイレントモード（標準出力に出力しない）
                        isNotSilentMode = false;
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
                        // ヘルプ表示
                        Console.WriteLine("c:\\>safetee [-c] [-n] [-s|-u] [-a <追加書込ファイル>] [-f <読込先ファイル>] [< text.txt] ");
                        Console.WriteLine("  -c : クリップボードにコピー。");
                        Console.WriteLine("  -n : 標準出力に出力しない。");
                        Console.WriteLine("  -s : Shift_JISモード。");
                        Console.WriteLine("  -u : UTF-8モード。");
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
                // 本当はtry～catchが必要
            }

            FileStream fs = null;                       // 出力ファイル
            StreamWriter w = null;                      // 出力ファイルへの書込ユーティリティ
            string line = string.Empty;                 // 1行バッファ用の文字列
            StringBuilder sb = new StringBuilder();     // クリップボード用に入力ファイルの全行を貯める文字列バッファ
            try
            {
                if (!string.IsNullOrEmpty(appendFileName))
                {
                    // 出力ファイル名の指定あり
                    try
                    {
                        // 出力ファイルを追加書込モードで開く
                        fs = File.Open(appendFileName, FileMode.Append);
                    }
                    catch (Exception ex)
                    {

                        fs = null;
                    }
                }

                // 出力ファイルへの書込ユーティリティ
                if (fs != null) w = new StreamWriter(fs, System.Text.Encoding.GetEncoding(lang));

                // 入力ファイルを1行ずつ読み込む
                while ((line = input.ReadLine()) != null)
                {
                    if (isNotSilentMode)    // 標準出力への出力モード？
                    {
                        // 標準出力へ1行出力
                        Console.WriteLine(line);
                    }
                    if (w != null)  // ファイルへの追加モード？
                    {
                        // ファイルへ1行出力
                        w.WriteLine(line);
                        // ファイルへの出力の書込を直ちに行う。
                        w.Flush();
                    }
                    if (sb != null) // クリップボードコピーモード？
                    {
                        // クリップボード用文字バッファへ1行追加
                        sb.Append(line + "\n");
                    }
                }

                if (isCopyToClipboard)  // クリップボードコピーモード？
                {
                    // クリップボードへコピー
                    Clipboard.SetText(sb.ToString());
                }
            }
            catch (Exception excp_)
            {
                // ここでは例外処理をしていない。
                // なんらかのエラーメッセージを出力しても良い。
            }
            finally
            {
                // 入力ファイルと、出力ファイルのオブジェクトの資源（リソース）を破棄
                input.Dispose();
                if (fs != null) fs.Dispose();
            }
        }
            
    }
}
