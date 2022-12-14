using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;

namespace onTrack.UnitTests
{
    [TestClass]
    public class UnitTest2
    {
        public class CharacterMutater
        {
            Random rand = new Random();
            public List<int> generatedIndices = new List<int>();

            public enum Mutation
            {
                add,
                sub
            }

            public Mutation RandomMutation()
            {
                int randomInt = rand.Next(0, 1);
                return randomInt == 0 ? Mutation.add : Mutation.sub;
            }

            public List<int> NonGeneratedIndices(string objective, bool outsideRange)
            {
                List<int> indices = new List<int>();

                for (int i = 0; i < objective.Length; i++)
                    if (!generatedIndices.Contains(i))
                        indices.Add(i);

                if (outsideRange)
                    if (!generatedIndices.Contains(-1))
                    {
                        indices.Add(-1);
                        generatedIndices.Add(-1);
                    }
                    if (!generatedIndices.Contains(objective.Length))
                    {
                        indices.Add(objective.Length);
                        generatedIndices.Add(objective.Length);
                    }

                return indices;
            }

            public int RandomIndex(string objective, bool outsideRange = true)
            {
                List<int> nonGeneratedIndices = NonGeneratedIndices(objective, outsideRange);
                int randomInt = rand.Next(0, nonGeneratedIndices.Count - 1);
                int index = nonGeneratedIndices[randomInt];
                generatedIndices.Add(index);
                return index;
            }

            public string AddChar(string objective)
            {
                int randomIndex = RandomIndex(objective);
                if (randomIndex < 0) return "." + objective;
                else if (randomIndex > 0) return objective + ".";
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
                return results;
            }
        }

        CharacterMutater characterMutater = new CharacterMutater();

        [TestMethod]
        public void Test_NonGeneratedIndices()
        {
            CharacterMutater characterMutater2 = new CharacterMutater();
            characterMutater2.generatedIndices.Add(-1);
            List<int> indices = characterMutater2.NonGeneratedIndices("Work", true);
            Assert.IsFalse(indices.Contains(-1));
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
        public void Test_GenerateCharacterMutations()
        {
            var mutations = characterMutater.GenerateCharacterMutations("Work", 4);
            Assert.AreEqual(4, mutations.Length);
        }
    }
}
