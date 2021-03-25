#!/usr/bin/env python3

import sys
import json

for line in sys.stdin:
    message = json.loads(line)
    print('%s\t%s' % (message['source'], 1))