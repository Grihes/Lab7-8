using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab7
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
           
        }
        public static bool[,] Percol(double p, int l)
        {
            bool[,] array = new bool[l, l];
            Random rnd = new Random();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (rnd.NextDouble() <= p)
                        array[i, j] = true; ;
                }
            }
            return array;
        }
        public static int[,] SetLabels(bool[,] array)
        {
            int label = 0;
            int[,] labels = new int[array.GetLength(0), array.GetLength(1)];
            int[] np = new int[array.GetLength(0) * array.GetLength(1) ];
            for (int i=0; i<np.Length; i++)
            {
                np[i] = i;
            }
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    if (array[i, j])
                    {
                        if (i == 0 && j == 0)
                        {
                            label++;
                            labels[i, j] = label;
                        }
                        else if (i == 0 && j != 0)
                        {
                            if (labels[i, j-1] == 0)
                            {
                                label++;
                                labels[i, j] = label;
                            }
                            else
                                labels[i, j] = FindLabel(labels[i, j-1] , np );
                        }
                        else if (j == 0 && i != 0)
                        {
                            if (labels[i-1, j ] == 0)
                            {
                                label++;
                                labels[i, j] = label;
                            }
                            else
                                labels[i, j] = FindLabel(labels[i-1, j ] , np );
                        }
                        else if (i != 0 && j != 0)
                        {
                            if (labels[i, j - 1] == 0 && labels[i - 1, j] == 0)
                            {
                                label++;
                                labels[i, j] = label;
                            }
                            else if (labels[i, j - 1] == 0 && labels[i - 1, j] != 0)
                            {
                                labels[i, j] = FindLabel(labels[i - 1, j], np );
                            }
                            else if (labels[i - 1, j] == 0 && labels[i, j - 1] != 0)
                            {
                                labels[i, j] = FindLabel(labels[i, j - 1], np);
                            }
                            else if (labels[i - 1, j] != 0 && labels[i, j - 1] != 0)
                            {
                                int replaceableValue = FindLabel(Math.Max(labels[i, j - 1], labels[i - 1, j]), np);
                                int trueValue = FindLabel(Math.Min(labels[i, j - 1], labels[i - 1, j]), np);
                                for (int t = 0; t < np.Length; t++)
                                {
                                    if (np[t] == replaceableValue)
                                        np[t] = trueValue;
                                }
                                labels[i, j] = FindLabel(labels[i-1, j ], np );
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                        labels[i,j] = np[labels[i,j]];
                }
            }

            return labels;
        }
    public static int FindLabel(int x, int[] np)
        {
            int y = x;
            while (np[y]!=y)
            {
                y = np[y];
            }
            while (np[x]!= x)
            { 
               int z = np[x];
               np[x] =  y;
               x = z;
            }
            return y;
        }
        public static int FindUnitClustersLabel (bool[,] array)
        {
            int[,] labels = new int[array.GetLength(0), array.GetLength(1)];
            labels = SetLabels(array);
            int [] labelsTopRow =new int[ array.GetLength(1)];
            int[] labelsBotRow = new int[ array.GetLength(1)];
            int unitClastersLabel = 0;
            for (int i =0; i< array.GetLength(1); i++)
            {
                labelsTopRow[i] = labels[1,i];
                labelsBotRow[i] = labels[array.GetLength(0) - 1, i];
            }
            foreach( int element in labelsTopRow )
            {
                if (Array.IndexOf(labelsBotRow, element) != -1 && element != 0)
                {
                    unitClastersLabel = element;
                    break;
                }
                else
                    unitClastersLabel = 0;
            }
            return unitClastersLabel;
        }
        public static Dictionary<int, int> FindClustersSize(bool[,] array)
        {
            int[,] labels = new int[array.GetLength(0), array.GetLength(1)];
            labels = SetLabels(array);
            Dictionary<int, int> clasters = new Dictionary<int, int>();
            foreach (int element in labels)
            {
                if (clasters.ContainsKey(element)&&(element!=0))
                    clasters[element]++;
                else if (!clasters.ContainsKey(element) && (element != 0))
                    clasters.Add(element, 1);
            }
            return clasters;
        }
        public static Dictionary<int, int> FindClustersNumberS(bool[,] array)
        {
            Dictionary<int, int> clastersNumberS = new Dictionary<int, int>();
            Dictionary<int, int> clasters = FindClustersSize(array);
            foreach (int element in clasters.Values)
            {
                if (clastersNumberS.ContainsKey(element))
                    clastersNumberS[element]++;
                else
                    clastersNumberS.Add(element, 1);
            }
            return clastersNumberS;
        }
        public static double FindAverageClusterSize(bool [,] array)
        {
            Dictionary<int, int> clasters = FindClustersSize(array);
            double generalSize = 0;
            double averageSize = 0;
            double clastersNumber = 0;
            foreach (var element in clasters)
            {
                generalSize += element.Value;
                clastersNumber++;
            }
            averageSize = generalSize / clastersNumber;
            return averageSize;
        }
        public static double FindProbability (bool[,] array , int l)
        {
            int label = FindUnitClustersLabel(array);
            Dictionary<int, int> clasters = FindClustersSize(array);
            double unitClusterSize = 0;
            if (clasters.ContainsKey(label))
                unitClusterSize = clasters[label];
            double probability = unitClusterSize / (l * l);
            return probability;
        }
    }
}
