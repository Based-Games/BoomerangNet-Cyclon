# Simple touchLibrary.json creation tool
import os, sys, json, hashlib

args = sys.argv
if len(args) != 3:
    print("Please provide a path to your Cyclon install folder and version!")
    sys.exit()

gameFolder = args[1]
gameVersion = args[2]
if not os.path.exists(gameFolder):
    print("The path you've specified doesn't exist!")
    sys.exit()

entries = []

for subdir, dirs, files in os.walk(gameFolder):
    for filename in files:
        if filename in [".keep", 'config.json']:
            continue
        fullfilepath = subdir + os.sep + filename
        relfilepath = fullfilepath.replace(gameFolder, "").replace("\\", '/')

        with open(fullfilepath, 'rb') as file:
            entries.append({
                'name': f'../{relfilepath}',
                'hash': hashlib.md5(file.read()).hexdigest()
            })

data = {
    'hashes': entries,
    'systemVersion': gameVersion
}

cleanversion = gameVersion.replace('.', '_')

with open(f'{gameFolder}/hashList_{cleanversion}.json', 'w', encoding='UTF-8') as outFile:
    outFile.write(json.dumps(data, indent=4))

print('donion ringz!')
