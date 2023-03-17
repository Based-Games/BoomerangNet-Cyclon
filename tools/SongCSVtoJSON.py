import sys, os, json

args = sys.argv

if len(args) != 2:
    print("Please provide a path to your Discstock.csv file!")
    sys.exit()

infile = args[1]
if not os.path.exists(infile):
    print("The path you've specified doesn't exist!")
    sys.exit()

infile = open(infile, 'r')
songs = infile.read()
infile.close()

sorted_songs = []

for song in songs.split('\n'):
    song = song.split(',')
    sorted_songs.append({
        'songid': song[39],
        'gameid': int(song[3]),
        'packid': int(song[1]),
        'pack': song[2],
        'difficulty': int(song[5]),
        'title': song[4],
        'cost': int(song[6]),
        'open_level': int(song[7]),
        'songs': [
            {
                'song': int(song[12]),
                'chart': song[15],
            },
            {
                'song': int(song[13]),
                'chart': song[16],
            },
            {
                'song': int(song[14]),
                'chart': song[17],
            }
        ],
        'rewards': {
            'bp': int(song[31]),
            'exp': int(song[33]),
            'djicon': int(song[34]),
            'song': int(song[35]),
            'pattern': song[36]
        }
    })

outfile = open(args[1].replace('.csv', '.json'), 'w')
outfile.write(json.dumps(sorted_songs, indent=4))
print('donion ringz!')