import sys
import os
import random

username = sys.argv[1]
directory = "../userdata/"
outdir = "../dataExtracted/"+username+"/"
if not os.path.exists(outdir):
	os.mkdir(outdir)
infile = open(directory+"target_"+username+".csv","r")
lines = infile.readlines()
num_tasks = 12 * 18 * 10
len_task = 50
num_all = len(lines)
start_index = [i for i in range(num_all - len_task)]
start_slice = random.sample(start_index, num_tasks)
for index in start_slice:
	data = map(float, lines[index].split(','))
	if int(data[1]) == 1:
		continue
	outfile = open(outdir+"4_"+str(index)+".txt","w")
	for j in range(len_task):
		outfile.write(lines[index+j])
