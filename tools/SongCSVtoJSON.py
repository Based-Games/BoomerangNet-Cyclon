import sys, os, json

args = sys.argv

if len(args) != 2:
    print("Please provide a path to your Discstock.csv file!")
    sys.exit()

infile = args[1]
if not os.path.exists(infile):
    print("The path you've specified doesn't exist!")
    sys.exit()

infile = open(infile, 'r', encoding='utf-8')
songs = infile.read()
infile.close()

sorted_songs = []

for song in songs.split('\n'):
    song = song.split(',')
    item = {
            'id': int(song[0]),
            'fileName': song[1],
            'songTitle': song[2] if song[2] != '-' else '',
            'inGameTitleKr': song[34] if song[34] != '-' else '',
            'inGameTitleEn': song[35] if song[35] != '-' else '',
            'genre': song[3] if song[3] != '-' else '',
            'difficulty': int(song[4]),
            'BPM': int(song[5]),
            'composedBy': song[6] if song[6] != '-' else '',
            'arrangedBy': song[7] if song[7] != '-' else '',
            'vocalist': song[8] if song[8] != '-' else '',
            'artist': song[9] if song[9] != '-' else '',
            'ptInfo': song[10],
            'serverId': song[11],
            'EZ': int(song[12]) if song[12] != '-' else None,
            'NM': int(song[13]) if song[13] != '-' else None,
            'HD': int(song[14]) if song[14] != '-' else None,
            'PR': int(song[15]) if song[15] != '-' else None,
            'MX': int(song[16]) if song[16] != '-' else None,
            'S1': int(song[17]) if song[17] != '-' else None,
            'S2': int(song[18]) if song[18] != '-' else None,
            'EZNote': int(song[19]),
            'EZCombo': int(song[20]),
            'NMNote': int(song[21]),
            'NMCombo': int(song[22]),
            'HDNote': int(song[23]),
            'HDCombo': int(song[24]),
            'PRNote': int(song[25]),
            'PRCombo': int(song[26]),
            'MXNote': int(song[27]),
            'MXCombo': int(song[28]),
            'S1Note': int(song[29]),
            'S1Combo': int(song[30]),
            'S2Note': int(song[31]),
            'S2Combo': int(song[32]),
            'groupSet': int(song[33]),
        }
    sorted_songs.append(item)

outfile = open(args[1].replace('.csv', '.json'), 'w')
outfile.write(json.dumps(sorted_songs, indent=4))
print('donion ringz!')