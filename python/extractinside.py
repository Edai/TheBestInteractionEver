import sys
import os

username = sys.argv[1]
directory = "../userdata/"
outdir = "../dataExtracted/"+username+"/"
if not os.path.exists(outdir):
	os.mkdir(outdir)
infile = open(directory+"target_"+username+".csv","r")
lines = infile.readlines()
num_tasks = 12 * 12
cursor = 0
for i in range(num_tasks):
	data = map(float, lines[cursor].split(','))
	outfile = open(outdir+str(int(data[1]))+"_"+str(int(data[2]))+"_"+str(int(data[0]))+".txt","w")
	if int(data[1]) == 1:
		while int(data[9]) != 1:
			cursor += 1
			data = map(float, lines[cursor].split(','))
		while int(data[9]) == 1: 
			outfile.write(lines[cursor])
			cursor += 1
			data = map(float, lines[cursor].split(','))
	elif int(data[1]) == 2:
		while True:
			while int(data[10]) != 1:
				cursor += 1
				data = map(float, lines[cursor].split(','))
			lines_write = []
			count = 0
			while int(data[10]) == 1:
				lines_write.append(lines[cursor])
				count += 1
				cursor += 1
				data = map(float, lines[cursor].split(','))
			if count >= 100: #dwell
				for j in range(len(lines_write)):
					outfile.write(lines_write[j])
				break
	elif int(data[1]) == 3:
		while int(data[10]) != 1:
			cursor += 1
			data = map(float, lines[cursor].split(','))
		while int(data[10]) == 1: 
			outfile.write(lines[cursor])
			cursor += 1
			data = map(float, lines[cursor].split(','))
	while int(data[0]) == i:
		cursor += 1
		data = map(float, lines[cursor].split(','))
	cursor+=1

