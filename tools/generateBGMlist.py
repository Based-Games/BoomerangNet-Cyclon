# Simple BGMLibrary.json creation tool
import os, sys, json

args = sys.argv
if len(args) != 2:
    print("Please provide a path to your BGM folder")
    sys.exit()

folder = args[1]
if not os.path.exists(folder):
    print("The path you've specified doesn't exist!")
    sys.exit()

entries = {}

pack_id = 0
for subdir, dirs, files in os.walk(folder):
    for filename in files:
        if filename.endswith('.wav'):
            fullfilepath = subdir + os.sep + filename
            relfilepath = fullfilepath.replace(folder, "").replace("\\", '/')

            file_split = relfilepath.split('/')
            if len(file_split) == 3:
                entry_dir = entries.get(file_split[1], {})
                entry_dir['id'] = pack_id
                sounds = entry_dir.get('sounds', [])
                sounds.append(filename.removesuffix('.wav'))
                entry_dir['sounds'] = sounds
                entries[file_split[1]] = entry_dir
    pack_id += 1


with open(f'{folder}/BGMLibrary.json', 'w', encoding='utf-8') as outFile:
    outFile.write(json.dumps(entries, indent=4))

print('donion ringz!')
