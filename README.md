# coresearch
 .NET Core cross-platform, in-memory, simple, full text search library

About
-----
Coresearch uses an inverted index with a boosted trie data structure for indexing atomic search criterion from content to resources. Trie algorithm makes Coresearch more elastic and allows both exact word querying and operations like fuzzy search, wildcards and character matching. Entire trie structure is stored in memory for better performance. The entire project was written as a learning project so be aware of putting in on the production 😉

## CLI commands
Load data from files in specific path and extension (recursive)
```
source <string path> <string extension>
alias: load
```

Get resource name from specific word
```
get <string key>
alias: search
```

Add resource with content
```
add <string resource name> <string content>
alias: insert
```

Remove specific word (key)
```
delete <string key>
```

Echo
```
echo <string content>
```

Turn on/off debug mode
```
debug <bool>
```

Remove every word (key) and collect memory
```
flush
```

Show information
```
info
```

Clear console
```
clear
```

## CLI arguments
Turn debug mode
```
--debug <bool>
-d
```

Set max size of memory (in bytes)
```
--memory-limit <int>
-m
```

Pre-process every word before insert
```
--normalize <bool>
-n
```

Pattern for removing unwanted characters, used for each word before insert
```
--pattern <regex>
-p
```

Load data from specific path at start
```
--source <string>
-s
```

Set extension for loading data at start
```
--extension <string>
-e
```


License
-----
coresearch is licensed under the MIT.
