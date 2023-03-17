import sys, os, json

args = sys.argv

if len(args) != 2:
    print("Please provide a path to your icon.csv file!")
    sys.exit()

infile = args[1]
if not os.path.exists(infile):
    print("The path you've specified doesn't exist!")
    sys.exit()

infile = open(infile, 'r')
in_icons = infile.read()
infile.close()

icons = []

for icon in in_icons.split('\n'):
    icon = icon.split(',')
    icons.append({
        'id': int(icon[0]),
        'name': icon[1],
        'gameid': int(icon[2]),
    })

outfile = open(args[1].replace('.csv', '.json'), 'w')
outfile.write(json.dumps(icons, indent=4))
print('donion ringz!')