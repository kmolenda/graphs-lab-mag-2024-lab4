
using System.Text;

public static class GraphAlgorithms
{
    // Przeglądanie grafu w głąb g, począwszy od wierzchołka `start`
    public static IEnumerable<T> DFS<T>(this IGraphNonWeighted<T> g, T start) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        var stack = new Stack<T>();
        stack.Push(start);
        while (stack.Count > 0)
        {
            var current = stack.Pop();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            yield return current;
            foreach (var neighbour in g.Neighbours(current))
                stack.Push(neighbour);
        }
    }

    // Przeglądanie grafu wszerz g, począwszy od wierzchołka `start`
    public static IEnumerable<T> BFS<T>(this IGraphNonWeighted<T> g, T start) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        var queue = new Queue<T>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (visited.Contains(current))
                continue;
            visited.Add(current);
            yield return current;
            foreach (var neighbour in g.Neighbours(current))
                queue.Enqueue(neighbour);
        }
    }

    // Konwerter grafu na notację DOT
    public static string ToDot<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        var sb = new StringBuilder();
        sb.AppendLine("graph {");
        foreach (var vertex in g.Vertices)
            sb.AppendLine($"\t{vertex};");
        foreach (var (v1, v2) in g.Edges)
            sb.AppendLine($"\t{v1} -- {v2};");
        sb.AppendLine("}");
        return sb.ToString();
    } 

    // Liczba spójnych składowych w grafie, metoda brute-force
    public static int CountConnectedComponents<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        var count = 0;
        foreach (var vertex in g.Vertices)
        {
            if (visited.Contains(vertex))
                continue;
            count++;
            foreach (var v in DFS(g, vertex))
                visited.Add(v);
        }
        return count;
    }


    // Liczba spójnych składowych w grafie, wersja zoptymalizowana
    public static int CountConnectedComponentsOptimized<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        var count = 0;
        T vertex;
        while( (vertex = g.Vertices.Except(visited).FirstOrDefault() ) != null)
        {
            if (vertex.Equals(default) )
                break;
            count++;
            foreach (var v in DFS(g, vertex))
                visited.Add(v);
        }
        return count;
    }

    // Sprawdzenie czy graf jest spójny
    public static bool IsConnected<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        return CountConnectedComponents(g) == 1;
    }

    // Lista składowych spójnych w grafie
    public static IEnumerable<IEnumerable<T>> ConnectedComponents<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        var visited = new HashSet<T>();
        foreach (var vertex in g.Vertices)
        {
            if (visited.Contains(vertex))
                continue;
            var component = new List<T>();
            foreach (var v in DFS(g, vertex))
            {
                visited.Add(v);
                component.Add(v);
            }
            yield return component;
        }
    }

/* Do poprawy

    // Punkty artykulacji w grafie algorytm naiwny, brute force
    // referencja: https://en.wikipedia.org/wiki/Biconnected_component
    public static IEnumerable<T> ArticulationPoints<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        foreach (var vertex in g.Vertices)
        {
            var copy = new GraphAdjList<T>(g); // nie zaimplementowano kopiowania/klonowania grafu
            copy.RemoveVertex(vertex);
            if (!copy.IsConnected())
                yield return vertex;
        }
    }

    // Mosty w grafie algorytm naiwny, brute force
    // referencja: https://en.wikipedia.org/wiki/Bridge_(graph_theory)
    public static IEnumerable<(T, T)> Bridges<T>(this IGraphNonWeighted<T> g) where T : IEquatable<T>
    {
        foreach (var (v1, v2) in g.Edges)
        {
            var copy = new GraphAdjList<T>(g); // nie zaimplementowano kopiowania/klonowania grafu
            copy.RemoveEdge(v1, v2);
            if (!copy.IsConnected())
                yield return (v1, v2);
        }
    }

*/
    

} 