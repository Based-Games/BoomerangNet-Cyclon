import sys, os, json

args = sys.argv

if len(args) != 2:
    print("Please provide a path to your Mission.csv file!")
    sys.exit()

infile = args[1]
if not os.path.exists(infile):
    print("The path you've specified doesn't exist!")
    sys.exit()

infile = open(infile, 'r')
missions = infile.read()
infile.close()

sorted_missions = []

for mission in missions.split('\n'):
    mission = mission.split(',')
    sorted_missions.append({
        'missionid': mission[39],
        'gameid': int(mission[3]),
        'packid': int(mission[1]),
        'pack': mission[2],
        'difficulty': int(mission[5]),
        'title': mission[4],
        'cost': int(mission[6]),
        'open_level': int(mission[7]),
        'songs': [
            {
                'song': int(mission[12]),
                'chart': mission[15],
            },
            {
                'song': int(mission[13]),
                'chart': mission[16],
            },
            {
                'song': int(mission[14]),
                'chart': mission[17],
            }
        ],
        'rewards': {
            'bp': int(mission[31]),
            'exp': int(mission[33]),
            'djicon': int(mission[34]),
            'song': int(mission[35]),
            'pattern': mission[36]
        }
    })

outfile = open(args[1].replace('.csv', '.json'), 'w')
outfile.write(json.dumps(sorted_missions, indent=4))
print('donion ringz!')