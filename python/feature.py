import sys
import os
import random
import math
import numpy as np

radius = 2
username = sys.argv[1]
directory = "../data2d/"+username+"/"
outdir = "../features/" + username + "/"
if not os.path.exists(outdir):
	os.mkdir(outdir)
files = os.listdir(directory)
for file in files:
	infile = open(directory+file,"r")
	pre = file.split('_')
	if pre[0] == '4':
		outfile = open(outdir+"4.txt","a")
	else:
		outfile = open(outdir+pre[0]+"_"+pre[1]+".txt","a")
	lines = infile.readlines()
	cursors = []
	insides = []
	intentions = []
	for line in lines:
		numbers = map(float, line.split(','))
		if len(numbers) < 8:
			break
		cursors.append(np.array([numbers[5],numbers[6]]))
		intentions.append(int(numbers[7]))
		insides.append(int(numbers[8]))
	newline = ""
	length = len(cursors)
	newline += str(length) + "," # length, time
	newline += str(np.linalg.norm(cursors[length-1]- cursors[0])) + "," # distance of begin and end
	indirection = cursors[length/2] - cursors[0]
	outdirection = cursors[length/2] - cursors[length-1]
	newline += str(np.linalg.norm((indirection+outdirection)/2)) + "," # the length of the cross
	newline += str(math.acos(np.dot(indirection,outdirection)/np.linalg.norm(indirection)/np.linalg.norm(outdirection))) + ","
	speeds = []
	for i in range(length-1):
		speeds.append(np.linalg.norm(cursors[i+1] - cursors[i]))
	speedup = 0
	speedaverage = 0
	speeddeviation = 0
	for i in range(length-2):
		if speeds[i+1] > speeds[i]:
			speedup += 1.0 / (length-2)
	for speed in speeds:
		speedaverage += speed / len(speeds)
	for speed in speeds:
		speeddeviation += (speedaverage - speed) * (speedaverage - speed)
	speeddeviation = math.sqrt(speeddeviation/len(speeds))
	newline += str(speedaverage) + "," + str(speeddeviation) + "," + str(speedup) #speeds
	newline += "\n"
	outfile.write(newline)


