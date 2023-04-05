import sys, os, json

args = sys.argv

if len(args) != 2:
    print("Please provide a path to your Common.csv file!")
    sys.exit()

infile = args[1]
if not os.path.exists(infile):
    print("The path you've specified doesn't exist!")
    sys.exit()

infile = open(infile, 'r')
in_text = infile.read()
infile.close()

items = []

for item in in_text.split('\n'):
    item = item.split(',')
    items.append({
        'id': int(item[0]),
        'name': item[1],
        'serverId': int(item[2]),
    })

outfile = open(args[1].replace('.csv', '.json'), 'w')
outfile.write(json.dumps(item, indent=4))
print('donion ringz!')