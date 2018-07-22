using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
namespace Algo_Spring2016_TermProject_i13_1821_i13_1822
{
    public class Floyd
    {
        public void Run(ref List<List<int>> AdjMatrix, ref int[,] FloydPath, bool isShortest)
        {
            int iterations = isShortest ? AdjMatrix.Count : AdjMatrix.Count - 29;

            for (int i = 0; i < AdjMatrix.Count; i++)
            {
                for (int j = 0; j < AdjMatrix.Count; j++)
                {
                    FloydPath[i, j] = 0;


                    if (i != j && AdjMatrix[i][j] == 0)
                    {
                        AdjMatrix[i][j] = int.MaxValue / 2;
                    }
                    else
                    {
                        AdjMatrix[i][j] = i == j ? 0 : AdjMatrix[i][j];
                    }

                }
            }
            for (int k = 0; k < iterations; k++)
            {
                for (int i = 0; i < AdjMatrix.Count; i++)
                {
                    for (int j = 0; j < AdjMatrix.Count; j++)
                    {
                        if (AdjMatrix[i][j] > AdjMatrix[i][k] + AdjMatrix[k][j])
                        {
                            AdjMatrix[i][j] = AdjMatrix[i][k] + AdjMatrix[k][j];
                            FloydPath[i, j] = k+1;
                        }

                    }
                }

            }
        }
    }
}