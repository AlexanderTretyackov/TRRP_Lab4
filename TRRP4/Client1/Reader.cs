using System;
using System.IO;

public static class Reader
{
    public static int[,] Read(string fileName)
    {

        int[,] graph = null;
        string[] line;
        using (StreamReader sr = new StreamReader(fileName))
        {
            line = sr.ReadLine().Split(' ');

            if (line[0] == "p")
            {
                int n = int.Parse(line[2]);
                graph = new int[n, n];
            }
            else
            {
                throw new Exception("Некорректные данные");
            }

            while (!sr.EndOfStream)
            {
                line = sr.ReadLine().Split(' ');

                if (line[0] == "e")
                {
                    int i = int.Parse(line[1]) - 1;
                    int j = int.Parse(line[2]) - 1;

                    graph[i, j] = graph[j, i] = 1;
                }
                else
                {
                    throw new Exception("Некорректные данные");
                }
            }
        }

        return graph;
    }
}
