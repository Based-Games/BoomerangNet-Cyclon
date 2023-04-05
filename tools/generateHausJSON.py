import sys, os, json

args = sys.argv

if len(args) != 3:
    print("Please provide a path to your musicLibrary.json file and output folder!")
    sys.exit()

infile = args[1]
outfolder = args[2]
if not os.path.exists(infile):
    print("The path you've specified doesn't exist!")
    sys.exit()

if not os.path.exists(outfolder):
    print("The path you've specified doesn't exist!")
    sys.exit()

stages = []

with open(infile, 'r') as jsonFile:
    song_json: dict = json.loads(jsonFile.read())
    for index, song_data in enumerate(song_json.get('songs', [])):
        song_data: dict

        for index2 in range(4):
            stage_data = {
                'id': index + (index2*100),
                'stage': index2 + 1,
                'songId': song_data.get('id')
            }

            difficulties = ['EZ', 'NM', 'HD', 'PR', 'MX', 'S1', 'S2']
            pt_list = song_data.get('ptInfo', '').split('_')
            pt_diffs = []

            for pt in pt_list:
                pt = pt.split('-')[0]
                pt_diffs.append(pt)
            
            for difficulty in difficulties:
                if difficulty in pt_diffs:
                    stage_data[difficulty] = True
                else:
                    stage_data[difficulty] = False

            stages.append(stage_data)

out_data = {
    'stages': stages
}

with open(f'{outfolder}/hausStages.json', 'w') as outFile:
    outFile.write(json.dumps(out_data, indent=4))

print('donion ringz!')