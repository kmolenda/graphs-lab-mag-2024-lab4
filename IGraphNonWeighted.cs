// Interfejs dla grafu nieważonego
// Wierzchołki grafu są reprezentowane przez typ `T`, który musi implementować interfejs `IEquatable<T>`.
// Krawędź grafu jest reprezentowana jako krotka `(T, T)`.

public interface IGraphNonWeighted<T> where T : IEquatable<T>
{
    // Właściwości
    int NoOfVertices { get; } // liczba wierzchołków
    int NoOfEdges { get; } // liczba krawędzi
    bool ContainsVertex(T vertex); // sprawdza, czy wierzchołek jest w grafie
    bool ContainsEdge((T, T) edge); // sprawdza, czy krawędź jest w grafie
    IEnumerable<(T, T)> Edges { get; } // zwraca wszystkie krawędzie grafu
    IEnumerable<T> Vertices { get; } // zwraca wszystkie wierzchołki grafu
    IEnumerable<T> Neighbours(T vertex); // zwraca listę sąsiadów wierzchołka

    // Metody
    void AddVertex(T vertex); // dodaje wierzchołek do grafu
    void RemoveVertex(T vertex); // usuwa wierzchołek z grafu
    void AddEdge((T, T) edge); // dodaje krawędź do grafu
    void RemoveEdge((T, T) edge); // usuwa krawędź z grafu
}
