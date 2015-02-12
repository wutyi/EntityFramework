using System;
using System.Linq;
using Microsoft.Data.Entity.Internal;
using Microsoft.Data.Entity.Utilities;
using Xunit;

namespace Microsoft.Data.Entity.Tests.Utilities
{
    public class MultigraphTest
    {
        #region Fixture

        private class Vertex
        {
            public int Id { get; set; }

            public override string ToString()
            {
                return Id.ToString();
            }
        }

        private class Edge
        {
            public int Id { get; set; }

            public override string ToString()
            {
                return Id.ToString();
            }
        }

        #endregion

        [Fact]
        public void AddVertex_adds_a_vertex()
        {
            var vertexOne = new Vertex { Id = 1 };
            var vertexTwo = new Vertex { Id = 2 };

            var graph = new Multigraph<Vertex, Edge>();

            graph.AddVertex(vertexOne);
            graph.AddVertex(vertexTwo);

            Assert.Equal(2, graph.Vertices.Count());
            Assert.Equal(2, graph.Vertices.Intersect(new[] { vertexOne, vertexTwo }).Count());
        }

        [Fact]
        public void AddVertices_add_verticies()
        {
            var vertexOne = new Vertex { Id = 1 };
            var vertexTwo = new Vertex { Id = 2 };
            var vertexThree = new Vertex { Id = 3 };

            var graph = new Multigraph<Vertex, Edge>();

            graph.AddVertices(new[] { vertexOne, vertexTwo });
            graph.AddVertices(new[] { vertexTwo, vertexThree });

            Assert.Equal(3, graph.Vertices.Count());
            Assert.Equal(3, graph.Vertices.Intersect(new[] { vertexOne, vertexTwo, vertexThree }).Count());
        }

        [Fact]
        public void AddEdge_adds_an_edge()
        {
            var vertexOne = new Vertex { Id = 1 };
            var vertexTwo = new Vertex { Id = 2 };

            var edgeOne = new Edge { Id = 1 };
            var edgeTwo = new Edge { Id = 2 };

            var graph = new Multigraph<Vertex, Edge>();
            graph.AddVertices(new[] { vertexOne, vertexTwo });
            graph.AddEdge(vertexOne, vertexTwo, edgeOne);
            graph.AddEdge(vertexOne, vertexTwo, edgeTwo);

            Assert.Equal(2, graph.Edges.Count());
            Assert.Equal(2, graph.Edges.Intersect(new[] { edgeOne, edgeTwo }).Count());

            Assert.Equal(0, graph.GetEdges(vertexTwo, vertexOne).Count());
            Assert.Equal(2, graph.GetEdges(vertexOne, vertexTwo).Count());
            Assert.Equal(2, graph.GetEdges(vertexOne, vertexTwo).Intersect(new[] { edgeOne, edgeTwo }).Count());
        }

        [Fact]
        public void AddEdge_throws_on_verticies_not_in_the_graph()
        {
            var vertexOne = new Vertex { Id = 1 };
            var vertexTwo = new Vertex { Id = 2 };

            var edgeOne = new Edge { Id = 1 };

            var graph = new Multigraph<Vertex, Edge>();
            graph.AddVertex(vertexOne);

            Assert.Equal(
                Strings.GraphDoesNotContainVertex(vertexTwo),
                Assert.Throws<InvalidOperationException>(() => graph.AddEdge(vertexOne, vertexTwo, edgeOne)).Message);

            Assert.Equal(
                Strings.GraphDoesNotContainVertex(vertexTwo),
                Assert.Throws<InvalidOperationException>(() => graph.AddEdge(vertexTwo, vertexOne, edgeOne)).Message);
        }

        [Fact]
        public void AddEdges_adds_multiple_edges()
        {
            var vertexOne = new Vertex { Id = 1 };
            var vertexTwo = new Vertex { Id = 2 };

            var edgeOne = new Edge { Id = 1 };
            var edgeTwo = new Edge { Id = 2 };
            var edgeThree = new Edge { Id = 3 };

            var graph = new Multigraph<Vertex, Edge>();
            graph.AddVertices(new[] { vertexOne, vertexTwo });
            graph.AddEdges(vertexOne, vertexTwo, new[] { edgeOne });
            graph.AddEdges(vertexOne, vertexTwo, new[] { edgeTwo, edgeThree });

            Assert.Equal(0, graph.GetEdges(vertexTwo, vertexOne).Count());
            Assert.Equal(3, graph.GetEdges(vertexOne, vertexTwo).Count());
            Assert.Equal(3, graph.GetEdges(vertexOne, vertexTwo).Intersect(new[] { edgeOne, edgeTwo, edgeThree }).Count());
        }

        [Fact]
        public void AddEdge_updates_incomming_and_outgoing_neighbours()
        {
            var vertexOne = new Vertex { Id = 1 };
            var vertexTwo = new Vertex { Id = 2 };
            var vertexThree = new Vertex { Id = 3 };

            var edgeOne = new Edge { Id = 1 };
            var edgeTwo = new Edge { Id = 2 };
            var edgeThree = new Edge { Id = 3 };

            var graph = new Multigraph<Vertex, Edge>();
            graph.AddVertices(new[] { vertexOne, vertexTwo, vertexThree });
            graph.AddEdge(vertexOne, vertexTwo, edgeOne);
            graph.AddEdge(vertexOne, vertexThree, edgeTwo);
            graph.AddEdge(vertexTwo, vertexThree, edgeThree);

            Assert.Equal(2, graph.GetOutgoingNeighbours(vertexOne).Count());
            Assert.Equal(2, graph.GetOutgoingNeighbours(vertexOne).Intersect(new[] { vertexTwo, vertexThree }).Count());

            Assert.Equal(2, graph.GetIncomingNeighbours(vertexThree).Count());
            Assert.Equal(2, graph.GetIncomingNeighbours(vertexThree).Intersect(new[] { vertexOne, vertexTwo }).Count());
        }

        [Fact]
        public void Sort_simple()
        {
            var vertexOne = new Vertex { Id = 1 };
            var vertexTwo = new Vertex { Id = 2 };
            var vertexThree = new Vertex { Id = 3 };

            var edgeOne = new Edge { Id = 1 };
            var edgeTwo = new Edge { Id = 2 };

            var graph = new Multigraph<Vertex, Edge>();
            graph.AddVertices(new[] { vertexOne, vertexTwo, vertexThree });

            // 2 -> 1 -> 3
            graph.AddEdge(vertexTwo, vertexOne, edgeOne);
            graph.AddEdge(vertexOne, vertexThree, edgeTwo);

            Assert.Equal(
                new[] { vertexTwo, vertexOne, vertexThree },
                graph.TopologicalSort().ToArray());
        }

    }
}
