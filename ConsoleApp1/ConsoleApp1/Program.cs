using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {

        static void Main(string[] args)
        {
            string path = @"C:\Users\zamoo\Desktop\Projects\labake_1\Text1.txt";

            if (!File.Exists(path))
            {
                throw new ArgumentException("File does not exists");
            }

            StreamReader sr = File.OpenText(path);
            string text = sr.ReadToEnd();
            sr.Close();
            InfoResearch.ShowTable(text);
        }
    }

    public static class InfoResearch
    {
        public static double CountOccurences(string symbol, string text)
        {
            int start = 0;
            int at = 0;
            double occurence = 0.00;

            while ((start < text.Length) && (at != -1))
            {
                at = text.IndexOf(symbol, start);
                if (at != -1)
                {
                    occurence += 1;
                }
                start = at + 1;
            }
            return occurence;
        }

        public static double CountProbability(string symbol, string text)
        {
            return Math.Round(CountOccurences(symbol, text) / text.Length, 3);
        }

        public static double CountEntropy(string text)
        {
            
            double entropy = 0;

            for (int i = 97; i < 122; i++)
            {
                string symbol = Char.ConvertFromUtf32(i);
                double prob = CountProbability(symbol, text);
                Console.WriteLine(symbol + " " + prob);
                if (prob != 0)
                {
                    entropy += prob * Math.Log(2, 1 / prob);
                }
            }
            Console.WriteLine("Entropy: {0}", entropy);
            return entropy;
        }

        public static double CountInformationAmount(string text)
        {
            double entropy = CountEntropy(text);
            Console.WriteLine("InformationAmount: {0}", entropy * text.Length);
            return entropy * text.Length / 8;
        }

        public static int Entropy(this string s)
        {
            var d = new Dictionary<char, bool>();
            foreach (char c in s)
                if (!d.ContainsKey(c)) d.Add(c, true);
            return d.Count();
        }

        public static int getEntropy(this string s)
        {
            var hs = new HashSet<char>();
            foreach (char c in s)
                hs.Add(c);
            return hs.Count();
        }

        public static void ShowTable(string text)
        {
            Console.OutputEncoding = Encoding.UTF8;

            double oc = 0;
            double entropy = 0;
            double prob;
            double infoAmount;

            string alphabet = "АаБбВвГгҐґДдЕеЄєЖжЗзИиІіЇїЙйКкЛлМмНнОоПпРрСсТтУуФфХхЦцЧчШшЩщЬьЮюЯя .,";
            char[] chars = alphabet.ToCharArray();
            char[] textSymbols = text.ToCharArray();

            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < textSymbols.Length; j++)
                { 
                    if (chars[i] == textSymbols[j])
                    {
                        oc = oc + 1;
                    };
                }

                prob = oc / text.Length;

                if (prob != 0)
                {
                    entropy += -(prob * Math.Log(2, prob));
                }
                else continue;

                Console.WriteLine("Символ {0} зустрічається {1} разів з імовірністю {2:f5};", chars[i], oc, prob);
                oc = 0;
            }
            
            infoAmount = entropy * text.Length / 8;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ентропія: {0:f4}", entropy);
            Console.WriteLine("Кількість інформації: {0:f4} байтів; Розмір файлу: {1} байтів;", infoAmount, new FileInfo(@"C:\Users\zamoo\Desktop\Projects\labake_1\Text1.txt").Length);
            Console.ResetColor(); 
        }

    }

}
