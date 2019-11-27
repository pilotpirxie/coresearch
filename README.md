<p align="center">
 <img src="https://i.imgur.com/EUHKODL.png" alt="Coresearch"/>
</p>

# coresearch
 .NET Core cross-platform, in-memory, full text search library for building search engines

## About
**Coresearch** uses an inverted index with a boosted trie data structure for indexing atomic search criterion from content to resources. Trie algorithm makes Coresearch more elastic and allows both exact word querying and operations like fuzzy search, wildcards and character matching. Entire trie structure is stored in memory for better performance. The entire project was written as a learning project so be aware of putting in on the production 😉

<p align="center">
 <img src="https://i.imgur.com/w3BSIkm.gif" alt="Trie"/>
</p>

## Features
* Efficient **inverted index** algorithm (mapping from content to resources)
* Uses **Prefix Tree (Trie)** data structure for quick search and horizontal scaling
* **Exact search** with super fast O(k) worst case time, where k is the key length 
* **Wildcard search** for recursive depth-first search tree traversal  
* **Character match** search with deep level k+1
* Simple **command line** tool and **library**
* **Cross-platform**, runs on Linux, MacOS and Windows
* Easily handle **large amount of different resources**

<p align="center">
 <img src="https://upload.wikimedia.org/wikipedia/commons/thumb/b/be/Trie_example.svg/1024px-Trie_example.svg.png" alt="Trie"/>
</p>

[Trie on Wikipedia](https://en.wikipedia.org/wiki/Trie)

<p align="center">
 <img src="https://i.imgur.com/KtePNXo.gif" alt="Trie"/>
</p>

## CLI commands
### Load data from files in specific path and extension (recursive)
```
source <string path> <string extension>
alias: load
```
example:
```
> source ./ *.txt
```

### Get resource names for exact key (word)
```
get <string key>
alias: search
```

example:
```
> search cars
output:
<resource names for key cars>
```

### Search for every resource name which key starts with prefix 

Query modes:
* Question mark (?) is for select of all resource names belong to children of specific node (if any).
* Wildcard sign (\*) is for select of all resource names recursively under specific prefix.
* Exact matching (without any sign) produces equal output as get/search command.
```
query <string prefix> <query mode: . or *>
```

example 1: 
```
> query c ?
output: 
<resource names for keys: ca, cb, c5, co, c1, ...>
```
example 2: 
```
> query c *
output: 
<resource names for keys: ca, cabbage, c4a541, cars, cardio, cantaloupe, ...>
```
example 3: 
```
> query cars
output: 
<resource names for key cars>
```

### Add resource under key (word)
```
add <string resource name> <string content>
alias: insert
```
example:
```
> add english-dict.txt house 
```

### Remove specific key (word)
```
delete <string key>
```

example:
```
> delete sport
```

### Echo
```
echo <string content>
```

example:
```
> echo hello
output:
hello
```

### Turn on/off debug mode
```
debug <bool>
```

example:
```
> debug true
```

### Remove every node in structure and collect memory
```
flush
```

example:
```
> flush
```

### Show information
```
info
```

example:
```
> flush
output:
Nodes in trie: 4651175
Words inserted: 15513389
Resource files: 2227
Memory usage: 1044944640 bytes
```

### Clear console
```
clear
```

example:
```
> clear
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
