using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        private class Edge
        {
            public int Id { get; set; }
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
        public void AddVertex_throws_on_null_vertex()
        {
            Vertex vertex = null;

            var graph = new Multigraph<Vertex, Edge>();

            Assert.Equal(
                "vertex",
                Assert.Throws<ArgumentNullException>(() => graph.AddVertex(vertex)).ParamName);
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
        public void AddVerices_throws_on_null_vertices()
        {
            IEnumerable<Vertex> vertices = null;

            var graph = new Multigraph<Vertex, Edge>();

            Assert.Equal(
                "verticies",
                Assert.Throws<ArgumentNullException>(() => graph.AddVertices(vertices)).ParamName);
        }

        [Fact]
        public void AddEdge_adds_an_edge()
        {
            var vertexOne = new Vertex { Id = 1 };
            var vertexTwo = new Vertex { Id = 2 };
            var vertexThree = new Vertex { Id = 3 };

            var edgeOne = new Edge { Id = 1 };
            var edgeTwo = new Edge { Id = 2 };
            var edgeThree = new Edge { Id = 3 };

            var graph = new Multigraph<Vertex, Edge>();
            graph.AddEdge(vertexOne, vertexTwo, edgeOne);
            graph.AddEdge(vertexOne, vertexThree, edgeTwo);
            graph.AddEdge(vertexTwo, vertexThree, edgeThree);

            Assert.Equal(3, graph.Edges.Count());
            Assert.Equal(3, graph.Edges.Intersect(new[] { edgeOne, edgeTwo, edgeThree }).Count());

            Assert.Equal(2, graph.GetOutgoingNeighbours(vertexOne).Count());
            Assert.Equal(2, graph.GetOutgoingNeighbours(vertexOne).Intersect(new[] { vertexTwo, vertexThree }).Count());

            Assert.Equal(2, graph.GetIncomingNeighbours(vertexThree).Count());
            Assert.Equal(2, graph.GetIncomingNeighbours(vertexThree).Intersect(new[] { vertexOne, vertexTwo }).Count());
        }
    }
}
