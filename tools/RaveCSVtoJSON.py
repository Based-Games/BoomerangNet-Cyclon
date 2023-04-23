import sys, os, json

args = sys.argv

if len(args) != 3:
    print("Please provide a path to your RaveupAlbum.csv and RaveupStage.csv files (in that order)!")
    sys.exit()

infile1 = args[1]
infile2 = args[2]

if not os.path.exists(infile1):
    print("The path you've specified doesn't exist!")
    sys.exit()

if not os.path.exists(infile2):
    print("The path you've specified doesn't exist!")
    sys.exit()

infile1 = open(infile1, 'r', encoding='utf-8')
albums = infile1.read()
infile1.close()

infile2 = open(infile2, 'r', encoding='utf-8')
albumSongs = infile2.read()
infile2.close()

sorted_stages = {}
for stage in albumSongs.split('\n'):
    values = stage.split(',')
    songs = sorted_stages.get(int(values[1]), [])
    songs.append({
        'id': int(values[0]),
        'songId': int(values[2]),
        'chart': values[3]
    })
    sorted_stages[int(values[1])] = songs
    
sorted_sets = []

for album in albums.split('\n'):
    values = album.split(',')
    item = {
        'id': int(values[0]),
        'setName': values[1],
        'title': values[2],
        'subtitle': values[3],
        'difficulty': values[4],
        'serverId': values[5],
        'songs': sorted_stages.get(int(values[0]), {})
    }
    sorted_sets.append(item)

data = {
    'albums': sorted_sets
}

outfile = open(args[1].replace('.csv', '.json'), 'w')
outfile.write(json.dumps(data, indent=4))
print('donion ringz!')