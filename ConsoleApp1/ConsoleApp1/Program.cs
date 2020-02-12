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
            string path = @"C:\Users\zamoo\Desktop\Projects\labake_1\Text3.txt";
            FileInfo file = new FileInfo(path);

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

        public static double CountInformationAmount(string text)
        {
            double entropy = ShannonEntropy(text);
            Console.WriteLine("InformationAmount: {0}", entropy * text.Length);
            return entropy * text.Length / 8;
        }

        public static double ShannonEntropy(string s)
        {
            var map = new Dictionary<char, int>();
            foreach (char c in s)
            {
                if (!map.ContainsKey(c))
                {
                    map.Add(c, 1);
                }
                else
                {
                    map[c] += 1;
                }
            }

            int len = s.Length;
            double result = 0D;
            foreach (var item in map.Values)
            {
                var frequency = (double)item / len;
                result -= frequency * Math.Log(frequency);
            }
            result /= Math.Log(2);

            return result;
        }

        public static void ShowTable(string text)
        {
            Console.OutputEncoding = Encoding.UTF8;

            double oc = 0;
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

                if (prob == 0) continue;

                Console.WriteLine("Символ {0} зустрічається {1} разів з імовірністю {2:f5};", chars[i], oc, prob);
                oc = 0;
            }
            
            infoAmount = ShannonEntropy(text) * text.Length / 8;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ентропія: {0:f4}", ShannonEntropy(text));
            Console.WriteLine("Кількість інформації: {0:f4} байтів; Розмір файлу: {1} байтів;", infoAmount, new FileInfo(@"C:\Users\zamoo\Desktop\Projects\labake_1\Text3.txt").Length);
            Console.ResetColor(); 
        }

    }

}
