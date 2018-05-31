import os
import sys
import random
import numpy as np

num_task = 12
num_session = 12

num_circle = 36
radius = 4.2654*2
xmin = -30
xmax = 30
ymin = -15
ymax = 45

xpos = []
ypos = []
xpos.append(random.uniform(xmin,xmax))
ypos.append(random.uniform(ymin, ymax))
for i in range(1,num_circle):
	while True:
		x = random.uniform(xmin,xmax)
		y = random.uniform(ymin, ymax)
		no_conflict = True
		for j in range(i):
			distance = np.sqrt((xpos[j]-x)*(xpos[j]-x)+(ypos[j]-y)*(ypos[j]-y))
			if distance < radius:
				no_conflict = False
				break
		if no_conflict == True:
			xpos.append(x)
			ypos.append(y)
			break
outfile = open("position20.txt","w")
xline = ""
yline = ""
for i in range(num_circle):
	xline += str(xpos[i]) + " "
	yline += str(ypos[i]) + " "
outfile.write(xline+"\n")
outfile.write(yline+"\n")


