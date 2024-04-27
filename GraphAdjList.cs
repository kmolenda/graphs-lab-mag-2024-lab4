// Implementacja grafu nieskierowanego, prostego (bez pętli własnych oraz multikrawędzi), 
// nieważonego, etykietowanego za pomocą listy sąsiadów w C# 12.
// Klasa `GraphAdjList`:
// * zapewnia modyfikowalność struktury grafu (tj. po zainicjowaniu można dodawać/usuwać wierzchołki, można edytować - dodawać, usuwać - krawędzie)
// * dba o minimalizację ilości kodu, dostarczenie wyłącznie niezbędnych metod operowania na grafie
//   > ta implementacja stanowi bazę do ewentualnej rozbudowy o dodatkowe funkcjonalności oraz modyfikacji prowadzących do innego wariantu grafu
// * dostarcza podstawowe metody _poruszania się_ po grafie: przeglądanie w głąb (DFS) oraz wszerz (BFS)
// Wierzchołek grafu jest reprezentowany przez typ `T`, który musi implementować interfejs `IEquatable<T>`.
// Krawędź grafu jest reprezentowana jako krotka `(T, T)`.
// Wewnętrznie:
// * lista sąsiedztwa jest reprezentowana jako słownik, w którym kluczem jest wierzchołek, a wartością zbiór sąsiadów
// * krawędzie są przechowywane dodatkowo w zbiorze krotek `(T, T)` (cache, przyspiesza operacje na krawędziach)

using System.Collections.ObjectModel;

public class GraphAdjList<T> : IGraphNonWeighted<T> where T : IEquatable<T>
{
    // fundament: słownik, w którym kluczem jest wierzchołek, a wartością zbiór sąsiadów
    private readonly Dictionary<T, HashSet<T>> adjList = new();
    // pomocniczo: zbiór krawędzi grafu (cache, przyspiesza operacje na krawędziach)
    private readonly HashSet<(T, T)> edges = [];
    
    // właściwości i metody możliwe do zaimplementowania
    // wyłącznie na podstawie wiedzy o wewnętrznej strukturze grafu
    public int NoOfVertices => adjList.Count; // liczba wierzchołków
    public int NoOfEdges => edges.Count; // liczba krawędzi
    public bool ContainsVertex(T vertex) => adjList.ContainsKey(vertex); // sprawdza, czy wierzchołek jest w grafie
    public bool ContainsEdge(T v1, T v2) => edges.Contains((v1, v2)) || edges.Contains((v2,v1)); // sprawdza, czy krawędź jest w grafie
    public bool ContainsEdge((T, T) edge) => ContainsEdge(edge.Item1, edge.Item2); // sprawdza, czy krawędź jest w grafie
    public IEnumerable<(T, T)> Edges => edges;
    public IEnumerable<T> Vertices => adjList.Keys; // zwraca wszystkie wierzchołki grafu
    public IEnumerable<T> Neighbours(T vertex) => adjList[vertex]; // zwraca listę sąsiadów wierzchołka
    
    public IEnumerable<T> this[T vertex] => Neighbours(vertex); // indexer do dostępu do sąsiadów wierzchołka
    public void Clear() => adjList.Clear(); // usuwanie wszystkich wierzchołków i krawędzi


    // konstruktor klasy
    public GraphAdjList() { }

#region Dodawanie wierzchołków

    // dodawanie jednego wierzchołka
    public void AddVertex(T vertex)
    {
        if( !ContainsVertex(vertex) )
            adjList.Add(vertex, new HashSet<T>());
    } 

    // dodawanie wielu wierzchołków z sekwencji IEnumerable<T>
    public void AddVertices(IEnumerable<T> vertices)
    {
        foreach (var vertex in vertices)
            AddVertex(vertex);
    }
#endregion

#region Dodawanie krawędzi
    // dodawanie pojedynczej krawędzi, o ile wierzchołki istnieją
    // a krawędź nie istnieje
    public void AddEdge(T v1, T v2)
    {
        if (!ContainsVertex(v1) || !ContainsVertex(v2))
            throw new ArgumentException("Vertex not found in graph");
        if (v1.Equals(v2))
            throw new ArgumentException("Self-loops are not allowed in simple graph");
        if (ContainsEdge(v1, v2))
            return;
        adjList[v1].Add(v2);
        adjList[v2].Add(v1);
        edges.Add((v1, v2)); // dodanie krawędzi do cache
    }

    // przeciążenie metody AddEdge dla krawędzi zapisanej jako ValueTuple
    public void AddEdge((T, T) edge) => AddEdge(edge.Item1, edge.Item2);

    // dodanie pojedynczej krawędzi i wierzchołków, jeśli nie istnieją
    public void AddEdgeAndVertices(T v1, T v2)
    {
        AddVertex(v1);
        AddVertex(v2);
        AddEdge(v1, v2);
    }

    // przeciążenie metody AddEdgeAndVertices dla krawędzi zapisanej jako ValueTuple
    public void AddEdgeAndVertices((T, T) edge) => AddEdgeAndVertices(edge.Item1, edge.Item2);

    // dodanie wielu krawędzi z sekwencji IEnumerable<(T, T)> i wielu wierzchołków, jeśli nie istnieją
    public void AddEdgesAndVertices(IEnumerable<(T, T)> edges)
    {
        foreach (var (v1, v2) in edges)
            AddEdgeAndVertices(v1, v2);
    }
#endregion

#region Usuwanie wierzchołków i krawędzi
    // usuwanie jednej krawędzi, o ile istnieje
    public void RemoveEdge(T v1, T v2)
    {
        if (!ContainsVertex(v1) || !ContainsVertex(v2))
            throw new ArgumentException("Vertex not found in graph");
        if (!ContainsEdge(v1, v2))
            return;
        adjList[v1].Remove(v2);
        adjList[v2].Remove(v1);
        edges.Remove((v1, v2)); // usuwanie krawędzi z cache
    }

    // przeciążenie metody RemoveEdge dla krawędzi zapisanej jako ValueTuple
    public void RemoveEdge((T, T) edge) => RemoveEdge(edge.Item1, edge.Item2);

    // usuwanie wielu krawędzi z sekwencji IEnumerable<(T, T)>
    public void RemoveEdges(IEnumerable<(T, T)> edges)
    {
        foreach (var (v1, v2) in edges)
            RemoveEdge(v1, v2);
    }

    // usuwanie jednego wierzchołka i wszystkich krawędzi z nim związanych
    public void RemoveVertex(T vertex)
    {
        if (!ContainsVertex(vertex))
            return;
        foreach (var neighbour in Neighbours(vertex))
            RemoveEdge(vertex, neighbour);
        adjList.Remove(vertex);
    }

    // usuwanie wielu wierzchołków i krawędzi z nimi powiązanych z sekwencji IEnumerable<T>
    public void RemoveVertices(IEnumerable<T> vertices)
    {
        foreach (var vertex in vertices)
            RemoveVertex(vertex);
    }
#endregion

    // generuje graf w postaci tekstowej
    public override string ToString()
    {
        var sb = new System.Text.StringBuilder();
        foreach (var vertex in Vertices)
        {
            sb.Append($"{vertex}: ");
            sb.Append(string.Join(", ", Neighbours(vertex)));
            sb.AppendLine();
        }
        return sb.ToString();
    }

#if DEBUG
    public void Dump()
    {
        Console.WriteLine($"Vertices: {string.Join(", ", Vertices)}");
        Console.WriteLine($"Edges: {string.Join(", ", Edges)}");
        Console.WriteLine(this);
    }
#endif
}
