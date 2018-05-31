import os
import sys
import random

file = open("order.txt","w")
num_task = 12
num_session = 12
times_rand = num_task * 4

order = []

for m in range(num_session):
	o = []
	for i in range(num_task/3):
		o.append(1)
		o.append(2)
		o.append(3)
	for i in range(num_task/3*3,num_task):
		o.append(random.randint(1,3))

	for i in range(times_rand):
		j = random.randint(0,num_task-1)
		k = random.randint(0,num_task-1)
		t = o[j]
		o[j] = o[k]
		o[k] = t
	order.extend(o)

line = ""
for i in range(num_task*num_session):
	line += str(order[i]) + " "
line += "\n"

file.write(line)

num_session = 12
size = []
for i in range(num_session/3):
	size.append(1)
	size.append(2)
	size.append(3)
for i in range(times_rand):
	j = random.randint(0,num_session-1)
	k = random.randint(0,num_session-1)
	t = size[j]
	size[j] = size[k]
	size[k] = t
line = ""
for i in range(num_session):
	line += str(size[i]) + " "
line += "\n"
file.write(line)
file.close()