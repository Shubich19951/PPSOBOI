#!/usr/bin/env python3

from operator import itemgetter
import sys

counter = {}

for line in sys.stdin:
    line = line.strip()
    source, count = line.split('\t', 1)
    try:
        count = int(count)
        counter[source] = counter.get(source, 0) + count
    except ValueError:
        pass


sorted_counter = sorted(counter.items(), key=itemgetter(0))

for word, count in sorted_counter:
    print('%s\t%s'% (word, count))