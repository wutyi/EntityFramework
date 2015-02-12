using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.Data.Entity.Internal;

namespace Microsoft.Data.Entity.Utilities
{
    public class Multigraph<TVertex, TEdge> : Graph<TVertex>
    {
        private readonly HashSet<TVertex> _verticies = new HashSet<TVertex>();
        private readonly HashSet<TEdge> _edges = new HashSet<TEdge>();
        private readonly Dictionary<TVertex, Dictionary<TVertex, List<TEdge>>> _successorMap = new Dictionary<TVertex, Dictionary<TVertex, List<TEdge>>>();

        public IEnumerable<TEdge> Edges
        {
            get { return _edges; }
        }

        public virtual IEnumerable<TEdge> GetEdges([NotNull] TVertex from, [NotNull] TVertex to)
        {
            Dictionary<TVertex, List<TEdge>> successorSet;
            if (_successorMap.TryGetValue(from, out successorSet))
            {
                List<TEdge> edgeList;
                if (successorSet.TryGetValue(to, out edgeList))
                {
                    return edgeList;
                }
            }
            return Enumerable.Empty<TEdge>();
        }

        public virtual IEnumerable<TEdge> GetIncomingEdges([NotNull] TVertex to)
        {
            throw new NotImplementedException();
        }

        public virtual void AddVertex([NotNull] TVertex vertex)
        {
            Check.NotNull(vertex, nameof(vertex));

            _verticies.Add(vertex);
        }

        public virtual void AddVertices([NotNull] IEnumerable<TVertex> verticies)
        {
            Check.NotNull(verticies, nameof(verticies));

            _verticies.UnionWith(verticies);
        }

        public virtual void AddEdge([NotNull] TVertex from, [NotNull] TVertex to, [NotNull] TEdge edge)
        {
            AddEdges(from, to, new[] { edge });
        }

        public virtual void AddEdges([NotNull] TVertex from, [NotNull] TVertex to, [NotNull] IEnumerable<TEdge> edges)
        {
            Check.NotNull(from, nameof(from));
            Check.NotNull(to, nameof(to));
            Check.NotNull(edges, nameof(edges));

            if (!_verticies.Contains(from))
            {
                throw new InvalidOperationException(Strings.GraphDoesNotContainVertex(from));
            }

            if (!_verticies.Contains(to))
            {
                throw new InvalidOperationException(Strings.GraphDoesNotContainVertex(to));
            }

            Dictionary<TVertex, List<TEdge>> successorSet;
            if (!_successorMap.TryGetValue(from, out successorSet))
            {
                successorSet = new Dictionary<TVertex, List<TEdge>>();
                _successorMap.Add(from, successorSet);
            }

            List<TEdge> edgeList;
            if (!successorSet.TryGetValue(to, out edgeList))
            {
                edgeList = new List<TEdge>();
                successorSet.Add(to, edgeList);
            }

            edgeList.AddRange(edges);
            _edges.UnionWith(edges);
        }

        public IEnumerable<TVertex> TopologicalSort()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TVertex> Vertices
        {
            get { return _verticies; }
        }

        public override IEnumerable<TVertex> GetOutgoingNeighbours([NotNull]TVertex from)
        {
            Dictionary<TVertex, List<TEdge>> successorSet;
            if (_successorMap.TryGetValue(from, out successorSet))
            {
                return successorSet.Keys;
            }
            return Enumerable.Empty<TVertex>();
        }

        public override IEnumerable<TVertex> GetIncomingNeighbours([NotNull]TVertex to)
        {
            return _successorMap.Where(kvp => kvp.Value.ContainsKey(to)).Select(kvp => kvp.Key);
        }
    }
}
