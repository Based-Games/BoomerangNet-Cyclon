import os
import sys
import xml.etree.ElementTree as ET
from datetime import date

chart_map = {
    0: 0,
    1: 1,
    2: 2, 
    3: 3,
    4: 4,
    5: 4,
    6: 5, 
    7: 6, 
    8: 6, 
    9: 7, 
    10: 7,
    11: 8, 
    12: 9, 
    13: 10, 
    14: 11,
    31: 31
}

def filterDoubleNotes(track: ET.Element):
    '''
    Given the `track` tree, filter out all double-bound notes.

    Mostly for hold/jog note situations. If a note is set whilst a hold note is active, move it elsewhere.
    '''
    hold_bound = [] # Gross array of all hold-bound ticks.
    for note in track.findall('./note'):
        try:
            dur = int(note.attrib['dur'])
            if dur is not None:
                start_tick = int(note.attrib['tick'])
                for tick in range(start_tick, start_tick + dur):
                    hold_bound.append(tick)
                continue
        except KeyError:
            pass

        try:
            tick = int(note.attrib['tick'])
            if tick is not None:
                if tick in hold_bound:
                    print(f'Removed double-bound @ {tick} track {track.attrib['idx']}')
                    track.remove(note)
        except KeyError:
            pass
    return track

def ParseChart(sourcePath): 
    tree = ET.parse(sourcePath) 
    root = tree.getroot()

    # Add in Tempo data and edit Header
    header = root.find('./header')
    if header is not None:
        version = header.find('./version')
        if version is not None:
            version.attrib['date'] = str(date.today())

        song_info = header.find('./songinfo')
        if song_info is not None:
            song_info.attrib['start_tick'] = '0'
            song_info.attrib['end_tick'] = str(int(song_info.attrib['tick']) + 100)

            root_tempo_element = ET.Element('tempo')
            tempo_element = ET.Element('tempo', attrib={'tick': '0', 'tempo': song_info.attrib['tempo'], 'tps': song_info.attrib['tps']})
            root_tempo_element.append(tempo_element)
            root.append(root_tempo_element)

            # Filter Notes
            for track in root.findall('./note_list/track'):
                idx = int(track.attrib['idx'])
                if idx >= 15 and idx != 31:
                    root.find('./note_list').remove(track)

                elif idx == 31:
                    end_sound = ET.Element('note', attrib={'tick': song_info.attrib['tick'], 'ins': '2', 'attr': '100'})
                    track.append(end_sound)

            # Convert Notes
            for track in root.findall('./note_list/track'):
                # Reassign track
                track.attrib['idx'] = str(chart_map[int(track.attrib['idx'])])

                # Edit attrs
                hold_start_tick = None
                notes_to_remove = []
                hold_ins = None
                previous_heart: ET.Element = None
                hearts = []
                holds: list[ET.Element] = []

                for note in track.findall('./note'):
                    try:
                        attr = int(note.attrib['attr'])
                        if attr in [1, 11]:
                            hold_start_tick = int(note.attrib['tick'])
                            hold_ins = note.attrib['ins']
                            notes_to_remove.append(note)
                        
                        elif attr in [3, 13] and hold_start_tick is not None and hold_ins == note.attrib['ins']:
                            end_tick = int(note.attrib['tick'])
                            duration = end_tick - hold_start_tick

                            # Create a new note
                            combined_note = ET.Element('note', tick=str(hold_start_tick), ins=hold_ins, dur=str(duration))
                            track.append(combined_note)

                            hold_start_tick = None
                            hold_ins = None
                            notes_to_remove.append(note)

                        elif attr == 5:
                            hearts.append(note)
                            if previous_heart is not None:
                                if int(note.attrib['tick']) - int(previous_heart.attrib['tick']) > 1536:
                                    holds.append(hearts)
                                    hearts = []
                            
                            previous_heart = note
                            track.remove(note)
                    except KeyError:
                        pass

                    try:
                        ins = int(note.attrib['ins'])
                        if ins == 3:
                            track.remove(note)
                    except KeyError:
                        pass

                new_holds = holds.copy()
                for i in range(len(holds)):
                    for j in range(0, len(holds[i])):
                        if holds[i][j].attrib['attr'] == '5':  # hearts
                            new_holds[i][j].attrib['attr'] = '10'
                            # if j + 1 < len(holds[i]):
                            #     if int(holds[i][j + 1].attrib['ins']) - int(holds[i][j].attrib['ins']) > 0:  # ccw
                            #         new_holds[i][j].attrib['attr'] = '10'
                            #     else:
                            #         new_holds[i][j].attrib['attr'] = '11'

                            # elif j + 1 == len(holds[i]):  # last note in the "chain"
                            #     new_holds[i][j].attrib['attr'] = new_holds[i][j - 1].attrib['attr']
                            
                        if int(holds[i][j].attrib['attr']) > 10: #B (11~13)
                            new_holds[i][j].attrib['attr'] = str(int(new_holds[i][j].attrib['attr']) - 6)

                        if j==0: continue

                        if int(holds[i][j].attrib['ins']) - int(holds[i][j - 1].attrib['ins']) > 0:
                            new_holds[i][j].attrib['attr'] = '2' if 1 <= int(holds[i][j].attrib['attr']) <= 3 else '6'
                        else: #cw
                            new_holds[i][j].attrib['attr'] = '3' if 1 <= int(holds[i][j].attrib['attr']) <= 3 else '7'

                for hold in new_holds:
                    for node in hold:
                        track.append(node)

                # Sort the notes
                sorted_notes = sorted(track.findall('note'), key=lambda x: int(x.attrib['tick']))
                idx = track.attrib['idx']
                track.clear()
                track.attrib['idx'] = idx

                for note in sorted_notes:
                    # Shrink hold notes to reasonable values
                    try:
                        dur = int(note.attrib['dur'])
                        if dur > 2500:
                            note.attrib['dur'] = '1250'
                    except KeyError:
                        pass

                    track.append(note)

                # Filter out double binds
                track = filterDoubleNotes(track)
                
                for note_to_remove in notes_to_remove:
                    track.remove(note_to_remove)
    
    root.append(ET.Comment('Converted to Cyclon via High5toCyclon'))
    ET.indent(root)
    return ET.tostring(root)

if len(sys.argv) != 2:
    print('usage: high5_to_cyclon.py <FILE TO CONVERT>')
    sys.exit()

filepath = sys.argv[1]
if os.path.exists(filepath):
    with open(filepath.replace('.xml', '_conv.xml'), 'wb') as outFile:
        outFile.write(ParseChart(filepath))

    print("Done, enjoy!")
else:
    print("File not found!")