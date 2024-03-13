import os
import sys
import xml.etree.ElementTree as ET
from datetime import date

class High5toCyclon():
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

    def filterDoubleNotes(self, track: ET.Element):
        '''
        Given the `track` tree, filter out all double-bound notes.

        Mostly for hold/jog note situations. If a note is set whilst a hold note is active, move it elsewhere.
        '''
        hold_bound = [] # Gross array of all hold-bound ticks.
        for note in track.findall('./note'):
            try:
                dur = int(note.attrib['dur']) + 20
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

    def sortHearts(self, hearts: dict[int, list[ET.Element]], root: ET.Element):
        '''
        Given the hearts dict and root, sort and set proper direction for notes.
        '''
        total_notes: list[ET.Element] = []
        sorted_notes: dict[int, list[ET.Element]] = {}
        for track in root.findall('./note_list/track'):
            track_id = int(track.attrib['idx'])
            for note in hearts[track_id]:
                note.attrib['track'] = str(track_id)
                total_notes.append(note)

        total_notes = sorted(total_notes, key=lambda x: int(x.attrib['tick']))
        
        for note_id in range(len(total_notes)):
            note = total_notes[note_id]
            track_id = int(note.attrib['track'])
            track_notes = sorted_notes.get(track_id, [])
            if note_id == 0: # The first note
                note.attrib['attr'] = '10'

            elif note_id + 1 < len(total_notes):
                next_note = total_notes[note_id + 1]
                if int(next_note.attrib['tick']) - int(note.attrib['tick']) > 90: # Too far! Match the last note.
                    last_note = total_notes[note_id - 1]
                    note.attrib['attr'] = last_note.attrib['attr']

                else:
                    next_track_id = int(next_note.attrib['track'])
                    if next_track_id > track_id:
                        note.attrib['attr'] = '10'  # Up
                    else:
                        note.attrib['attr'] = '11'  # Down

            elif note_id + 1 == len(total_notes):
                note.attrib['attr'] = total_notes[note_id - 1].attrib['attr']

            track_notes.append(note)
            sorted_notes[track_id] = track_notes

        for track in root.findall('./note_list/track'):
            track_id = int(track.attrib['idx'])
            try:
                for note in sorted_notes[track_id]:
                    track.append(note)
            except KeyError:
                pass
        return root

    def SortNotes(self, root: ET.Element):
        '''
        Sort the entire root.
        '''
        for track in root.findall('./note_list/track'):
            sorted_notes = sorted(track.findall('note'), key=lambda x: int(x.attrib['tick']))
            idx = track.attrib['idx']
            track.clear()
            track.attrib['idx'] = idx

            for note in sorted_notes:
                track.append(note)

            track = self.filterDoubleNotes(track)
        return root

    def ParseChart(self, sourcePath): 
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
                song_info.attrib['end_tick'] = str(int(song_info.attrib['tick']) + 10)

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
                        track.append(ET.Element('note', attrib={'tick': song_info.attrib['tick'], 'ins': '2', 'attr': '100'}))

                # Create the hearts dict
                all_hearts = {}

                # Convert Notes
                for track in root.findall('./note_list/track'):
                    # Reassign track
                    track.attrib['idx'] = str(self.chart_map[int(track.attrib['idx'])])
                    track_hearts = []

                    # Edit attrs
                    hold_start_tick = None
                    notes_to_remove = []
                    hold_ins = None
                    previous_heart: ET.Element = None
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
                                if duration > 1500:
                                    duration = 1500
                                combined_note = ET.Element('note', tick=str(hold_start_tick), ins=hold_ins, dur=str(duration))
                                track.append(combined_note)

                                hold_start_tick = None
                                hold_ins = None
                                notes_to_remove.append(note)

                            elif attr == 5:
                                if previous_heart is not None:
                                    if int(note.attrib['tick']) - int(previous_heart.attrib['tick']) > 1536:
                                        track_hearts.append(note)
                                
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

                        all_hearts[int(track.attrib['idx'])] = track_hearts

                    new_holds = holds.copy()
                    for i in range(len(holds)):
                        for j in range(0, len(holds[i])):
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
                    
                    for note_to_remove in notes_to_remove:
                        track.remove(note_to_remove)
        
        # Sort heart notes.
        root = self.sortHearts(all_hearts, root)

        # Sort the entire chart.
        root = self.SortNotes(root)

        root.append(ET.Comment('Converted to Cyclon via High5toCyclon'))
        ET.indent(root)
        return ET.tostring(root)

if __name__ == '__main__':
    if len(sys.argv) != 2:
        print('usage: high5_to_cyclon.py <FILE TO CONVERT>')
        sys.exit()

    filepath = sys.argv[1]
    if os.path.exists(filepath):
        with open(filepath.replace('.xml', '_conv.xml'), 'wb') as outFile:
            outFile.write(High5toCyclon.ParseChart(High5toCyclon(), filepath))

        print("Done, enjoy!")
    else:
        print("File not found!")