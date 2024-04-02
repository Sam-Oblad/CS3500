/// <summary>
/// Author: Sam Oblad
/// Partner: None
/// Date: 1/27/2024
/// Course: CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and Sam Oblad - This work may not be copied for use in Academic Coursework
/// 
/// I, Sam Oblad, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
/// 
/// File Contents
/// This file contains the tests for my DepenencyGraph data structure. Tests without a test category are general tests ensuring that the graph is
/// behaving normally, while error handling tests are meant to test edge cases, or bad actors. 
/// 
/// </summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void simpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void simpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }
        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void emptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }
        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void simpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
            Assert.AreEqual(0, t.GetDependents("x").Count());
            Assert.AreEqual(0, t.GetDependees("y").Count());
        }
        ///<summary>
        ///It should be possible to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void staticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void sizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void enumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }
        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void replaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });
            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));
            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());
            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        /// <summary>
        /// test if the dependencies of a, b, c and d were all created correctly
        /// tests Getdependents and GetDependees for correctness
        /// </summary>
        [TestMethod()]
        public void smallGraphTest()
        {
            HashSet<String> answers = new();
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "d");
            t.AddDependency("d", "d");

            answers.Add("b");
            answers.Add("c");
            IEnumerable<string> responses = t.GetDependents("a");
            Assert.IsTrue(answers.SetEquals(responses));
            answers.Clear();

            answers.Add("d");
            responses = t.GetDependents("b");
            Assert.IsTrue(answers.SetEquals(responses));

            responses = t.GetDependents("d");
            Assert.IsTrue(answers.SetEquals(responses));
            answers.Clear();

            responses = t.GetDependents("c");
            Assert.IsTrue(answers.SetEquals(responses));

            responses = t.GetDependees("a");
            Assert.IsTrue(answers.SetEquals(responses));

            responses = t.GetDependees("b");
            answers.Add("a");
            Assert.IsTrue(answers.SetEquals(responses));

            responses = t.GetDependees("c");
            Assert.IsTrue(answers.SetEquals(responses));
            answers.Clear();

            responses = t.GetDependees("d");
            answers.Add("b");
            answers.Add("d");
            Assert.IsTrue(answers.SetEquals(responses));

        }

        /// <summary>
        /// Tests ReplaceDependents() method, using same graph tested above, 
        /// checks dependent replacements and dependee relationships for correctness
        /// </summary>
        [TestMethod()]

        public void replaceDependentTest()
        {
            HashSet<String> answers = new();
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "d");
            t.AddDependency("d", "d");

            HashSet<string> replacements = new();
            replacements.Add("d");

            t.ReplaceDependents("a", replacements);
            answers.Add("d");
            Assert.IsTrue(answers.SetEquals(t.GetDependents("a")));
            answers.Clear();

            answers.Add("b");
            answers.Add("d");
            answers.Add("a");
            Assert.IsTrue(answers.SetEquals(t.GetDependees("d")));

        }

        /// <summary>
        /// tests replacingDependees() method checks proper dependee replacement 
        /// using same graph above checks distant relationships for correctness 
        /// as well as dependents
        /// </summary>
        [TestMethod()]
        public void replaceDependeeTest()
        {
            HashSet<String> answers = new();
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "d");
            t.AddDependency("d", "d");

            HashSet<string> replacements = new();
            replacements.Add("c");
            answers.Add("c");
            t.ReplaceDependees("b", replacements);
            Assert.IsTrue(answers.SetEquals(t.GetDependees("b")));
            answers.Clear();
            answers.Add("a");
            Assert.IsTrue(answers.SetEquals(t.GetDependees("c")));
            answers.Clear();

            answers.Add("b");
            Assert.IsTrue(answers.SetEquals(t.GetDependents("c")));

        }

        /// <summary>
        /// Tests removeDependency() method
        /// </summary>
        [TestMethod()]

        public void removeDependencyTest()
        {
            HashSet<String> answers = new();
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "d");
            t.AddDependency("d", "d");

            answers.Add("c");
            t.RemoveDependency("a", "b");
            Assert.IsTrue(answers.SetEquals(t.GetDependents("a")));
            answers.Clear();
            Assert.IsTrue(answers.SetEquals(t.GetDependees("b")));
            answers.Add("a");
            Assert.IsTrue(answers.SetEquals(t.GetDependees("c")));
        }

        /// <summary>
        /// Tests hasDependent() method
        /// </summary>
        [TestMethod()]
        public void hasDependentTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "d");
            t.AddDependency("d", "d");
            Assert.IsTrue(t.HasDependents("b"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependents("c"));
            Assert.IsFalse(t.HasDependents("e"));
        }

        /// <summary>
        /// Tests hasDependee() method
        /// </summary>
        [TestMethod()]
        public void hasDependeeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "d");
            t.AddDependency("d", "d");
            Assert.IsTrue(t.HasDependees("c"));
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsFalse(t.HasDependees("e"));
        }

        /// <summary>
        /// tests the this[] method
        /// </summary>
        [TestMethod()]
        public void thisTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "d");
            t.AddDependency("d", "d");
            int responseA = t["a"];
            int responseB = t["b"];
            int responseC = t["c"];
            int responseD = t["d"];
            Assert.AreEqual(0, responseA);
            Assert.AreEqual(1, responseB);
            Assert.AreEqual(1, responseC);
            Assert.AreEqual(2, responseD);

        }

        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void stressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }
            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }
            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }
            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new
                HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new
                HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        /// <summary>
        /// Tests multiple graphs at the same time
        /// </summary>
        [TestMethod()]
        public void multipleGraphTest()
        {
            DependencyGraph a = new DependencyGraph();
            DependencyGraph b = new DependencyGraph();
            DependencyGraph c = new DependencyGraph();

            a.AddDependency("Oil", "Gas");
            a.AddDependency("Gas", "Car");
            a.AddDependency("Car", "People");
            b.AddDependency("x", "y");

            for (int i = 0; i < 200; i++)
            {
                c.AddDependency(i.ToString(), (i + 1).ToString());
            }

            Assert.AreEqual(3, a.Size);
            Assert.AreEqual(1, b.Size);
            Assert.AreEqual(200, c.Size);

            a.RemoveDependency("Gas", "Car");
            b.ReplaceDependents("x", ["z"]);
            c.RemoveDependency("1", "2");

            Assert.AreEqual(2, a.Size);
            Assert.AreEqual(1, b.Size);
            Assert.AreEqual(199, c.Size);

            a.AddDependency("Electricity", "Car");
            b.RemoveDependency("x", "z");
            c.ReplaceDependents("2", ["3", "4", "5"]);

            Assert.AreEqual(3, a.Size);
            Assert.AreEqual(0, b.Size);
            Assert.AreEqual(201, c.Size);



        }

        /// <summary>
        /// confirms that when calling this[] on a node that doesn't exist, 0 dependees is returned
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]
        public void thisNoNodeTest()
        {
            DependencyGraph t = new DependencyGraph();
            int response = t["a"];
            Assert.AreEqual(0, response);
        }

        /// <summary>
        /// confirms returning false when a node has no dependees
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]

        public void hasDependeesNoNodeTest()
        {
            DependencyGraph t = new DependencyGraph();
            bool response = t.HasDependees("a");
            Assert.IsFalse(response);
        }

        /// <summary>
        /// confirms returning true when a node has dependees
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]
        public void hasDependeesNodeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("b", "a");
            bool response = t.HasDependees("a");
            Assert.IsTrue(response);

        }

        /// <summary>
        /// confirms returning false when calling hasdependents() on a non-existent node 
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]

        public void hasDependentsNoNodeTest()
        {
            DependencyGraph t = new DependencyGraph();
            bool response = t.HasDependents("a");
            Assert.IsFalse(response);
        }

        /// <summary>
        /// confirms returning true when a node has dependees
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]
        public void hasDependentsNodeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            bool response = t.HasDependents("a");
            Assert.IsTrue(response);
        }


        /// <summary>
        /// Tests if getdependent returns an empty IEnumerable when node doesn't exist
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]
        public void getDependentNoNodeTest()
        {
            DependencyGraph t = new DependencyGraph();
            var response = t.GetDependents("a");
            Assert.AreEqual(0, response.Count());

        }


        /// <summary>
        /// Tests if getdependee returns an empty IEnumerable when node doesn't exist
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]
        public void getDependeeNoNodeTest()
        {
            DependencyGraph t = new DependencyGraph();
            var response = t.GetDependees("a");
            Assert.AreEqual(0, response.Count());

        }


        /// <summary>
        /// Tests removing dependencies that don't exist
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]
        public void removeNonExistingDependencyTest()
        {
            DependencyGraph t = new DependencyGraph();
            HashSet<string> answers = new();
            t.AddDependency("a", "b");
            t.AddDependency("b", "c");
            t.AddDependency("d", "e");

            answers.Add("b");
            t.RemoveDependency("a", "d");
            Assert.IsTrue(answers.SetEquals(t.GetDependents("a")));
            answers.Clear();

            answers.Add("a");
            Assert.IsTrue(answers.SetEquals(t.GetDependees("b")));
            answers.Clear();

            t.RemoveDependency("a", null);
            answers.Add("b");
            t.RemoveDependency("a", "d");
            Assert.IsTrue(answers.SetEquals(t.GetDependents("a")));
            answers.Clear();

            answers.Add("a");
            Assert.IsTrue(answers.SetEquals(t.GetDependees("b")));
            answers.Clear();

        }

        /// <summary>
        /// Tests replacing dependencies that don't exist
        /// First, test replacing dependents on a node without any
        /// Second, test replacing dependees on a node without any
        /// verify relationships of corresponding dependees and dependents are correct
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]
        public void replaceNonExistingDependencyTest()
        {
            DependencyGraph t = new DependencyGraph();
            HashSet<string> replacements = new();
            HashSet<string> answers = new();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");

            replacements.Add("e");
            replacements.Add("f");
            replacements.Add("g");
            answers.Add("e");
            answers.Add("f");
            answers.Add("g");

            //dependent replacement
            t.ReplaceDependents("b", replacements);
            Assert.IsTrue(answers.SetEquals(t.GetDependents("b")));
            answers.Clear();
            replacements.Clear();
            answers.Add("a");
            Assert.IsTrue(answers.SetEquals(t.GetDependees("b")));
            answers.Clear();
            answers.Add("b");
            Assert.IsTrue(answers.SetEquals(t.GetDependees("e")));
            Assert.IsTrue(answers.SetEquals(t.GetDependees("f")));
            Assert.IsTrue(answers.SetEquals(t.GetDependees("g")));
            answers.Clear();


            replacements.Add("x");
            replacements.Add("y");
            answers.Add("x");
            answers.Add("y");

            //dependee replacement
            t.ReplaceDependees("a", replacements);
            Assert.IsTrue(answers.SetEquals(t.GetDependees("a")));
            answers.Clear();
            answers.Add("a");
            Assert.IsTrue(answers.SetEquals(t.GetDependents("x")));
            Assert.IsTrue(answers.SetEquals(t.GetDependents("y")));

        }


        /// <summary>
        /// Ensures that nodes are not duplicating in the graph
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]

        public void addMultipleSameNodesToGraphTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "b");
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        /// Ensures that dependencies are not duplicating in their sets
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]
        public void addMultipleDifferentDependenciesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.ReplaceDependees("a", ["b"]);
            Assert.AreEqual(1, t.GetDependents("a").Count());
        }

        /// <summary>
        /// Tests removing and then replacing on the same node and replacing then removing on the same node
        /// </summary>
        [TestMethod()]
        [TestCategory("Error Handling")]
        public void removeAndReplaceOnSameNodeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("c", "d");
            t.AddDependency("x", "y");
            t.RemoveDependency("a", "b");

            Assert.AreEqual(2, t.Size);
            Assert.AreEqual(0, t.GetDependents("a").Count());
            Assert.AreEqual(0, t.GetDependees("c").Count());
            t.ReplaceDependents("a", ["d"]);
            Assert.AreEqual(3, t.Size);
            Assert.AreEqual(2, t.GetDependees("d").Count());
            Assert.AreEqual(1, t.GetDependents("a").Count());

            t.ReplaceDependents("c", ["e", "f"]);
            Assert.AreEqual(4, t.Size);
            Assert.AreEqual(2, t.GetDependents("c").Count());
            t.RemoveDependency("c", "e");
            Assert.AreEqual(3, t.Size);
            Assert.AreEqual(1, t.GetDependents("c").Count());

            t.RemoveDependency("x", "y");
            Assert.AreEqual(2, t.Size);
            t.ReplaceDependents("x", ["y"]);
            Assert.AreEqual(3, t.Size);
        }
    }
}