using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace onTrack.UnitTests
{
    [TestClass]
    public class UnitTest2
    {
        public class CharacterMutater
        {
            Random rand = new Random();
            private List<int> generatedIndices = new List<int>();

            public void AddGeneratedIndex(int index, string objective)
            {
                generatedIndices.Add(index);
                if (generatedIndices.Count == Max(objective) + 2)
                    generatedIndices.Clear();
            }

            public enum Mutation
            {
                add,
                sub
            }

            public Mutation RandomMutation()
            {
                int randomInt = rand.Next(0, 2);
                return randomInt == 0 ? Mutation.add : Mutation.sub;
            }

            public List<int> NonGeneratedIndices(string objective, bool outsideRange)
            {
                List<int> indices = new List<int>();

                for (int i = 0; i < Max(objective); i++)
                    if (!generatedIndices.Contains(i))
                        indices.Add(i);

                if (outsideRange)
                {
                    if (!generatedIndices.Contains(-1))
                    {
                        indices.Add(-1);
                        AddGeneratedIndex(-1, objective);
                    }
                    if (!generatedIndices.Contains(Max(objective)))
                    {
                        indices.Add(objective.Length);
                        AddGeneratedIndex(Max(objective), objective);
                    }
                }

                return indices;
            }

            public int Max(string objective)
            {
                return 10 < objective.Length ? 10 : objective.Length;
            }

            public int RandomIndex(string objective, bool outsideRange = true)
            {
                List<int> nonGeneratedIndices = NonGeneratedIndices(objective, outsideRange);
                int randomInt = rand.Next(0, nonGeneratedIndices.Count);
                int index = nonGeneratedIndices[randomInt];
                AddGeneratedIndex(index, objective);
                return index;
            }

            public string AddChar(string objective)
            {
                int randomIndex = RandomIndex(objective);
                if (randomIndex < 0) return "." + objective;
                else if (randomIndex > Max(objective)) return objective + ".";
                return objective.Insert(randomIndex, ".");
            }

            public string SubChar(string objective)
            {
                int randomIndex = RandomIndex(objective, false);
                return objective.Remove(randomIndex, 1);
            }

            public string GenerateCharacterMutation(string objective)
            {
                return RandomMutation() == Mutation.add ? AddChar(objective) : SubChar(objective);
            }

            public string[] GenerateCharacterMutations(string objective, int count)
            {
                string[] results = new string[count];
                for (var i = 0; i < count; i++)
                    results[i] = GenerateCharacterMutation(objective);
                generatedIndices.Clear();
                return results;
            }

            public string[] GenerateMutationsOfObjective(string objective, int count)
            {
                int randPlace = rand.Next(count);
                string[] mutations = GenerateCharacterMutations(objective, count - 1);
                List<string> results = mutations.ToList();
                results.Insert(randPlace, objective);
                return results.ToArray();
            }
        }

        CharacterMutater characterMutater = new CharacterMutater();

        [TestMethod]
        public void Test_NonGeneratedIndices()
        {
            CharacterMutater characterMutater2 = new CharacterMutater();
            List<int> indices = characterMutater2.NonGeneratedIndices("Work", false);
        }

        [TestMethod]
        public void Test_AddLetter()
        {
            string objectiveWithAddedLetter = characterMutater.AddChar("Work");
            Assert.IsTrue(objectiveWithAddedLetter.Contains("."));
        }

        [TestMethod]
        public void Test_SubLetter()
        {
            string objectiveWithAddedLetter = characterMutater.SubChar("Work");
            Assert.IsTrue(objectiveWithAddedLetter.Length == 3);
        }

        [TestMethod]
        public void Test_RandomIndex()
        {
            for (int i = 0; i < 10; i++)
            {
                var index = characterMutater.RandomIndex("Work", true);
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Test_GenerateCharacterMutations()
        {
            var mutations = characterMutater.GenerateCharacterMutations("Work", 4);
            Assert.AreEqual(4, mutations.Length);
        }

        [TestMethod]
        public void Test_GenerateMutationsOfObjective()
        {
            var mutations = characterMutater.GenerateMutationsOfObjective("Work", 4);
            Assert.AreEqual(4, mutations.Length);
        }
    }
}