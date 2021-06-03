using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AdvancedConsole.Commands.Modules
{
    public interface ITreeNode
    {
        string Name { get; }
        IReadOnlyCollection<string> Aliases { get; }
        IEnumerable<ITreeNode> SubNodes { get; }
    }

    public static class TreeNodeExtensions
    {
        public static IEnumerable<ITreeNode> GetNodes(this ITreeNode node, ArraySegment<string> path)
        {
            if (path.Count == 0) yield return node;
            else if (path.Count == 1)
            {
                foreach (ITreeNode subNode in node.SubNodes)
                {
                    if(subNode.Name != path[0]) continue;
                    yield return subNode;
                }
            }
            else
            {
                foreach (ITreeNode subNode in node.SubNodes)
                {
                    if(subNode.Name != path[0]) continue;
                    foreach (ITreeNode treeNode in subNode.GetNodes(path[1..]))
                    {
                        yield return treeNode;
                    }
                }   
            }
        }
    }
}