Test2();

void Test2()
{
    var g = new GraphAdjList<char>();
    g.AddVertices("ABCDEFGH");
    g.AddEdgesAndVertices(new List<(char, char)> {
         ('A', 'B'), ('A', 'C'), ('B', 'C'), ('B', 'D'), ('C', 'F'), ('D', 'F'), ('D', 'E'), ('H', 'I') });
    // Console.WriteLine( g.ToDot() );
    Console.WriteLine( g.CountConnectedComponents() );
    Console.WriteLine( g.CountConnectedComponentsOptimized() );
    foreach (var cc in g.ConnectedComponents())
        Console.WriteLine(string.Join(", ", cc));


}

void Test1()
{
    var g = new GraphAdjList<char>();
    g.AddEdgesAndVertices(new List<(char, char)> { ('A', 'B'), ('A', 'C'), ('B', 'C'), ('B', 'D') });
    // g.AddVertices("ABCDE");
    // g.AddEdge('A', 'B');
    // g.AddEdge('A', 'C');
    // g.AddEdge('B', 'C');
    // g.AddEdge('B', 'D');

    g.Dump();

    foreach (var v in GraphAlgorithms.DFS(g, 'A'))
        Console.WriteLine($"Vertex: {v}");

    Console.WriteLine(string.Join(", ", g.DFS('A') ));

    var l = GraphAlgorithms.DFS(g, 'A').ToList();

    Console.WriteLine( g.ToDot() );
}
