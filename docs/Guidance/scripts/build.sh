asciidoc -d article -v -a icons index.asc &&
a2x -f chunked -d article -L -k -a linkcss -v --icons index.asc &&
a2x -f pdf -d article -L -v index.asc