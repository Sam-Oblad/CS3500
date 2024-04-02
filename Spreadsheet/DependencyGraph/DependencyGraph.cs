// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SpreadsheetUtilities
{
    /// <summary>
    /// <Author> Araum Karimi </Author>
    /// <Version> January 27, 2024 </Version>
    /// 
    /// DependencyGraph implementation that tracks dependencies between strings. Uses 2 dictionaries to map dependencies, 
    /// one to access dependents using dependees as the key, and one to access dependees using dependents as the key.
    /// </summary>
    public class DependencyGraph
    {
        Dictionary<String, HashSet<String>> dependeeAsKey, dependentAsKey;
        int size;

        /// <summary>
        /// Creates an new DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            this.size = 0;
            this.dependeeAsKey = new();
            this.dependentAsKey = new();
        }

        /// <summary>
        /// Returns the number of dependencies in this DependencyGraph
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        ///  Returns the number of dependees of s.
        ///  If s has no dependees, returns 0
        /// </summary>
        /// <param name="s"> Potential dependent </param>
        /// <returns> Number of dependees of s</returns>
        public int this[string s]
        {
            get 
            {
                if (!HasDependees(s))
                {
                    return 0;
                }
                return dependentAsKey[s].Count;
            }
        }

        /// <summary>
        /// Determines if string s has dependents.
        /// </summary>
        /// <param name="s"> Potential dependee </param>
        /// <returns> True if s has dependents, False if it does not </returns>
        public bool HasDependents(string s)
        {
            if (!dependeeAsKey.ContainsKey(s))
            {
                return false;
            }
            return dependeeAsKey[s].Count > 0;
        }

        /// <summary>
        /// Determines if string s has dependees.
        /// </summary>
        /// <param name="s"> Potential dependent </param>
        /// <returns> True if s has dependees, False if it does not </returns>
        public bool HasDependees(string s)
        {
            if(!dependentAsKey.ContainsKey(s))
            { 
                return false;
            }
            return dependentAsKey[s].Count > 0;
        }

        /// <summary>
        /// Returns the dependents of a dependee s in the form of a list.
        /// If s has no dependents, returns an empty list.
        /// </summary>
        /// <param name="s"> Dependee </param>
        /// <returns> List of dependents of s </returns>
        public IEnumerable<string> GetDependents(string s)
        {
            if (HasDependents(s))
            {
                return dependeeAsKey[s].ToList();
            }
            return new List<string>();
        }

        /// <summary>
        /// Returns the dependees of a dependent s in the form of a list.
        /// If s has no dependents, returns an empty list.
        /// </summary>
        /// <param name="s"> Dependent </param>
        /// <returns> List of dependees of s </returns>
        public IEnumerable<string> GetDependees(string s)
        {
            if (HasDependees(s))
            {
                return dependentAsKey[s].ToList();
            }
            return new List<string>();
        }

        /// <summary>
        /// Adds a new dependency to this graph where t is a dependent of s, and s is a dependee of t.
        /// </summary>
        /// <param name="s"> Dependee </param>
        /// <param name="t"> Dependent </param> 
        public void AddDependency(string s, string t)
        {
            if(!HasDependents(s))
            {
                dependeeAsKey.TryAdd(s, new HashSet<string>());
            }
            if(!HasDependees(t))
            {
                dependentAsKey.TryAdd(t, new HashSet<string>());
            }
            if (!dependeeAsKey[s].Contains(t))
            {
                dependeeAsKey[s].Add(t);
                dependentAsKey[t].Add(s);
                size++;
            }
        }

        /// <summary>
        /// Removes a dependency of this graph where t is a dependent of s, and s is a dependee of t, if this dependency exists.
        /// If the dependency does not exist, this method does nothing. 
        /// </summary>
        /// <param name="s"> Dependee </param>
        /// <param name="t"> Dependent </param>
        public void RemoveDependency(string s, string t)
        {
            if (HasDependents(s))
            {
                if (dependeeAsKey[s].Contains(t))
                {
                    dependeeAsKey[s].Remove(t);
                    dependentAsKey[t].Remove(s);
                    size--;
                }
            }
        }

        /// <summary>
        /// Removes all current dependents of a dependee string, and replaces them with a new set of dependents passed into 
        /// the parameter.
        /// </summary>
        /// <param name="s"> Dependee </param>
        /// <param name="newDependents"> IEnumerable of new dependents to replace with </param>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (HasDependents(s))
            {
                foreach (var dependent in dependeeAsKey[s])
                {
                    RemoveDependency(s, dependent);
                }
            }
            foreach (var dependent in newDependents)
            {
                AddDependency(s, dependent);
            }
        }

        /// <summary>
        /// Removes all current dependees of a dependent string, and replaces them with a new set of dependees passed into 
        /// the parameter.
        /// </summary>
        /// <param name="s"> Dependent </param>
        /// <param name="newDependees"> IEnumerable of new dependees to replace with </param>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (HasDependees(s))
            {
                foreach (var dependee in dependentAsKey[s])
                {
                    RemoveDependency(dependee, s);
                }
            }
            foreach (var dependee in newDependees)
            {
                AddDependency(dependee, s);
            }
        }
    }
}
