import sys
import os
import random
import math

radius = 2
username = sys.argv[1]
directory = "../dataExtracted/"+username+"/"
outdir = "../data2d/"+username+"/"
if not os.path.exists(outdir):
	os.mkdir(outdir)
files = os.listdir(directory)
for file in files:
	infile = open(directory+file,"r")
	outfile = open(outdir+file,"w")
	lines = infile.readlines()
	for line in lines:
		numbers = map(float, line.split(','))
		targetx = numbers[3]
		targety = numbers[4]
		targetz = numbers[5]
		cursorx = numbers[6]
		cursory = numbers[7]
		cursorz = numbers[8]
		target_alpha = math.asin(targety/radius)
		target_phi = math.atan2(targetz, targetx)
		cursor_alpha = math.asin(cursory/radius)
		cursor_phi = math.atan2(cursorz, cursorx)
		newline = ""
		for i in range(3):
			newline += str(int(numbers[i])) + ","
		newline += str(target_alpha) + "," + str(target_phi) + "," + str(cursor_alpha) + "," + str(cursor_phi) + "," + str(int(numbers[9])) + "," + str(int(numbers[10])) + "\n"
		outfile.write(newline)
