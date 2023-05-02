import sys, os, json

args = sys.argv

if len(args) != 2:
    print("Please provide a path to your output folder!")
    sys.exit()

outfolder = args[1]

if not os.path.exists(outfolder):
    print("The path you've specified doesn't exist!")
    sys.exit()

stages = []

for i in range(200):

    for index2 in range(4):
        stage_data = {
            'id': i + (index2*1000),
            'stage': index2 + 1,
            'songId': 3
        }

        difficulties = ['EZ', 'NM', 'HD', 'PR', 'MX', 'S1', 'S2']
        pt_diffs = [
            'EZ',
            'NN',
            'HD',
            'PR',
            'MX'
        ]
        
        for difficulty in difficulties:
            if difficulty in pt_diffs:
                stage_data[difficulty] = True
            else:
                stage_data[difficulty] = False

        stages.append(stage_data)

out_data = {
    'stages': stages
}

with open(f'{outfolder}/hausStagesGetDown.json', 'w') as outFile:
    outFile.write(json.dumps(out_data, indent=4))

print('donion ringz!')