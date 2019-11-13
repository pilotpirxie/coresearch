# coresearch
 .NET Core in-memory simple full text search library

Coresearch use inverted index with boosted trie data structure for indexing content to resources (words to resource meta information). Trie algorithm makes coresearch more elastic and allows both exact word querying and operations like wildcards and character matching. Entire prefix trie is store in memory for better performance.
