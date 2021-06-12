using System.Collections.Generic;
using System.Reflection;
using AdvancedConsole.Commands.Modules;

namespace AdvancedConsole.Reading.Completing
{
    public class MethodsTreeTabCompleter : ITabCompleter
    {
        private ModulesTree Tree { get; set; }

        public MethodsTreeTabCompleter(ModulesTree tree)
        {
            Tree = tree;
        }

        public IEnumerable<string> GetCompletion(string currentInput, int cursorIndex)
        {
            string[] words = currentInput.Split();
            if (currentInput.Length == 0 || words.Length == 0)
            {
                foreach (ITreeNode treeNode in Tree.Nodes)  
                {
                    yield return treeNode.Name;
                }
                yield break;
            }
            if (currentInput.Length != 0 && words.Length == 1)
            {
                foreach (ITreeNode treeNode in Tree.Nodes)
                {
                    if (treeNode.Name.StartsWith(words[^1]))
                        yield return treeNode.Name[words[^1].Length..];
                    foreach (string alias in treeNode.Aliases)
                    {
                        if (alias.StartsWith(words[^1]))
                            yield return alias[words[^1].Length..];
                    }
                }
            }
            else
            {
                foreach (ITreeNode treeNode in Tree.GetNodes(words[..^1]))
                {
                    foreach (ITreeNode treeNodeSubNode in treeNode.SubNodes)
                    {
                        if (treeNodeSubNode.Name.StartsWith(words[^1])) 
                            yield return treeNodeSubNode.Name[words[^1].Length..];
                        foreach (string alias in treeNodeSubNode.Aliases)
                        {
                            if (alias.StartsWith(words[^1]))
                                yield return alias[words[^1].Length..];
                        }
                    }
                }
            }

            foreach (string s in GetExplicitArgsCompletion(words, cursorIndex))
            {
                yield return s;
            }
        }

        private IEnumerable<string> GetExplicitArgsCompletion(string[] words, int cursorIndex)
        {
            List<string> result = new ();
            Tree.Walk(words, (path, node) =>
            {
                if(node is not Command commandNode) return;
                foreach (ParameterInfo inputParameter in commandNode.InputParameters)
                {
                    foreach (string pathPart in path)
                    {
                        string completion = inputParameter.Name + "=";
                        if(pathPart.StartsWith(completion)) break;
                        if(inputParameter.Name.StartsWith(pathPart)) result.Add(inputParameter.Name[pathPart.Length..] + "=");
                    }
                }
            });
            return result;
        }
    }
}