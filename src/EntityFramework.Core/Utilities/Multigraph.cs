using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Microsoft.Data.Entity.Utilities
{
    public class Multigraph<TVertex, TEdge> : Graph<TVertex>
    {
        private readonly HashSet<TVertex> _verticies = new HashSet<TVertex>();
        private readonly HashSet<TEdge> _edges = new HashSet<TEdge>();
        private readonly Dictionary<TVertex, HashSet<TVertex>> _successorMap = new Dictionary<TVertex, HashSet<TVertex>>();

        public IEnumerable<TEdge> Edges
        {
            get { return _edges; }
        }

        public virtual IEnumerable<TEdge> GetIncomingEdges([NotNull] TVertex to)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TEdge> GetOutgoingEdges([NotNull] TVertex from)
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

        public virtual void AddEdge(TVertex from, TVertex to, TEdge edge)
        {
            HashSet<TVertex> successors;
            if (!_successorMap.TryGetValue(from, out successors))
            {
                successors = new HashSet<TVertex>();
                _successorMap.Add(from, successors);
            }
            successors.Add(to);

            _edges.Add(edge);
        }

        public override IEnumerable<TVertex> Vertices
        {
            get { return _verticies; }
        }

        public override IEnumerable<TVertex> GetOutgoingNeighbours([NotNull]TVertex from)
        {
            HashSet<TVertex> successorSet;
            if (_successorMap.TryGetValue(from, out successorSet))
            {
                return successorSet;
            }
            return Enumerable.Empty<TVertex>();
        }

        public override IEnumerable<TVertex> GetIncomingNeighbours([NotNull]TVertex to)
        {
            return _successorMap.Where(kvp => kvp.Value.Contains(to)).Select(kvp => kvp.Key);
        }
    }
}
