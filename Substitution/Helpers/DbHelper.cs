﻿using System;
using System.Linq;
using LiteDB;
using Substitution.Classes;

namespace Substitution.Helpers
{
    internal static class DbHelper
    {
        private const string DbName = "Substitution.db";

        private const string CollectionWords = "words";
        private const string CollectionIgnore = "ignore";

        public static Word[] WordsGet()
        {
            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionWords)) return null;

                var collection = db.GetCollection<Word>(CollectionWords);
                return collection.FindAll().OrderBy(i => i.Value).ToArray();
            }
        }

        public static void WordAdd(string word)
        {
            if (string.IsNullOrEmpty(word.Trim())) return;

            var objWord = new Word
            {
                Value = word.Trim()
            };
            
            using (var db = new LiteDatabase(DbName))
            {
                var collection = db.GetCollection<Word>(CollectionWords);
                var existing = collection.Find(i => i.Value.Equals(word, StringComparison.CurrentCultureIgnoreCase));
                if (existing.Any()) return;

                collection.Insert(objWord);
            }
        }

        public static void WordDelete(Word word)
        {
            if (word == null) return;

            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionWords)) return;

                var collection = db.GetCollection<Word>(CollectionWords);
                collection.Delete(word.Id);
            }
        }

        public static Word WordRandom()
        {
            var words = WordsGet();

            if (words == null) return null;
            if (words.Length == 0) return null;

            var rnd = new Random();
            var index = rnd.Next(words.Length);

            return words[index];
        }



        public static User[] IgnoreGet()
        {
            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionIgnore)) return null;

                var collection = db.GetCollection<User>(CollectionIgnore);
                return collection.FindAll().OrderBy(i => i.Username).ToArray();
            }
        }

        public static void IgnoreAdd(string username)
        {
            var fixedLower = username.Trim().ToLower();
            if (string.IsNullOrEmpty(fixedLower)) return;

            var objWord = new User
            {
                Username = fixedLower
            };

            using (var db = new LiteDatabase(DbName))
            {
                var collection = db.GetCollection<User>(CollectionIgnore);
                var existing = collection.Find(i => i.Username.Equals(fixedLower, StringComparison.CurrentCultureIgnoreCase));
                if (existing.Any()) return;

                collection.Insert(objWord);
            }
        }

        public static void IgnoreDelete(string username)
        {
            if (username == null) return;
            var fixedLower = username.ToLower().Trim();

            using (var db = new LiteDatabase(DbName))
            {
                if (!db.CollectionExists(CollectionIgnore)) return;

                var collection = db.GetCollection<User>(CollectionIgnore);

                var existing = collection.Find(i => i.Username == fixedLower).ToArray();
                if (existing.Length == 0) return;
                var currentUser = existing.Single();

                collection.Delete(currentUser.Id);
            }
        }
    }
}