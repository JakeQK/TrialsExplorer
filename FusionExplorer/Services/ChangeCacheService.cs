using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionExplorer.Services
{
    public class ChangeCacheService
    {
        // Use a Dictionary for efficient key-based lookups
        public Dictionary<string, ChangeCache> _cachedChanges = new Dictionary<string, ChangeCache>();

        public void Create(string name, string originalContent, string newContent)
        {
            // Use TryGetValue for efficient lookup without iterating
            if (_cachedChanges.TryGetValue(name, out ChangeCache changeCache))
            {
                // The item already exists, just update the latest content
                changeCache.NewContent = newContent;
            }
            else
            {
                // First time seeing this name, add a new entry
                _cachedChanges.Add(name, new ChangeCache
                {
                    OriginalContent = originalContent,
                    NewContent = newContent
                });
            }
        }

        // A method to retrieve the tracked change
        public ChangeCache GetChange(string name)
        {
            _cachedChanges.TryGetValue(name, out ChangeCache changeCache);
            return changeCache;
        }
    }

    public class ChangeCache
    {
        public string OriginalContent { get; set; }
        public string NewContent { get; set; }
    }
}
