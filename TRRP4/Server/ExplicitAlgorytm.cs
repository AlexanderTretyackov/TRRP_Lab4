using System;
using System.Collections.Generic;

namespace VertexCoverProblem
{
    class ExplicitAlgorytm
    {
        private int[,] graph;
        public ExplicitAlgorytm(int[,] graph) {
            this.graph = graph;
        }

        // проверка является ли вершинным покрытием
        bool Check(List<int> verts)
        {
            var n = graph.GetLength(1);
            var tmp = (int[,]) graph.Clone();

            foreach (var v in verts)
            {
                for (var i = 0; i < n; i++)
                {
                    tmp[v, i] = tmp[i, v] = 0;
                    // удаляем все вершины, которые были инцидентны с данной
                }
            }

            foreach (var e in tmp)
            {
                // если хоть одна вершина осталась в графе
                // это не вершинное покрытие
                if (e != 0) return false;
            }
            return true;
        }
        



        List<int> CheckSubsets() {
            var n = graph.GetLength(0);
            var subsets = new List<List<int>>();
            subsets.Add(new List<int>());

            var minLength = n;
            List<int> mvc = null;

            // генерируем всевозможные наборы вершин (берем/не берем)
            for (var v = 0; v < n; v++) {
                var sl = subsets.Count;
                for (var i = 0; i < sl; i++) {
                    var ssl = subsets[i].Count;
                    List<int> ss = subsets[i].GetRange(0, ssl);
                    ss.Add(v);
                    subsets.Add(ss);
                }
            }

           foreach(var verts in subsets) {
                // проход по всем наборам

                var res = Check(verts);

                if (res)
                {
                    if (verts.Count < minLength)
                    {
                        minLength = verts.Count;
                        mvc = verts;
                    }
                }
           }

            return mvc;
        }


        public List<int> Solve()
        {
            return CheckSubsets();
            
        }
    }
}
