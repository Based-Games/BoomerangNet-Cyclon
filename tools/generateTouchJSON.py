# Simple touchLibrary.json creation tool
import os, sys, json

args = sys.argv
if len(args) != 2:
    print("Please provide a path to your Touch folder!")
    sys.exit()

touchFolder = args[1]
if not os.path.exists(touchFolder):
    print("The path you've specified doesn't exist!")
    sys.exit()

entries = []

allfiles = os.listdir(touchFolder)
for f in allfiles:
    entries.append({
        'fileName': f,
        'name': '',
        'id': int(f.removesuffix('.wav'))
    })

data = {
    'keysounds': entries
}

with open(f'{touchFolder}/touchLibrary.json', 'w') as outFile:
    outFile.write(json.dumps(data, indent=4))

print('donion ringz!')