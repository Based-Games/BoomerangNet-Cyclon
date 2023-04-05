import sys, os, json

args = sys.argv

if len(args) != 2:
    print("Please provide a path to your HouseStage.csv file!")
    sys.exit()

infile = args[1]
if not os.path.exists(infile):
    print("The path you've specified doesn't exist!")
    sys.exit()

infile = open(infile, 'r', encoding='utf-8')
songs = infile.read()
infile.close()

sorted_songs = []

for index, song in enumerate(songs.split('\n')):
    values = song.split(',')
    item = {
        'id': index + 1,
        'songId': int(values[2]),
        'EZ': bool(int(values[3])),
        'NM': bool(int(values[4])),
        'HD': bool(int(values[5])),
        'PR': bool(int(values[6])),
        'MX': bool(int(values[7])),
    }
    sorted_songs.append(item)

outfile = open(args[1].replace('.csv', '.json'), 'w')
outfile.write(json.dumps(sorted_songs, indent=4))
print('donion ringz!')